using Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Vartotojas
{
    public interface IVartotojasRepo
    {
        public Task<Guid> Insert(Guid RolesId, string Vardas, string Pavarde, string Email, string Password);
        public Task Delete(Guid id);
        public Task<IEnumerable<VartotojasDo>> GetAll();
        public Task Update(Guid id, Guid rolesId, string vardas, string pavarde, string email, string password);

    }
}
