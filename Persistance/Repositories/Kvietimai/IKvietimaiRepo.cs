using Models.Models;
using Models.Models.Trainers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Kvietimai
{
    public interface IKvietimaiRepo
    {
        public Task<Guid> Insert(string TrenerioID, string VartotojoId);
        public Task Delete(Guid id);
        public Task<IEnumerable<TrainerRequestsToFriendDo>> GetAll(Guid trenerioId);
        public Task Update(Guid KvietimoId, Guid TrenerioID, Guid VartotojoId);
    }
}
