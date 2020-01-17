using System.Collections.Generic;

namespace NaucnaCentrala.API.DTOs
{
    public class KreiranjeCasopisaDTO
    {
        public string Naziv { get; set; }
        public string ISSNbroj { get; set; }
        public bool OpenAccess { get; set; }
        public List<ScientificArea> NaucneOblasti { get; set; }
    }
}
