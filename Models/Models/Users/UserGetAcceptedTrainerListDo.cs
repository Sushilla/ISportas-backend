using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models.Users
{
    public class UserGetAcceptedTrainerListDo
    {
        public Guid Id { get; set; }
        public string Vardas { get; set; }
        public string Pavarde { get; set; }
        public string Email { get; set; }
    }
}
