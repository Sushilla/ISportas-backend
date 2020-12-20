using Models.dto;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Pratymai
{
    public class PratymaiRepo : IPratymaiRepo
    {
        private readonly ISqlClient _sqlClient;

        private readonly string _insertQueryString = "INSERT INTO Pratymai (PratimoId, Pavadinimas) VALUES ('{0}', '{1}')";
        private readonly string _deleteQueryString = "DELETE FROM Pratymai WHERE PratimoId='{0}'";
        private readonly string _getAllQueryString = "SELECT * FROM Pratymai";

        private readonly string _updateQueryString =
            "UPDATE Pratymai SET Pavadinimas='{0}' WHERE PratimoId='{1}'";

        public PratymaiRepo(ISqlClient sqlclient)
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

        public async Task<IEnumerable<PratymaiDo>> GetAll()
        {
            var getAllQuery = string.Format(_getAllQueryString);

            var result = await _sqlClient.ExecuteQueryList<PratymaiDto>(getAllQuery, Func);
            var resultTask = result.Select(d => new PratymaiDo
            {
                PratimoId = new Guid(d.PratimoId),
                Pavadinimas = d.Pavadinimas
            });

            return resultTask;
        }

        private async Task<PratymaiDto> Func(SqlDataReader reader) //pagalbine fnkc
        {
            var PratimoId = await reader.GetFieldValueAsync<string>("PratimoId");
            var Pavadinimas = await reader.GetFieldValueAsync<string>("Pavadinimas");

            return new PratymaiDto
            {
                PratimoId = PratimoId,
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
