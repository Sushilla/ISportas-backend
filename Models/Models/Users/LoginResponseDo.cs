using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models.Users
{
    public class LoginResponseDo
    {
        public Guid Id { get; set; }
        public string Vardas { get; set; }
        public string Pavarde { get; set; }
        public string Email { get; set; }
        public string Pavadinimas { get; set; }
    }
}
