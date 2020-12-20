using Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.PratymuSkaicius
{
    public interface IPratymuSkaiciusRepo
    {
        public Task<Guid> Insert(string PratimoId, int Priejimas, int Skaicius);
        public Task Delete(Guid id);
        public Task<IEnumerable<PratymuSksaiciusDo>> GetAll();
        public Task Update(Guid id, Guid pratId, int priejimas, int skaicius);

    }
}
