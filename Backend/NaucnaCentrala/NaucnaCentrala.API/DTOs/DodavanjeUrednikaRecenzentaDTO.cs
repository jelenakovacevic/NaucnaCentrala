using System.Collections.Generic;

namespace NaucnaCentrala.API.DTOs
{
    public class DodavanjeUrednikaRecenzentaDTO
    {
        public List<Reviewer> Recenzenti { get; set; }
        public List<Editor> Urednici { get; set; }
    }

    public class Reviewer
    {
        public string ImeRecenzenta { get; set; }
    }

    public class Editor
    {
        public string ImeUrednika { get; set; }
    }
}
