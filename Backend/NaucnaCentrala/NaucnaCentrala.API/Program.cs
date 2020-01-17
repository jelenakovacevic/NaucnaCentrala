using System;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NaucnaCentrala.Core.Enums;
using NaucnaCentrala.Data;
using NaucnaCentrala.Domain.Entities;

namespace NaucnaCentrala.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<DataContext>();

                SeedData(services);
            }

            host.Run();
        }

        private static void SeedData(IServiceProvider services)
        {
            using (var context = new DataContext(
                services.GetRequiredService<DbContextOptions<DataContext>>()))
            {
                if (context.Korisnici.Any())
                {
                    return;
                }

                context.Korisnici.Add(
                   new Korisnik
                   {
                       KorisnickoIme = "admin1",
                       Lozinka = "admin1",
                       Ime = "Ivan",
                       Prezime = "Ivic",
                       Grad = "grad",
                       Drzava = "drzava",
                       Email = "administrator@hotmail.com",
                       Aktivan = true,
                       EmailPotvrdjen = true,
                       Uloga = Roles.Administrator.ToString(),
                       AdminPotvrdio = true
                   });

                context.Korisnici.Add(
                    new Korisnik
                    {
                        KorisnickoIme = "urednik",
                        Lozinka = "urednik",
                        Ime = "Jovan",
                        Prezime = "Jovic",
                        Grad = "grad",
                        Drzava = "drzava",
                        Email = "urednik@hotmail.com",
                        NaucneOblasti = "Biofizika, Matematicka ekonomija, Nuklearna fizika",
                        Aktivan = true,
                        EmailPotvrdjen = true,
                        Uloga = Roles.Editor.ToString(),
                        AdminPotvrdio = true
                    });

                context.Korisnici.Add(
                    new Korisnik
                    {
                        KorisnickoIme = "urednik1",
                        Lozinka = "urednik1",
                        Ime = "Nikola",
                        Prezime = "Nikolic",
                        Grad = "grad",
                        Drzava = "drzava",
                        Email = "urednik1@hotmail.com",
                        NaucneOblasti = "Biofizika, Matematicka ekonomija, Nuklearna fizika",
                        Aktivan = true,
                        EmailPotvrdjen = true,
                        Uloga = Roles.Editor.ToString(),
                        AdminPotvrdio = true
                    });

                context.Korisnici.Add(
                    new Korisnik
                    {
                        KorisnickoIme = "urednik2",
                        Lozinka = "urednik2",
                        Ime = "Sasa",
                        Prezime = "Sasic",
                        Grad = "grad",
                        Drzava = "drzava",
                        Email = "urednik2@hotmail.com",
                        NaucneOblasti = "Biofizika, Matematicka ekonomija, Nuklearna fizika",
                        Aktivan = true,
                        EmailPotvrdjen = true,
                        Uloga = Roles.Editor.ToString(),
                        AdminPotvrdio = true
                    });

                context.Korisnici.Add(
                    new Korisnik
                    {
                        KorisnickoIme = "urednik3",
                        Lozinka = "urednik3",
                        Ime = "Milan",
                        Prezime = "Milic",
                        Grad = "grad",
                        Drzava = "drzava",
                        Email = "urednik3@hotmail.com",
                        NaucneOblasti = "Biofizika, Matematicka ekonomija, Nuklearna fizika",
                        Aktivan = true,
                        EmailPotvrdjen = true,
                        Uloga = Roles.Editor.ToString(),
                        AdminPotvrdio = true
                    });

                context.Korisnici.Add(
                   new Korisnik
                   {
                       KorisnickoIme = "urednik4",
                       Lozinka = "urednik4",
                       Ime = "Marko",
                       Prezime = "Markovic",
                       Grad = "grad",
                       Drzava = "drzava",
                       Email = "urednik4@hotmail.com",
                       NaucneOblasti = "Kinematika, Internet marketing",
                       Aktivan = true,
                       EmailPotvrdjen = true,
                       Uloga = Roles.Editor.ToString(),
                       AdminPotvrdio = true
                   });

                context.Korisnici.Add(
                   new Korisnik
                   {
                       KorisnickoIme = "urednik5",
                       Lozinka = "urednik5",
                       Ime = "Milos",
                       Prezime = "Milosevic",
                       Grad = "grad",
                       Drzava = "drzava",
                       Email = "urednik5@hotmail.com",
                       NaucneOblasti = "Kinematika, Internet marketing",
                       Aktivan = true,
                       EmailPotvrdjen = true,
                       Uloga = Roles.Editor.ToString(),
                       AdminPotvrdio = true
                   });

                context.Korisnici.Add(
                   new Korisnik
                   {
                       KorisnickoIme = "urednik6",
                       Lozinka = "urednik6",
                       Ime = "Dusan",
                       Prezime = "Dusic",
                       Grad = "grad",
                       Drzava = "drzava",
                       Email = "urednik6@hotmail.com",
                       NaucneOblasti = "Kinematika, Internet marketing",
                       Aktivan = true,
                       EmailPotvrdjen = true,
                       Uloga = Roles.Editor.ToString(),
                       AdminPotvrdio = true
                   });

                context.Korisnici.Add(
                    new Korisnik
                    {
                        KorisnickoIme = "recenzent1",
                        Lozinka = "recenzent1",
                        Ime = "Vuk",
                        Prezime = "Vukovic",
                        Grad = "grad",
                        Drzava = "drzava",
                        Email = "recenzent1@hotmail.com",
                        NaucneOblasti = "Biofizika, Matematicka ekonomija, Nuklearna fizika",
                        Aktivan = true,
                        EmailPotvrdjen = true,
                        Uloga = Roles.Reviewer.ToString(),
                        AdminPotvrdio = true
                    });

                context.Korisnici.Add(
                    new Korisnik
                    {
                        KorisnickoIme = "recenzent2",
                        Lozinka = "recenzent2",
                        Ime = "Dejan",
                        Prezime = "Dejanovic",
                        Grad = "grad",
                        Drzava = "drzava",
                        Email = "recenzent2@hotmail.com",
                        NaucneOblasti = "Biofizika, Matematicka ekonomija, Nuklearna fizika",
                        Aktivan = true,
                        EmailPotvrdjen = true,
                        Uloga = Roles.Reviewer.ToString(),
                        AdminPotvrdio = true
                    });              

                context.Korisnici.Add(
                    new Korisnik
                    {
                        KorisnickoIme = "recenzent3",
                        Lozinka = "recenzent3",
                        Ime = "Milutin",
                        Prezime = "Msilutinovic",
                        Grad = "grad",
                        Drzava = "drzava",
                        Email = "recenzent3@hotmail.com",
                        NaucneOblasti = "Biofizika, Matematicka ekonomija, Nuklearna fizika",
                        Aktivan = true,
                        EmailPotvrdjen = true,
                        Uloga = Roles.Reviewer.ToString(),
                        AdminPotvrdio = true
                    });

                context.Korisnici.Add(
                    new Korisnik
                    {
                        KorisnickoIme = "recenzent4",
                        Lozinka = "recenzent4",
                        Ime = "Stojan",
                        Prezime = "Stojkovic",
                        Grad = "grad",
                        Drzava = "drzava",
                        Email = "recenzent4@hotmail.com",
                        NaucneOblasti = "Kinematika, Internet marketing",
                        Aktivan = true,
                        EmailPotvrdjen = true,
                        Uloga = Roles.Reviewer.ToString(),
                        AdminPotvrdio = true
                    });

                context.Korisnici.Add(
                    new Korisnik
                    {
                        KorisnickoIme = "recenzent5",
                        Lozinka = "recenzent5",
                        Ime = "Bojan",
                        Prezime = "Bojanovic",
                        Grad = "grad",
                        Drzava = "drzava",
                        Email = "recenzent5@hotmail.com",
                        NaucneOblasti = "Kinematika, Internet marketing",
                        Aktivan = true,
                        EmailPotvrdjen = true,
                        Uloga = Roles.Reviewer.ToString(),
                        AdminPotvrdio = true
                    });

                context.Korisnici.Add(
                    new Korisnik
                    {
                        KorisnickoIme = "recenzent6",
                        Lozinka = "recenzent6",
                        Ime = "Mirko",
                        Prezime = "Mirkovic",
                        Grad = "recenzent6",
                        Drzava = "recenzent6",
                        Email = "recenzent6@hotmail.com",
                        NaucneOblasti = "Kinematika, Internet marketing",
                        Aktivan = true,
                        EmailPotvrdjen = true,
                        Uloga = Roles.Reviewer.ToString(),
                        AdminPotvrdio = true
                    });

                context.SaveChanges();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseDefaultServiceProvider(options => options.ValidateScopes = false)
                .UseStartup<Startup>();
    }
}
