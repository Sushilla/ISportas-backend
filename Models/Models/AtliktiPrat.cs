using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Classes
{
    public class AtliktiPrat
    {
        public Guid TreniruotesId { get; set; }
        public Guid StatistikosId { get; set; }
        public Guid AtpazyntoPratymoId { get; set; }
        public int Priejimas { get; set; }
        public int Skaicius { get; set; }
    }
}
