using NaucnaCentrala.Core.Models;
using NaucnaCentrala.Data;
using NaucnaCentrala.Domain.Models;
using NaucnaCentrala.Interfaces.Services;
using NaucnaCentrala.Interfaces.Workers;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NaucnaCentrala.Workers
{
    public class CamundaWorker : ICamundaWorker
    {
        private readonly ICamundaService _camundaService;
        private readonly IEmailService _emailService;
        private readonly DataContext _dbContext;
        private readonly string _workerId = "worker";

        public CamundaWorker(ICamundaService camundaService, IEmailService emailService, DataContext dbContext)
        {
            _camundaService = camundaService;
            _emailService = emailService;
            _dbContext = dbContext;
        }

        public void Run()
        {
            StartRegistrationDataCheck();
            StartEmailSendingCheck();
            StartActivateUserCheck();
            StartActivationReviewerCheck();
            StartActivateChiefEditor();
            StartAddEditorsAndReviewersCheck();
            StartMagazineActivationCheck();
            StartMagazineCorrectionDataCheck();
        }

        public void StartMagazineCorrectionDataCheck()
        {
            var task = new Task(() =>
            {
                while (true)
                {
                    MagazineCorrectionDataCheck();
                }
            });

            task.Start();
        }

        public void StartMagazineActivationCheck()
        {
            var task = new Task(() =>
            {
                while (true)
                {
                    MagazineActivationCheck();
                }
            });

            task.Start();
        }

        public void StartAddEditorsAndReviewersCheck()
        {
            var task = new Task(() =>
            {
                while (true)
                {
                    AddEditorsAndReviewersCheck();
                }
            });

            task.Start();
        }

        public void StartActivateChiefEditor()
        {
            var task = new Task(() =>
            {
                while (true)
                {
                    ActivateChiefEditor();
                }
            });

            task.Start();
        }

        public void StartActivationReviewerCheck()
        {
            var task = new Task(() =>
            {
                while (true)
                {
                    ActivationReviewerCheck();
                }
            });

            task.Start();
        }

        public void StartActivateUserCheck()
        {
            var task = new Task(() =>
            {
                while (true)
                {
                    ActivateUserCheck();
                }
            });

            task.Start();
        }

        public void StartEmailSendingCheck()
        {
            var task = new Task(() =>
            {
                while (true)
                {
                    EmailSendingCheck();
                }
            });

            task.Start();
        }

        public void StartRegistrationDataCheck()
        {
            var task = new Task(() =>
            {
                while (true)
                {
                    RegistrationDataCheck();
                }
            });

            task.Start();
        }

        private void ActivateChiefEditor()
        {
            try
            {
                FetchAndLockResponse response = _camundaService.FetchAndLockExternalTask(_workerId, "ObradaUnetihPodatakaiPotrvdaGlUrednika", new[] { "issnBroj" });

                if (!response.Success)
                {
                    Thread.Sleep(500);
                    return;
                }

                var ISSNbroj = (string)response.ResponseData.variables["issnBroj"].value;
                var magazine = _dbContext.Casopisi.FirstOrDefault(x => x.ISSNbroj == ISSNbroj);
                _dbContext.SaveChanges();


                dynamic content = new { };
                _camundaService.CompleteExternalTask(response.ResponseData.id, _workerId, JsonConvert.SerializeObject(content));
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void ActivateUserCheck()
        {
            try
            {
                FetchAndLockResponse response = _camundaService.FetchAndLockExternalTask(_workerId, "AktivacijaKorisnika", new[] { "korisnickoIme" });

                if (!response.Success)
                {
                    Thread.Sleep(500);
                    return;
                }

                var korisnickoIme = (string)response.ResponseData.variables["korisnickoIme"].value;
                var korisnik = _dbContext.Korisnici.FirstOrDefault(x => x.KorisnickoIme == korisnickoIme);
                korisnik.Aktivan = true;
                _dbContext.SaveChanges();


                dynamic content = new { };
                _camundaService.CompleteExternalTask(response.ResponseData.id, _workerId, JsonConvert.SerializeObject(content));
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void EmailSendingCheck()
        {
            try
            {
                FetchAndLockResponse response = _camundaService.FetchAndLockExternalTask(_workerId, "SlanjeEmail", new[] { "korisnickoIme" });

                if (!response.Success)
                {
                    Thread.Sleep(500);
                    return;
                }

                var korisnickoIme = (string)response.ResponseData.variables["korisnickoIme"].value;
                var userToken = _dbContext.Korisnici.FirstOrDefault(x => x.KorisnickoIme == korisnickoIme).Token;

                _emailService.SendEmail("Verifikacija email-a", "Molim Vas, verifikujte email na sledecem <a href=\"https://localhost:44372/api/korisnik/potvrdi-email?token=" + userToken + "\">linku</a>");

                dynamic content = new { };
                _camundaService.CompleteExternalTask(response.ResponseData.id, _workerId, JsonConvert.SerializeObject(content));
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void RegistrationDataCheck()
        {
            try
            {
                FetchAndLockResponse response = _camundaService.FetchAndLockExternalTask(_workerId, "ProveraPodataka", new[] { "korisnickoIme", "lozinka" });

                if (!response.Success)
                {
                    Thread.Sleep(500);
                    return;
                }

                //TO DO: check username and is data valid
                dynamic content = new { validniPodaci = new CamundaVariable<bool>(true) };
                _camundaService.CompleteExternalTask(response.ResponseData.id, _workerId, JsonConvert.SerializeObject(content));
            }
            catch
            {
                return;
            }
        }

        private void ActivationReviewerCheck()
        {
            try
            {
                FetchAndLockResponse response = _camundaService.FetchAndLockExternalTask(_workerId, "ObradaRecenzenta", new[] { "korisnickoIme" });

                if (!response.Success)
                {
                    Thread.Sleep(500);
                    return;
                }

                var korisnickoIme = (string)response.ResponseData.variables["korisnickoIme"].value;
                var korisnik = _dbContext.Korisnici.FirstOrDefault(x => x.KorisnickoIme == korisnickoIme);
                korisnik.AdminPotvrdio = true;
                _dbContext.SaveChanges();


                dynamic content = new { };
                _camundaService.CompleteExternalTask(response.ResponseData.id, _workerId, JsonConvert.SerializeObject(content));
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void AddEditorsAndReviewersCheck()
        {
            try
            {
                FetchAndLockResponse response = _camundaService.FetchAndLockExternalTask(_workerId, "ObradaUnetihPodataka", new[] { "korisnickoIme" });

                if (!response.Success)
                {
                    Thread.Sleep(500);
                    return;
                }
                dynamic content = new { };
                _camundaService.CompleteExternalTask(response.ResponseData.id, _workerId, JsonConvert.SerializeObject(content));
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void MagazineActivationCheck()
        {
            try
            {
                FetchAndLockResponse response = _camundaService.FetchAndLockExternalTask(_workerId, "AktivacijaCasopisauSistemu", new[] { "naziv" });

                if (!response.Success)
                {
                    Thread.Sleep(500);
                    return;
                }

                var magazineName = (string)response.ResponseData.variables["naziv"].value;
                var casopis = _dbContext.Casopisi.FirstOrDefault(x => x.Naziv == magazineName);
                casopis.Aktivan = true;
                _dbContext.SaveChanges();


                dynamic content = new { };
                _camundaService.CompleteExternalTask(response.ResponseData.id, _workerId, JsonConvert.SerializeObject(content));
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void MagazineCorrectionDataCheck()
        {
            try
            {
                FetchAndLockResponse response = _camundaService.FetchAndLockExternalTask(_workerId, "ObradaPodataka", new[] { "naziv" });

                if (!response.Success)
                {
                    Thread.Sleep(500);
                    return;
                }

                var magazineName = (string)response.ResponseData.variables["naziv"].value;

                dynamic content = new { };
                _camundaService.CompleteExternalTask(response.ResponseData.id, _workerId, JsonConvert.SerializeObject(content));
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
