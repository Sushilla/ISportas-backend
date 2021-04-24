using Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Pratymai
{
    public interface IPratymaiRepo
    {
        public Task<Guid> Insert(IEnumerable<string> Pavadinimas);
        public Task Delete(Guid id);
        public Task<IEnumerable<PratymaiDo>> GetAll();
        public Task Update(Guid id, string pavadinimas);
    }
}
