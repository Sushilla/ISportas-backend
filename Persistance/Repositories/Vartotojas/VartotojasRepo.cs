using Models.dto;
using Models.Models;
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

        private readonly string _insertQueryString = "INSERT INTO Vartotojas (Id, RolesId, Vardas, Pavarde, Email, Password) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')";
        private readonly string _deleteQueryString = "DELETE FROM Vartotojas WHERE Id='{0}'";
        private readonly string _getAllQueryString = "SELECT * FROM Vartotojas";

        private readonly string _updateQueryString =
            "UPDATE Vartotojas SET RolesId='{0}', Vardas='{1}', Pavarde='{2}', Email='{3}', Password='{4}' WHERE Id='{5}'";
        public VartotojasRepo(ISqlClient sqlclient)
        {
            _sqlClient = sqlclient;
        }
        public async Task<Guid> Insert(Guid RolesId, string Vardas, string Pavarde, string Email, string Password)
        {
            var id = Guid.NewGuid();
            var insertQuery = string.Format(_insertQueryString, id, RolesId, Vardas, Pavarde, Email, Password);

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

        public async Task Update(Guid id, Guid rolesId, string vardas, string pavarde, string email, string password)
        {
            var queryString = string.Format(_updateQueryString, rolesId, vardas, pavarde, email, password, id);

            await _sqlClient.ExecuteNonQuery(queryString);
        }
    }
}
