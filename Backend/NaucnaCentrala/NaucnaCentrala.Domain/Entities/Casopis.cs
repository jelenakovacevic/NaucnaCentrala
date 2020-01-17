using System.ComponentModel.DataAnnotations.Schema;

namespace NaucnaCentrala.Domain.Entities
{
    [Table("Casopis")]
    public class Casopis
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string ISSNbroj { get; set; }
        public bool OpenAccess { get; set; }
        public string NaucneOblasti { get; set; }
        public string Urednici { get; set; }
        public string Recenzenti { get; set; }
        public bool AdminRecenzirao { get; set; }
        public bool PodaciValidni { get; set; }
        public string GlavniUrednik { get; set; }
        public bool Aktivan { get; set; }
    }
}
