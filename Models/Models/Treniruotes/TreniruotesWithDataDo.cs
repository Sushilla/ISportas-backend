using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models.Treniruotes
{
    public class TreniruotesWithDataDo
    {
        public Guid TreniruotesId { get; set; }
        public string Pavadinimas { get; set; }
        public string Aprasymas { get; set; }
        public IEnumerable<PratymuSksaiciusDo> TreniruotesPratymai{get; set;}
    }
}
