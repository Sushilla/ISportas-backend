using Models.dto;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.PakviestiTreneriai
{
    public class PakviestiTreneriaiRepo : IPakviestiTreneriaiRepo
    {
        private readonly ISqlClient _sqlClient;

        private readonly string _insertQueryString = "INSERT INTO PakviestiTreneriai (PakvietimoId, Id, TrenerioID) VALUES ('{0}', '{1}', '{2}')";
        private readonly string _deleteQueryString = "DELETE FROM PakviestiTreneriai WHERE PakvietimoId='{0}'";
        private readonly string _getAllQueryString = "SELECT * FROM PakviestiTreneriai";

        private readonly string _updateQueryString =
            "UPDATE PakviestiTreneriai SET Id='{0}', TrenerioID='{1}' WHERE PakvietimoId='{2}'";
        public PakviestiTreneriaiRepo(ISqlClient sqlclient)
        {
            _sqlClient = sqlclient;
        }

        public async Task<Guid> Insert(string Id, string TrenerioID)
        {
            var id = Guid.NewGuid();
            var insertQuery = string.Format(_insertQueryString, id, Id, TrenerioID);

            await _sqlClient.ExecuteNonQuery(insertQuery);

            return id;
        }

        public async Task Delete(Guid id)
        {
            var deleteQuery = string.Format(_deleteQueryString, id.ToString());

            await _sqlClient.ExecuteNonQuery(deleteQuery);
        }

        public async Task<IEnumerable<PakviestiTreneriaiDo>> GetAll()
        {
            var getAllQuery = string.Format(_getAllQueryString);

            var result = await _sqlClient.ExecuteQueryList<PakviestiTreneriaiDto>(getAllQuery, Func);
            var resultTask = result.Select(d => new PakviestiTreneriaiDo
            {
                PakvietimoID = new Guid(d.PakvietimoID),
                Id = new Guid(d.Id),
                TrenerioID = new Guid(d.TrenerioID)
            });

            return resultTask;
        }

        private async Task<PakviestiTreneriaiDto> Func(SqlDataReader reader) //pagalbine fnkc
        {
            var PakvietimoID = await reader.GetFieldValueAsync<string>("PakvietimoID");
            var Id = await reader.GetFieldValueAsync<string>("Id");
            var TrenerioID = await reader.GetFieldValueAsync<string>("TrenerioID");

            return new PakviestiTreneriaiDto
            {
                PakvietimoID = PakvietimoID,
                Id = Id,
                TrenerioID = TrenerioID
            };
        }

        public async Task Update(Guid PakvietimoId, Guid Id, Guid TrenerioID)
        {
            var queryString = string.Format(_updateQueryString, Id, TrenerioID, PakvietimoId);

            await _sqlClient.ExecuteNonQuery(queryString);
        }
    }
}
