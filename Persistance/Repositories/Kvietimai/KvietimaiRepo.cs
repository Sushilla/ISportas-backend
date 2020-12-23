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

namespace Persistance.Repositories.Kvietimai
{
    public class KvietimaiRepo : IKvietimaiRepo
    {
        private readonly ISqlClient _sqlClient;

        private readonly string _insertQueryString = "INSERT INTO Kvietimai (KvietimoId, TrenerioID, VartotojoId, SukurimoData) VALUES ('{0}', '{1}', '{2}', '{3}')";
        private readonly string _deleteQueryString = "DELETE FROM Kvietimai WHERE KvietimoId='{0}'";
        private readonly string _getAllQueryString = "SELECT k.*, v.Vardas, v.Pavarde FROM Kvietimai as k, Vartotojas as v WHERE TrenerioID='{0}' AND v.Id=k.VartotojoId ORDER BY k.SukurimoData desc";
        private readonly string _getNumberOfInvitesString = "SELECT COUNT(*) as yra FROM Kvietimai WHERE TrenerioID='{0}'";
        private readonly string _updateQueryString =
            "UPDATE Kvietimai SET TrenerioID='{0}', VartotojoId='{1}' WHERE KvietimoId='{2}'";

        public KvietimaiRepo(ISqlClient sqlclient)
        {
            _sqlClient = sqlclient;
        }

        public async Task<Guid> Insert(string TrenerioID, string VartotojoId)
        {
            var id = Guid.NewGuid();
            var SukurimoData = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            var insertQuery = string.Format(_insertQueryString, id, TrenerioID, VartotojoId, SukurimoData);

            await _sqlClient.ExecuteNonQuery(insertQuery);

            return id;
        }

        public async Task Delete(Guid id)
        {
            var deleteQuery = string.Format(_deleteQueryString, id.ToString());

            await _sqlClient.ExecuteNonQuery(deleteQuery);
        }

        public async Task<IEnumerable<TrainerRequestsToFriendDo>> GetAll(Guid trenerioId)
        {
            var getAllQuery = string.Format(_getAllQueryString, trenerioId.ToString());

            var result = await _sqlClient.ExecuteQueryList<TrainerRequestsToFriendDto>(getAllQuery, Func);
            var resultTask = result.Select(d => new TrainerRequestsToFriendDo
            {
                KvietimoId = new Guid(d.KvietimoId),
                TrenerioID = new Guid(d.TrenerioID),
                VartotojoId = new Guid(d.VartotojoId),
                SukurimoData = DateTime.Parse(d.SukurimoData),
                Vardas = d.Vardas,
                Pavarde = d.Pavarde
            });

            return resultTask;
        }

        private async Task<TrainerRequestsToFriendDto> Func(SqlDataReader reader) //pagalbine fnkc
        {
            var KvietimoId = await reader.GetFieldValueAsync<string>("KvietimoId");
            var TrenerioID = await reader.GetFieldValueAsync<string>("TrenerioID");
            var VartotojoId = await reader.GetFieldValueAsync<string>("VartotojoId");
            var SukurimoData = await reader.GetFieldValueAsync<DateTime>("SukurimoData");
            var Vardas = await reader.GetFieldValueAsync<string>("Vardas");
            var Pavarde = await reader.GetFieldValueAsync<string>("Pavarde");

            return new TrainerRequestsToFriendDto
            {
                    KvietimoId = KvietimoId,
                    TrenerioID = TrenerioID,
                    VartotojoId = VartotojoId,
                    SukurimoData = SukurimoData.ToString(),
                    Vardas = Vardas,
                    Pavarde = Pavarde
            };
        }


        public async Task<IEnumerable<TrainerRequestCountDo>> GetNumberOfRequests(Guid trenerioId)
        {
            var getAllQuery = string.Format(_getNumberOfInvitesString, trenerioId.ToString());

            var result = await _sqlClient.ExecuteQueryList<TrainerRequestCountDto>(getAllQuery, getSkaicius);
            var resultTask = result.Select(d => new TrainerRequestCountDo
            {
                yra = Int32.Parse(d.yra)
            });

            return resultTask;
        }

        private async Task<TrainerRequestCountDto> getSkaicius(SqlDataReader reader) //pagalbine fnkc
        {
            var yra = await reader.GetFieldValueAsync<int>("yra");

            return new TrainerRequestCountDto
            {
                yra = yra +""
            };
        }

        public async Task Update(Guid KvietimoId, Guid TrenerioID, Guid VartotojoId)
        {
            var queryString = string.Format(_updateQueryString, TrenerioID, VartotojoId, KvietimoId);

            await _sqlClient.ExecuteNonQuery(queryString);
        }
    }
}
