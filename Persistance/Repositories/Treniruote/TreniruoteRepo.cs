using Models.dto;
using Models.Models;
using Persistance.Repositories.Vartotojai;
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
        private readonly IVartotojaiRepo _ivertotojai;

        private readonly string _insertQueryString = "INSERT INTO Treniruote (TreniruotesId, TrenerioId, VartotojoId, Pavadinimas, Aprasymas, SukurimoData) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')";
        private readonly string _deleteQueryString = "DELETE FROM Treniruote WHERE TreniruotesId='{0}'";
        private readonly string _getAllQueryString = "SELECT t.TreniruotesId, t.TrenerioId, t.Pavadinimas, t.Aprasymas, t.SukurimoData FROM Treniruote as t WHERE TrenerioId='{0}'";

        private readonly string _updateQueryString =
            "UPDATE Treniruote SET TrenerioID='{0}', VartotojoId='{1}', Pavadinimas='{2}', Aprasymas='{3}' WHERE TreniruotesId='{4}'";

        public TreniruoteRepo(ISqlClient sqlclient, IVartotojaiRepo ivertotojai)
        {
            _sqlClient = sqlclient;
            _ivertotojai = ivertotojai;
        }

        public async Task<Guid> Insert(string TrenerioID, string VartotojoId, string Pavadinimas, string Aprasymas, IEnumerable<string> vartId)
        {
            var id = Guid.NewGuid();
            var SukurimoData = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            var insertQuery = string.Format(_insertQueryString, id, TrenerioID, VartotojoId, Pavadinimas, Aprasymas, SukurimoData);

            await _sqlClient.ExecuteNonQuery(insertQuery);

            foreach(var vart in vartId)
            {
                await _ivertotojai.Insert(id.ToString(), vart);
            }

            return id;
        }

        public async Task Delete(Guid id)
        {
            var deleteQuery = string.Format(_deleteQueryString, id.ToString());

            await _sqlClient.ExecuteNonQuery(deleteQuery);
        }

        public async Task<IEnumerable<TreniruoteDo>> GetAll(Guid id)
        {
            var getAllQuery = string.Format(_getAllQueryString, id.ToString());

            var result = await _sqlClient.ExecuteQueryList<TreniruoteDto>(getAllQuery, Func);
            var resultTask = result.Select(d => new TreniruoteDo
            {
                TreniruotesId = new Guid(d.TreniruotesId),
                TrenerioId = new Guid(d.TrenerioId),
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
            var Pavadinimas = await reader.GetFieldValueAsync<string>("Pavadinimas");
            var Aprasymas = await reader.GetFieldValueAsync<string>("Aprasymas");
            var SukurimoData = await reader.GetFieldValueAsync<DateTime>("SukurimoData");

            return new TreniruoteDto
            {
                TreniruotesId = TreniruotesId,
                TrenerioId = TrenerioId,
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
