using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISporta.Controllers.PratymuSkaiciusControler.Dto
{
    public class InsertPratymuSkaiciusRequest
    {
        public string PratimoId { get; set; }
        public int Priejimas { get; set; }
        public int Skaicius { get; set; }
    }
}
