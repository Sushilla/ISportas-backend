using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models
{
    public class PrasymaiPakeistRoleDo
    {
        public Guid PakvietimoId { get; set; }
        public DateTime SukurimoData { get; set; }
        public string Vardas { get; set; }
        public string Pavarde { get; set; }
    }
}
