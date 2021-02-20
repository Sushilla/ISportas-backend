using Models.Classes;
using Models.Models;
using Models.Models.Treniruotes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Treniruote
{
    public interface ITreniruoteRepo
    {
        public Task<Guid> Insert(string TrenerioID, string VartotojoId, string Pavadinimas, string Aprasymas, IEnumerable<string> vartId, IEnumerable<TreniruotesPratymai> prat);
        public Task Delete(Guid id);
        public Task<IEnumerable<TreniruoteDo>> GetAll(Guid id);
        public Task<IEnumerable<TreniruotesWithDataDo>> GetEditData(Guid id);
        public Task Update(Guid TreniruotesId, Guid TrenerioID, Guid VartotojoId, string Pavadinimas, string Aprasymas);
    }
}
