using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models
{
    public class KvietimaiDo
    {
        public Guid KvietimoId { get; set; }
        public Guid TrenerioID { get; set; }
        public Guid VartotojoId { get; set; }
        public DateTime SukurimoData { get; set; }
    }
}
