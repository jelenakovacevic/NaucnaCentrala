using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NaucnaCentrala.API.DTOs;
using NaucnaCentrala.Core.Enums;
using NaucnaCentrala.Core.Models;
using NaucnaCentrala.Data;
using NaucnaCentrala.Domain.Entities;
using NaucnaCentrala.Interfaces.Services;
using Newtonsoft.Json;

namespace NaucnaCentrala.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        private readonly ICamundaService _camundaService;
        private readonly IEmailService _emailService;
        private readonly DataContext _dbContext;
        private readonly string _jwtSecret = "2124hj421gh214f241";

        public KorisnikController(ICamundaService camundaService, IEmailService emailService, DataContext dbContext)
        {
            _camundaService = camundaService;
            _emailService = emailService;
            _dbContext = dbContext;
        }

        [HttpPost("registruj-se")]
        public IActionResult Register([FromBody] RegistracijaDTO registracijaDTO)
        {
            try
            {
                _camundaService.StartProcess("RegistracijaProces", null);

                string taskId = _camundaService.GetUnassignedTaskId("Task_UnosPodataka");

                dynamic content = new
                {
                    korisnickoIme = new CamundaVariable<string>(registracijaDTO.KorisnickoIme),
                    lozinka = new CamundaVariable<string>(registracijaDTO.Lozinka),
                    ime = new CamundaVariable<string>(registracijaDTO.Ime),
                    prezime = new CamundaVariable<string>(registracijaDTO.Prezime),
                    grad = new CamundaVariable<string>(registracijaDTO.Grad),
                    drzava = new CamundaVariable<string>(registracijaDTO.Drzava),
                    titula = new CamundaVariable<string>(registracijaDTO.Titula),
                    recenzent = new CamundaVariable<bool>(registracijaDTO.Recenzent),
                    naucneOblasti = new CamundaVariable<string>(string.Join(",", registracijaDTO.NaucneOblasti.Select(x => x.Naziv)))
                };

                bool success = _camundaService.SubmitTaskForm(taskId, JsonConvert.SerializeObject(content));

                if (success)
                {
                    _dbContext.Korisnici.Add(new Korisnik
                    {
                        KorisnickoIme = registracijaDTO.KorisnickoIme,
                        Lozinka = registracijaDTO.Lozinka,
                        Ime = registracijaDTO.Ime,
                        Prezime = registracijaDTO.Prezime,
                        Grad = registracijaDTO.Grad,
                        Drzava = registracijaDTO.Drzava,
                        Titula = registracijaDTO.Titula,
                        Recenzent = registracijaDTO.Recenzent,
                        NaucneOblasti = string.Join(",", registracijaDTO.NaucneOblasti.Select(x => x.Naziv)),
                        Token = Guid.NewGuid().ToString("n"),
                        Uloga = registracijaDTO.Recenzent ? Roles.Reviewer.ToString() : Roles.User.ToString()
                    });
                    _dbContext.SaveChanges();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("uloguj-se")]
        public IActionResult Authenticate([FromBody]LogovanjeDTO loginDTO)
        {
            var korisnik = _dbContext.Korisnici.FirstOrDefault
                (x => x.KorisnickoIme == loginDTO.KorisnickoIme && x.Lozinka == loginDTO.Lozinka);

            if (korisnik == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, korisnik.KorisnickoIme.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(tokenString);
        }

        [Route("potvrdi-email")]
        [HttpGet]
        public IActionResult VerifyEmail([FromQuery] string token)
        {
            var user = _dbContext.Korisnici.FirstOrDefault(x => x.Token == token);

            if (user == null)
                return BadRequest();

            user.EmailPotvrdjen = true;
            _dbContext.SaveChanges();

            var taskId = _camundaService.GetUnassignedTaskId("Task_PotrvdaEmail");

            dynamic content = new { };
            var success = _camundaService.CompleteTask(taskId, JsonConvert.SerializeObject(content));
            if (success)
                return Ok("Verifikacija email-a je uspesna.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route("uloga")]
        [HttpGet]
        public IActionResult GetRole()
        {
            var username = User.Identity.Name;
            var role = _dbContext.Korisnici.FirstOrDefault(x => x.KorisnickoIme == username)?.Uloga;
            return Ok(role);
        }

        [Route("dobavi-korisnike-za-potvrdu")]
        [HttpGet]
        public IActionResult GetReviewersWaiting()
        {
            var taskId = _camundaService.GetAssignedTaskId("Task_PotvrdaRecenzenta", "admin1");

            var users = _dbContext.Korisnici.Where(x => x.AdminPotvrdio == false && x.Recenzent == true && x.EmailPotvrdjen == true);
            return Ok(users);
        }

        [Route("urednici/{id}")]
        [HttpGet]
        public IActionResult GetEditors(int id)
        {
            var magazineScientificAreas = _dbContext.Casopisi.FirstOrDefault(x => x.Id == id).NaucneOblasti.Split(",");
            var users = _dbContext.Korisnici.ToList();

            var filteredUsers = users.Where(
                            x => x.AdminPotvrdio == true
                            && x.Uloga == Roles.Editor.ToString()
                            && x.KorisnickoIme != User.Identity.Name
                            && x.NaucneOblasti.Split(",", StringSplitOptions.None).Select(y => y).Intersect(magazineScientificAreas).Any());

            return Ok(filteredUsers.Select(x => x.KorisnickoIme).ToList());
        }

        [Route("recenzenti/{id}")]
        [HttpGet]
        public IActionResult GetReviewers(int id)
        {
            var magazineScientificAreas = _dbContext.Casopisi.FirstOrDefault(x => x.Id == id).NaucneOblasti.Split(",");
            var users = _dbContext.Korisnici.ToList();
            var filteredUsers = users.Where(
                x => x.AdminPotvrdio == true 
                && x.Uloga == Roles.Reviewer.ToString()
                && x.NaucneOblasti.Split(",", StringSplitOptions.None).Select(y => y).Intersect(magazineScientificAreas).Any());

            return Ok(filteredUsers.Select(x => x.KorisnickoIme).ToList());
        }

        [Route("odobri/{id}")]
        [HttpPost]
        public IActionResult ApproveReviewer(int id)
        {
            var taskId = _camundaService.GetAssignedTaskId("Task_PotvrdaRecenzenta", "admin1");

            dynamic content = new { };
            var success = _camundaService.CompleteTask(taskId, JsonConvert.SerializeObject(content));

            return Ok();
        }
    }
}
