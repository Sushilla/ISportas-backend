using Models.Models;
using Models.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.PakviestiTreneriai
{
    public interface IPakviestiTreneriaiRepo
    {
        public Task<Guid> Insert(string Id, string TrenerioID);
        public Task<Guid> InsertAcceptedRequest(Guid kvietId);
        public Task Delete(Guid id);
        public Task<IEnumerable<UserGetAcceptedTrainerListDo>> GetAll(Guid id);
        public Task Update(Guid PakvietimoId, Guid Id, string stat, Guid TrenerioID);
    }
}
