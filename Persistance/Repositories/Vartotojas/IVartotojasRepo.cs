using Models.Models;
using Models.Models.Trainers;
using Models.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Vartotojas
{
    public interface IVartotojasRepo
    {
        public Task<IEnumerable<LoginResponseDo>> UserRegister(string Vardas, string Pavarde, string Email, string Password);
        public Task Delete(Guid id);
        public Task<IEnumerable<VartotojasDo>> GetAll();
        public Task Update(Guid id, Guid rolesId, string vardas, string pavarde, string email, string password);
        public Task<IEnumerable<TrainerListDo>> GetTrainers();
        public Task<IEnumerable<LoginResponseDo>> GetLoginUserInfo(string email, string pass);


    }
}
