using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models
{
    public class TreniruoteDo
    {
        public Guid TreniruotesId { get; set; }
        public Guid TrenerioId { get; set; }
        public Guid VartotojoId { get; set; }
        public string Pavadinimas { get; set; }
        public string Aprasymas { get; set; }
        public DateTime SukurimoData { get; set; }
    }
}
