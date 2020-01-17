using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NaucnaCentrala.API.DTOs;
using NaucnaCentrala.Core.Models;
using NaucnaCentrala.Data;
using NaucnaCentrala.Domain.Entities;
using NaucnaCentrala.Interfaces.Services;
using Newtonsoft.Json;

namespace NaucnaCentrala.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CasopisController : ControllerBase
    {

        private readonly ICamundaService _camundaService;
        private readonly IEmailService _emailService;
        private readonly DataContext _dbContext;

        public CasopisController(ICamundaService camundaService, IEmailService emailService, DataContext dbContext)
        {
            _camundaService = camundaService;
            _emailService = emailService;
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult Create([FromBody] KreiranjeCasopisaDTO kreiranjeCasopisaDTO)
        {
            try
            {
                _camundaService.StartProcess("KreiranjeCasopisaProces", User.Identity.Name);

                string taskId = _camundaService.GetUnassignedTaskId("Task_UnosPodatakaCasopis");

                dynamic content = new
                {
                    naziv = new CamundaVariable<string>(kreiranjeCasopisaDTO.Naziv),
                    issnBroj = new CamundaVariable<string>(kreiranjeCasopisaDTO.ISSNbroj),
                    openAccess = new CamundaVariable<bool>(kreiranjeCasopisaDTO.OpenAccess),
                    naucnaOblast = new CamundaVariable<string>(string.Join(",", kreiranjeCasopisaDTO.NaucneOblasti.Select(x => x.Naziv)))
                };

                bool success = _camundaService.SubmitTaskForm(taskId, JsonConvert.SerializeObject(content));

                if (success)
                {
                    _dbContext.Casopisi.Add(new Casopis
                    {
                        Naziv = kreiranjeCasopisaDTO.Naziv,
                        ISSNbroj = kreiranjeCasopisaDTO.ISSNbroj,
                        OpenAccess = kreiranjeCasopisaDTO.OpenAccess,
                        NaucneOblasti = string.Join(",", kreiranjeCasopisaDTO.NaucneOblasti.Select(x => x.Naziv)),
                        Aktivan = false,
                        GlavniUrednik = User.Identity.Name
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

        [Route("koriguj/{id}")]
        [HttpPost]
        public IActionResult Correct(int id, [FromBody] KorigovanjeCasopisaDTO korigovanjeCasopisaDTO)
        {
            try
            {
                string taskId = _camundaService.GetUnassignedTaskId("Task_DopunaPodataka");

                dynamic content = new
                {
                    issnBroj = new CamundaVariable<string>(korigovanjeCasopisaDTO.ISSNbroj),
                    openAccess = new CamundaVariable<bool>(korigovanjeCasopisaDTO.OpenAccess)
                };

                bool success = _camundaService.SubmitTaskForm(taskId, JsonConvert.SerializeObject(content));

                if (success)
                {
                    var casopisi = _dbContext.Casopisi.FirstOrDefault(x => x.Id == id);
                    casopisi.ISSNbroj = korigovanjeCasopisaDTO.ISSNbroj;
                    casopisi.OpenAccess = korigovanjeCasopisaDTO.OpenAccess;
                    
                    _dbContext.SaveChanges();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("dobavi-casopise-za-dodavanje-urednika-i-recenzenata")]
        [HttpGet]
        public IActionResult GetAddingReviewersTask()
        {
            var taskId = _camundaService.GetAssignedTaskId("Task_DodavanjeUrednikRecenzent", "admin1");

            var casopisi = _dbContext.Casopisi.Where(x => x.Urednici == null && x.Recenzenti == null);
            return Ok(casopisi);
        }

        [Route("dodavanje-recenzenata-i-urednika/{id}")]
        [HttpPost]
        public IActionResult AddEditorsAndReviewers(int id, [FromBody]DodavanjeUrednikaRecenzentaDTO urednikRecenzentDTO)
        {
            try
            {
                var taskId = _camundaService.GetUnassignedTaskId("Task_DodavanjeUrednikRecenzent");

                dynamic content = new
                {
                    urednik = new CamundaVariable<string>(string.Join(",", urednikRecenzentDTO.Urednici.Select(x => x.ImeUrednika))),
                    recenzent = new CamundaVariable<string>(string.Join(",", urednikRecenzentDTO.Recenzenti.Select(x => x.ImeRecenzenta)))
                };

                bool success = _camundaService.SubmitTaskForm(taskId, JsonConvert.SerializeObject(content));

                if (success)
                {
                    var casopisi = _dbContext.Casopisi.FirstOrDefault(x => x.Id == id);
                    casopisi.Urednici = string.Join(",", urednikRecenzentDTO.Urednici.Select(x => x.ImeUrednika));
                    casopisi.Recenzenti = string.Join(",", urednikRecenzentDTO.Recenzenti.Select(x => x.ImeRecenzenta));
                    _dbContext.SaveChanges();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("dobavi-casopise-za-potvrdu")]
        [HttpGet]
        public IActionResult GetMagazinesWaitingForApproval()
        {
            var taskId = _camundaService.GetAssignedTaskId("Task_AdminProveraPodataka", "admin1");

            var casopisi = _dbContext.Casopisi.Where(x => x.AdminRecenzirao == false);
            return Ok(casopisi);
        }

        [Route("dobavi-casopis/{id}")]
        [HttpGet]
        public IActionResult GetMagazine(int id)
        {
            var casopisi = _dbContext.Casopisi.FirstOrDefault(x => x.Id == id);
            return Ok(casopisi);
        }

        [Route("dobavi-casopis-za-korigovanje")]
        [HttpGet]
        public IActionResult GetMagazinesForCorrection()
        {
            var taskId = _camundaService.GetAssignedTaskId("Task_DopunaPodataka", "urednik");

            var casopisi = _dbContext.Casopisi.Where(x => x.AdminRecenzirao == true && x.PodaciValidni == false);
            return Ok(casopisi);
        }

        [Route("odobri/{id}")]
        [HttpPost]
        public IActionResult ApproveMagazine(int id)
        {
            var taskId = _camundaService.GetAssignedTaskId("Task_AdminProveraPodataka", "admin1");

            dynamic content = new
            {
                validniPodaci = new CamundaVariable<bool>(true)
            };
            var success = _camundaService.CompleteTask(taskId, JsonConvert.SerializeObject(content));

            if(success)
            {
                var casopisi = _dbContext.Casopisi.FirstOrDefault(x => x.Id == id);
                casopisi.AdminRecenzirao = true;
                casopisi.PodaciValidni = true;
                _dbContext.SaveChanges();
            }

            return Ok();
        }

        [Route("odbij/{id}")]
        [HttpPost]
        public IActionResult DeclineMagazine(int id)
        {
            var taskId = _camundaService.GetAssignedTaskId("Task_AdminProveraPodataka", "admin1");

            dynamic content = new
            {
                validniPodaci = new CamundaVariable<bool>(false)
            };
            var success = _camundaService.CompleteTask(taskId, JsonConvert.SerializeObject(content));

            if (success)
            {
                var casopisi = _dbContext.Casopisi.FirstOrDefault(x => x.Id == id);
                casopisi.AdminRecenzirao = true;
                casopisi.PodaciValidni = false;
                _dbContext.SaveChanges();
            }

            return Ok();
        }
    }
}