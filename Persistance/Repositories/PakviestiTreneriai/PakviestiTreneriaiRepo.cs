using Models.dto;
using Models.dto.Users;
using Models.Models;
using Models.Models.Users;
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
        private readonly string _getAllQueryString = "SELECT v.Id, v.Vardas, v.Pavarde, v.Email FROM PakviestiTreneriai as p, Vartotojas as v WHERE p.Id='{0}' AND p.TrenerioID=v.Id";
        private readonly string _getAllAccpetedUdersQueryString = "SELECT v.Id, v.Vardas, v.Pavarde, v.Email FROM PakviestiTreneriai as p, Vartotojas as v WHERE p.TrenerioID='{0}' AND p.Id=v.Id";
        private readonly string _acceptStringCOPY = "INSERT INTO PakviestiTreneriai(PakvietimoId, Id, TrenerioID, Statusas) SELECT '{0}', k.VartotojoId, k.TrenerioId, 'accept' FROM Kvietimai as k WHERE k.KvietimoId= '{1}' DELETE FROM Kvietimai WHERE KvietimoId= '{1}'";

        private readonly string _updateQueryString =
            "UPDATE PakviestiTreneriai SET Id='{0}', TrenerioID='{1}', Statusas='{2}' WHERE PakvietimoId='{3}'";
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

        public async Task<Guid> InsertAcceptedRequest(Guid kvietId)
        {
            var id = Guid.NewGuid();
            var copyDel = string.Format(_acceptStringCOPY, id, kvietId.ToString());

            await _sqlClient.ExecuteNonQuery(copyDel);

            return id;
        }

        public async Task Delete(Guid id)
        {
            var deleteQuery = string.Format(_deleteQueryString, id.ToString());

            await _sqlClient.ExecuteNonQuery(deleteQuery);
        }

        public async Task<IEnumerable<UserGetAcceptedTrainerListDo>> GetAll(Guid id)
        {
            var getAllQuery = string.Format(_getAllQueryString, id);

            var result = await _sqlClient.ExecuteQueryList<UserGetAcceptedTrainerListDto>(getAllQuery, Func);
            var resultTask = result.Select(d => new UserGetAcceptedTrainerListDo
            {
                Id = new Guid(d.Id),
                Vardas = d.Vardas,
                Pavarde = d.Pavarde,
                Email = d.Email
            });

            return resultTask;
        }


        public async Task<IEnumerable<UserGetAcceptedTrainerListDo>> GetAllUserForTrainer(Guid id)
        {
            var getAllQuery = string.Format(_getAllAccpetedUdersQueryString, id);

            var result = await _sqlClient.ExecuteQueryList<UserGetAcceptedTrainerListDto>(getAllQuery, Func);
            var resultTask = result.Select(d => new UserGetAcceptedTrainerListDo
            {
                Id = new Guid(d.Id),
                Vardas = d.Vardas,
                Pavarde = d.Pavarde,
                Email = d.Email
            });

            return resultTask;
        }

        private async Task<UserGetAcceptedTrainerListDto> Func(SqlDataReader reader) //pagalbine fnkc
        {
            var Id = await reader.GetFieldValueAsync<string>("Id");
            var Vardas = await reader.GetFieldValueAsync<string>("Vardas");
            var Pavarde = await reader.GetFieldValueAsync<string>("Pavarde");
            var Email = await reader.GetFieldValueAsync<string>("Email");

            return new UserGetAcceptedTrainerListDto
            {
                Id = Id,
                Vardas = Vardas,
                Pavarde = Pavarde,
                Email = Email
            };
        }

        public async Task Update(Guid PakvietimoId, Guid Id, string Statusas, Guid TrenerioID)
        {
            var queryString = string.Format(_updateQueryString, Id, TrenerioID, Statusas, PakvietimoId);

            await _sqlClient.ExecuteNonQuery(queryString);
        }
    }
}
