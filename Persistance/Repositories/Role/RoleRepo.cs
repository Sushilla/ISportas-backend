using Models.dto;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Role
{
    public class RoleRepo : IRoleRepo
    {
        private readonly ISqlClient _sqlClient;

        private readonly string _insertQueryString = "INSERT INTO Role (RolesId, Pavadinimas) VALUES ('{0}', '{1}')";
        private readonly string _deleteQueryString = "DELETE FROM Role WHERE RolesId='{0}'";
        private readonly string _getAllQueryString = "SELECT * FROM Role";

        private readonly string _updateQueryString =
            "UPDATE Role SET Pavadinimas='{0}' WHERE RolesId='{1}'";

        public RoleRepo(ISqlClient sqlclient)
        {
            _sqlClient = sqlclient;
        }

        public async Task<Guid> Insert(string Pavadinimas)
        {
            var id = Guid.NewGuid();
            var insertQuery = string.Format(_insertQueryString, id, Pavadinimas);

            await _sqlClient.ExecuteNonQuery(insertQuery);

            return id;
        }

        public async Task Delete(Guid id)
        {
            var deleteQuery = string.Format(_deleteQueryString, id.ToString());

            await _sqlClient.ExecuteNonQuery(deleteQuery);
        }

        public async Task<IEnumerable<RoleDo>> GetAll()
        {
            var getAllQuery = string.Format(_getAllQueryString);

            var result = await _sqlClient.ExecuteQueryList<RoleDto>(getAllQuery, Func);
            var resultTask = result.Select(d => new RoleDo
            {
                RolesId = new Guid(d.RolesId),
                Pavadinimas = d.Pavadinimas
            });

            return resultTask;
        }

        private async Task<RoleDto> Func(SqlDataReader reader) //pagalbine fnkc
        {
            var RolesId = await reader.GetFieldValueAsync<string>("RolesId");
            var Pavadinimas = await reader.GetFieldValueAsync<string>("Pavadinimas");

            return new RoleDto
            {
                RolesId = RolesId,
                Pavadinimas = Pavadinimas
            };
        }

        public async Task Update(Guid id, string pavadinimas)
        {
            var queryString = string.Format(_updateQueryString, pavadinimas, id);

            await _sqlClient.ExecuteNonQuery(queryString);
        }

    }
}
