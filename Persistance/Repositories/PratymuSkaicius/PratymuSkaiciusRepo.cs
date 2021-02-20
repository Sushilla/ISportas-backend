using Models.dto;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.PratymuSkaicius
{
    public class PratymuSkaiciusRepo : IPratymuSkaiciusRepo
    {
        private readonly ISqlClient _sqlClient;

        private readonly string _insertQueryString = "INSERT INTO PratymuSkaicius (TreniruotesId, PratymoId, Priejimai, Skaicius) VALUES ('{0}', '{1}', '{2}', '{3}')";
        private readonly string _deleteQueryString = "DELETE FROM PratymuSkaicius WHERE TreniruotesId='{0}'";
        private readonly string _getAllQueryString = "SELECT p.*, pra.Pavadinimas FROM PratymuSkaicius as p, Pratymai as pra WHERE p.PratymoId = pra.PratimoId and p.TreniruotesId = '{0}'";

        private readonly string _updateQueryString =
            "UPDATE PratymuSkaicius SET Priejimai='{0}', Skaicius='{1}' WHERE TreniruotesId='{2}' AND PratymoId='{3}'";

        public PratymuSkaiciusRepo(ISqlClient sqlclient)
        {
            _sqlClient = sqlclient;
        }

        public async Task<Guid> Insert(string id, string PratimoId, int Priejimas, int Skaicius)
        {
            var insertQuery = string.Format(_insertQueryString, id, PratimoId, Priejimas, Skaicius);

            await _sqlClient.ExecuteNonQuery(insertQuery);

            return new Guid();
        }

        public async Task DeleteAll(Guid id)
        {
            var deleteQuery = string.Format(_deleteQueryString, id.ToString());

            await _sqlClient.ExecuteNonQuery(deleteQuery);
        }

        public async Task<IEnumerable<PratymuSksaiciusDo>> GetAll(Guid id)
        {
            var getAllQuery = string.Format(_getAllQueryString, id.ToString());

            var result = await _sqlClient.ExecuteQueryList<PratymuSkaiciusDto>(getAllQuery, Func);
            var resultTask = result.Select(d => new PratymuSksaiciusDo
            {
                TreniruotesId = new Guid(d.TreniruotesId),
                PratymoId = new Guid(d.PratymoId),
                Priejimai = Int32.Parse(d.Priejimai),
                Skaicius = Int32.Parse(d.Skaicius),
                Pavadinimas = d.Pavadinimas
            });

            return resultTask;
        }

        private async Task<PratymuSkaiciusDto> Func(SqlDataReader reader) //pagalbine fnkc
        {
            var TreniruotesId = await reader.GetFieldValueAsync<string>("TreniruotesId");
            var PratymoId = await reader.GetFieldValueAsync<string>("PratymoId");
            var Priejimai = await reader.GetFieldValueAsync<int>("Priejimai");
            var Skaicius = await reader.GetFieldValueAsync<int>("Skaicius");
            var Pavadinimas = await reader.GetFieldValueAsync<string>("Pavadinimas");

            return new PratymuSkaiciusDto
            {
                TreniruotesId = TreniruotesId,
                PratymoId = PratymoId,
                Priejimai = Priejimai.ToString(),
                Skaicius = Skaicius.ToString(),
                Pavadinimas = Pavadinimas
            };
        }

        public async Task Update(Guid id, Guid pratId, int priejimas, int skaicius)
        {
            var queryString = string.Format(_updateQueryString, priejimas, skaicius, id, pratId);

            await _sqlClient.ExecuteNonQuery(queryString);
        }
    }
}
