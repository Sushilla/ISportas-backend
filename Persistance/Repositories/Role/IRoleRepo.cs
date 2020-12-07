using Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Role
{
    public interface IRoleRepo
    {
        public Task<Guid> Insert(string Pavadinimas);

        public Task Delete(Guid id);

        public Task<IEnumerable<RoleDo>> GetAll();

        public Task Update(Guid id, string pavadinimas);
    }
}
