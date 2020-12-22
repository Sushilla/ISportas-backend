using Models.dto;
using Models.dto.Trainers;
using Models.Models;
using Models.Models.Trainers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Vartotojas
{
    public class VartotojasRepo : IVartotojasRepo
    {
        private readonly ISqlClient _sqlClient;

        private readonly string _insertQueryString = "INSERT INTO Vartotojas (Id, RolesId, Vardas, Pavarde, Email, Password) VALUES ('{0}', '445e5161-bef7-432e-9329-30c4ffd09541', '{1}', '{2}', '{3}', '{4}')";
        private readonly string _deleteQueryString = "DELETE FROM Vartotojas WHERE Id='{0}'";
        private readonly string _getAllQueryString = "SELECT * FROM Vartotojas";
        private readonly string _getTrainerQueryString = "SELECT Id, Email FROM Vartotojas WHERE RolesId='c2103b14-4be9-43e3-b11c-a8da83e83a78'";
        private readonly string asdasa = "IF NOT EXISTS (SELECT * FROM Vartotojas WHERE Email = '{0}') BEGIN INSERT INTO Vartotojas (Id, RolesId, Vardas, Pavarde, Email, Password) VALUES ('{1}', '445e5161-bef7-432e-9329-30c4ffd09541', '{2}', '{3}', '{4}', '{5}') END";

        private readonly string _updateQueryString =
            "UPDATE Vartotojas SET RolesId='{0}', Vardas='{1}', Pavarde='{2}', Email='{3}', Password='{4}' WHERE Id='{5}'";
        public VartotojasRepo(ISqlClient sqlclient)
        {
            _sqlClient = sqlclient;
        }
        public async Task<Guid> UserRegister(string Vardas, string Pavarde, string Email, string Password)
        {
            var id = Guid.NewGuid();
            var insertQuery = string.Format(asdasa, Email, id, Vardas, Pavarde, Email, Password);

            await _sqlClient.ExecuteNonQuery(insertQuery);

            return id;
        }

        public async Task Delete(Guid id)
        {
            var deleteQuery = string.Format(_deleteQueryString, id.ToString());

            await _sqlClient.ExecuteNonQuery(deleteQuery);
        }

        public async Task<IEnumerable<VartotojasDo>> GetAll()
        {
            var getAllQuery = string.Format(_getAllQueryString);

            var result = await _sqlClient.ExecuteQueryList<VartotojasDto>(getAllQuery, Func);
            var resultTask = result.Select(d => new VartotojasDo
            {
                Id = new Guid(d.Id),
                RolesId = new Guid(d.RolesId),
                Vardas = d.Vardas,
                Pavarde = d.Pavarde,
                Email = d.Email,
                Password = d.Password

            });

            return resultTask;
        }

        private async Task<VartotojasDto> Func(SqlDataReader reader) //pagalbine fnkc
        {
            var Id = await reader.GetFieldValueAsync<string>("Id");
            var RolesId = await reader.GetFieldValueAsync<string>("RolesId");
            var Vardas = await reader.GetFieldValueAsync<string>("Vardas");
            var Pavarde = await reader.GetFieldValueAsync<string>("Pavarde");
            var Email = await reader.GetFieldValueAsync<string>("Email");
            var Password = await reader.GetFieldValueAsync<string>("Password");

            return new VartotojasDto
            {
                Id = Id,
                RolesId = RolesId,
                Vardas = Vardas,
                Pavarde = Pavarde,
                Email = Email,
                Password = Password
            };
        }

        public async Task<IEnumerable<TrainerListDo>> GetTrainers()
        {
            var getAllQuery = string.Format(_getTrainerQueryString);

            var result = await _sqlClient.ExecuteQueryList<TrainerListDto>(getAllQuery, FuncToGetTRainersList);
            var resultTask = result.Select(d => new TrainerListDo
            {
                Id = new Guid(d.Id),
                Email = d.Email
            });

            return resultTask;
        }

        private async Task<TrainerListDto> FuncToGetTRainersList(SqlDataReader reader) //pagalbine fnkc
        {
            var Id = await reader.GetFieldValueAsync<string>("Id");
            var Email = await reader.GetFieldValueAsync<string>("Email");

            return new TrainerListDto
            {
                Id = Id,
                Email = Email
            };
        }

        public async Task Update(Guid id, Guid rolesId, string vardas, string pavarde, string email, string password)
        {
            var queryString = string.Format(_updateQueryString, rolesId, vardas, pavarde, email, password, id);

            await _sqlClient.ExecuteNonQuery(queryString);
        }
    }
}
