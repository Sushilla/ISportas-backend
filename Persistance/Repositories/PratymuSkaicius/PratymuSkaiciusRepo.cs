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
        private readonly string _getAllQueryString = "SELECT * FROM PratymuSkaicius";

        private readonly string _updateQueryString =
            "UPDATE PratymuSkaicius SET Priejimai='{0}', Skaicius='{1}' WHERE TreniruotesId='{2}' AND PratymoId='{3}'";

        public PratymuSkaiciusRepo(ISqlClient sqlclient)
        {
            _sqlClient = sqlclient;
        }

        public async Task<Guid> Insert(string PratimoId, int Priejimas, int Skaicius)
        {
            var id = Guid.NewGuid();
            var insertQuery = string.Format(_insertQueryString, id, PratimoId, Priejimas, Skaicius);

            await _sqlClient.ExecuteNonQuery(insertQuery);

            return id;
        }

        public async Task Delete(Guid id)
        {
            var deleteQuery = string.Format(_deleteQueryString, id.ToString());

            await _sqlClient.ExecuteNonQuery(deleteQuery);
        }

        public async Task<IEnumerable<PratymuSksaiciusDo>> GetAll()
        {
            var getAllQuery = string.Format(_getAllQueryString);

            var result = await _sqlClient.ExecuteQueryList<PratymuSkaiciusDto>(getAllQuery, Func);
            var resultTask = result.Select(d => new PratymuSksaiciusDo
            {
                TreniruotesId = new Guid(d.TreniruotesId),
                PratymoId = new Guid(d.PratymoId),
                Priejimai = Int32.Parse(d.Priejimai),
                Skaicius = Int32.Parse(d.Skaicius)
            });

            return resultTask;
        }

        private async Task<PratymuSkaiciusDto> Func(SqlDataReader reader) //pagalbine fnkc
        {
            var TreniruotesId = await reader.GetFieldValueAsync<string>("TreniruotesId");
            var PratymoId = await reader.GetFieldValueAsync<string>("PratymoId");
            var Priejimai = await reader.GetFieldValueAsync<int>("Priejimai");
            var Skaicius = await reader.GetFieldValueAsync<int>("Skaicius");

            return new PratymuSkaiciusDto
            {
                TreniruotesId = TreniruotesId,
                PratymoId = PratymoId,
                Priejimai = Priejimai.ToString(),
                Skaicius = Skaicius.ToString()
            };
        }

        public async Task Update(Guid id, Guid pratId, int priejimas, int skaicius)
        {
            var queryString = string.Format(_updateQueryString, priejimas, skaicius, id, pratId);

            await _sqlClient.ExecuteNonQuery(queryString);
        }
    }
}
