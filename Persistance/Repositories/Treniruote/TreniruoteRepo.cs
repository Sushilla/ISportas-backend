using Models.dto;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Treniruote
{
    public class TreniruoteRepo : ITreniruoteRepo
    {
        private readonly ISqlClient _sqlClient;

        private readonly string _insertQueryString = "INSERT INTO Treniruote (TreniruotesId, TrenerioId, VartotojoId, Pavadinimas, Aprasymas, SukurimoData) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')";
        private readonly string _deleteQueryString = "DELETE FROM Treniruote WHERE TreniruotesId='{0}'";
        private readonly string _getAllQueryString = "SELECT * FROM Treniruote";

        private readonly string _updateQueryString =
            "UPDATE Treniruote SET TrenerioID='{0}', VartotojoId='{1}', Pavadinimas='{2}', Aprasymas='{3}' WHERE TreniruotesId='{4}'";

        public TreniruoteRepo(ISqlClient sqlclient)
        {
            _sqlClient = sqlclient;
        }

        public async Task<Guid> Insert(string TrenerioID, string VartotojoId, string Pavadinimas, string Aprasymas)
        {
            var id = Guid.NewGuid();
            var SukurimoData = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            var insertQuery = string.Format(_insertQueryString, id, TrenerioID, VartotojoId, Pavadinimas, Aprasymas, SukurimoData);

            await _sqlClient.ExecuteNonQuery(insertQuery);

            return id;
        }

        public async Task Delete(Guid id)
        {
            var deleteQuery = string.Format(_deleteQueryString, id.ToString());

            await _sqlClient.ExecuteNonQuery(deleteQuery);
        }

        public async Task<IEnumerable<TreniruoteDo>> GetAll()
        {
            var getAllQuery = string.Format(_getAllQueryString);

            var result = await _sqlClient.ExecuteQueryList<TreniruoteDto>(getAllQuery, Func);
            var resultTask = result.Select(d => new TreniruoteDo
            {
                TreniruotesId = new Guid(d.TreniruotesId),
                TrenerioId = new Guid(d.TrenerioId),
                VartotojoId = new Guid(d.VartotojoId),
                Pavadinimas = d.Pavadinimas,
                Aprasymas = d.Aprasymas,
                SukurimoData = DateTime.Parse(d.SukurimoData)
            });

            return resultTask;
        }

        private async Task<TreniruoteDto> Func(SqlDataReader reader) //pagalbine fnkc
        {
            var TreniruotesId = await reader.GetFieldValueAsync<string>("TreniruotesId");
            var TrenerioId = await reader.GetFieldValueAsync<string>("TrenerioId");
            var VartotojoId = await reader.GetFieldValueAsync<string>("VartotojoId");
            var Pavadinimas = await reader.GetFieldValueAsync<string>("Pavadinimas");
            var Aprasymas = await reader.GetFieldValueAsync<string>("Aprasymas");
            var SukurimoData = await reader.GetFieldValueAsync<DateTime>("SukurimoData");

            return new TreniruoteDto
            {
                TreniruotesId = TreniruotesId,
                TrenerioId = TrenerioId,
                VartotojoId = VartotojoId,
                Pavadinimas = Pavadinimas,
                Aprasymas = Aprasymas,
                SukurimoData = SukurimoData.ToString()
            };
        }

        public async Task Update(Guid TreniruotesId, Guid TrenerioID, Guid VartotojoId, string Pavadinimas, string Aprasymas)
        {
            var queryString = string.Format(_updateQueryString, TrenerioID, VartotojoId, Pavadinimas, Aprasymas, TreniruotesId);

            await _sqlClient.ExecuteNonQuery(queryString);
        }
    }
}
