using Models.dto;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Kvietimai
{
    public class KvietimaiRepo : IKvietimaiRepo
    {
        private readonly ISqlClient _sqlClient;

        private readonly string _insertQueryString = "INSERT INTO Kvietimai (KvietimoId, TrenerioID, VartotojoId, SukurimoData) VALUES ('{0}', '{1}', '{2}', '{3}')";
        private readonly string _deleteQueryString = "DELETE FROM Kvietimai WHERE KvietimoId='{0}'";
        private readonly string _getAllQueryString = "SELECT * FROM Kvietimai";

        private readonly string _updateQueryString =
            "UPDATE Kvietimai SET TrenerioID='{0}', VartotojoId='{1}' WHERE KvietimoId='{2}'";

        public KvietimaiRepo(ISqlClient sqlclient)
        {
            _sqlClient = sqlclient;
        }

        public async Task<Guid> Insert(string TrenerioID, string VartotojoId)
        {
            var id = Guid.NewGuid();
            var SukurimoData = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            var insertQuery = string.Format(_insertQueryString, id, TrenerioID, VartotojoId, SukurimoData);

            await _sqlClient.ExecuteNonQuery(insertQuery);

            return id;
        }

        public async Task Delete(Guid id)
        {
            var deleteQuery = string.Format(_deleteQueryString, id.ToString());

            await _sqlClient.ExecuteNonQuery(deleteQuery);
        }

        public async Task<IEnumerable<KvietimaiDo>> GetAll()
        {
            var getAllQuery = string.Format(_getAllQueryString);

            var result = await _sqlClient.ExecuteQueryList<KvietimaiDto>(getAllQuery, Func);
            var resultTask = result.Select(d => new KvietimaiDo
            {
                KvietimoId = new Guid(d.KvietimoId),
                TrenerioID = new Guid(d.TrenerioID),
                VartotojoId = new Guid(d.VartotojoId),
                SukurimoData = DateTime.Parse(d.SukurimoData)
            });

            return resultTask;
        }

        private async Task<KvietimaiDto> Func(SqlDataReader reader) //pagalbine fnkc
        {
            var KvietimoId = await reader.GetFieldValueAsync<string>("KvietimoId");
            var TrenerioID = await reader.GetFieldValueAsync<string>("TrenerioID");
            var VartotojoId = await reader.GetFieldValueAsync<string>("VartotojoId");
            var SukurimoData = await reader.GetFieldValueAsync<DateTime>("SukurimoData");

            return new KvietimaiDto
            {
                KvietimoId = KvietimoId,
                TrenerioID = TrenerioID,
                VartotojoId = VartotojoId,
                SukurimoData = SukurimoData.ToString()
            };
        }

        public async Task Update(Guid KvietimoId, Guid TrenerioID, Guid VartotojoId)
        {
            var queryString = string.Format(_updateQueryString, TrenerioID, VartotojoId, KvietimoId);

            await _sqlClient.ExecuteNonQuery(queryString);
        }
    }
}
