using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models.Trainers
{
    public class TrainerRequestsToFriendDo
    {
        public Guid KvietimoId { get; set; }
        public Guid TrenerioID { get; set; }
        public Guid VartotojoId { get; set; }
        public DateTime SukurimoData { get; set; }
        public string Vardas { get; set; }
        public string Pavarde { get; set; }
    }
}
