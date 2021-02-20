using Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Vartotojai
{
    public interface IVartotojaiRepo
    {
        public Task<Guid> Insert(string id, string VartotojoId);
        public Task Delete(Guid id, Guid vartId);
        public Task DeleteAll(Guid id);
        public Task<IEnumerable<Guid>> GetAll(Guid id);
    }
}
