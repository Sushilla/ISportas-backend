using Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.PrasymaiPakeistRole
{
    public interface IPrasymaiPakeistRoleRepo
    {
        public  Task<Guid> Insert(Guid Id);
        public Task<Guid> AcceptUserToBecomeTrainer(Guid Id);
        public  Task Delete(Guid id);
        public Task<IEnumerable<PrasymaiPakeistRoleDo>> GetAll();

    }
}
