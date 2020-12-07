using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISporta.Controllers.VartotojasControler.Dto
{
    public class InsertVartotojasRequest
    {
        public string RolesId { get; set; }
        public string Vardas { get; set; }
        public string Pavarde { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
