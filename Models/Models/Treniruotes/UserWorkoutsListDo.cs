using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models.Treniruotes
{
    public class UserWorkoutsListDo
    {
        public Guid TreniruotesId { get; set; }
        public string Pavadinimas { get; set; }
        public string Aprasymas { get; set; }
        public DateTime SukurimoData { get; set; }
        public int Progress { get; set; }
    }
}
