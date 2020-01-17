using System.ComponentModel.DataAnnotations.Schema;

namespace NaucnaCentrala.Domain.Entities
{
    [Table("Korisnik")]
    public class Korisnik
    {
        public int Id { get; set; }
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Grad { get; set; }
        public string Drzava { get; set; }
        public string Titula { get; set; }
        public bool Recenzent { get; set; }
        public string NaucneOblasti { get; set; }
        public string Token { get; set; }
        public bool Aktivan { get; set; }
        public bool EmailPotvrdjen { get; set; }
        public string Uloga { get; set; }
        public string Email { get; set; }
        public bool AdminPotvrdio { get; set; }
    }
}
