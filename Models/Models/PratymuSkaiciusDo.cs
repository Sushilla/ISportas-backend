using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models
{
    public class PratymuSksaiciusDo
    {
        public Guid TreniruotesId { get; set; }
        public Guid PratymoId { get; set; }
        public int Priejimai { get; set; }
        public int Skaicius { get; set; }
    }
}
