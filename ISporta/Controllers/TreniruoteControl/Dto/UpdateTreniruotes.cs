using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISporta.Controllers.TreniruoteControl.Dto
{
    public class UpdateTreniruotes
    {
        public string TreniruotesId { get; set; }
        public string Pavadinimas { get; set; }
        public string Aprasymas { get; set; }
        public IEnumerable<string> UsersIds { get; set; }
        public IEnumerable<treniPrat> TreniruotesPratymai { get; set; }
    }

    public class treniPrat
    {
        public string treniruotesId { get; set; }
        public string pratymoId { get; set; }
        public int priejimai { get; set; }
        public int skaicius { get; set; }
        public string pavadinimas { get; set; }

    }
}
