using Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Treniruote
{
    public interface ITreniruoteRepo
    {
        public Task<Guid> Insert(string TrenerioID, string VartotojoId, string Pavadinimas, string Aprasymas);
        public Task Delete(Guid id);
        public Task<IEnumerable<TreniruoteDo>> GetAll();
        public Task Update(Guid TreniruotesId, Guid TrenerioID, Guid VartotojoId, string Pavadinimas, string Aprasymas);
    }
}
