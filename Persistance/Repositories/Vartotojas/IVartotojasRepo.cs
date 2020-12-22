﻿using Models.Models;
using Models.Models.Trainers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Vartotojas
{
    public interface IVartotojasRepo
    {
        public Task<Guid> UserRegister(string Vardas, string Pavarde, string Email, string Password);
        public Task Delete(Guid id);
        public Task<IEnumerable<VartotojasDo>> GetAll();
        public Task Update(Guid id, Guid rolesId, string vardas, string pavarde, string email, string password);
        public Task<IEnumerable<TrainerListDo>> GetTrainers();

    }
}
