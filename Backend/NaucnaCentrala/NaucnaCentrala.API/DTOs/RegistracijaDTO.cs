using System.Collections.Generic;

namespace NaucnaCentrala.API.DTOs
{
    public class RegistracijaDTO
    {
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Grad { get; set; }
        public string Drzava{ get; set; }
        public string Titula { get; set; }
        public bool Recenzent { get; set; }
        public List<ScientificArea> NaucneOblasti { get; set; }
    }

    public class ScientificArea
    {
        public string Naziv { get; set; }
    }
}
