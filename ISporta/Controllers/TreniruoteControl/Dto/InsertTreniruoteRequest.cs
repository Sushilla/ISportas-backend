using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISporta.Controllers.TreniruoteControl.Dto
{
    public class InsertTreniruoteRequest
    {
        public string TrenerioId { get; set; }
        public string VartotojoId { get; set; }
        public string Pavadinimas { get; set; }
        public string Aprasymas { get; set; }
    }
}
