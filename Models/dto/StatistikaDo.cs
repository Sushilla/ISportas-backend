using System;
using System.Collections.Generic;
using System.Text;

namespace Models.dto
{
    public class StatistikaDo
    {
        public Guid StatistikosId { get; set; }
        public DateTime TreniruotesPradzia { get; set; }
        public DateTime TreniruotesPabaiga { get; set; }
        public Guid VartotojoId { get; set; }
    }
}
