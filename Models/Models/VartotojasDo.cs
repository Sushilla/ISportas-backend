using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models
{
    public class VartotojasDo
    {
        public Guid Id { get; set; }
        public Guid RolesId { get; set; }
        public string Vardas { get; set; }
        public string Pavarde { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
