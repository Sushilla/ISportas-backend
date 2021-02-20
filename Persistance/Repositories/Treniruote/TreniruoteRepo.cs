using Models.Classes;
using Models.dto;
using Models.Models;
using Models.Models.Treniruotes;
using Persistance.Repositories.PratymuSkaicius;
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
        private readonly IPratymuSkaiciusRepo _ipratymuSkaicius;

        private readonly string _insertQueryString = "INSERT INTO Treniruote (TreniruotesId, TrenerioId, VartotojoId, Pavadinimas, Aprasymas, SukurimoData) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')";
        private readonly string _deleteQueryString = "DELETE FROM Treniruote WHERE TreniruotesId='{0}'";
        private readonly string _getAllQueryString = "SELECT t.TreniruotesId, t.TrenerioId, t.Pavadinimas, t.Aprasymas, t.SukurimoData FROM Treniruote as t WHERE TrenerioId='{0}'";
        private readonly string _getTreniruotesEditData = "SELECT t.TreniruotesId, t.TrenerioId, t.Pavadinimas, t.Aprasymas, t.SukurimoData FROM Treniruote as t WHERE TreniruotesId='{0}'";

        private readonly string _updateQueryString =
            "UPDATE Treniruote SET TrenerioID='{0}', VartotojoId='{1}', Pavadinimas='{2}', Aprasymas='{3}' WHERE TreniruotesId='{4}'";

        public TreniruoteRepo(ISqlClient sqlclient, IVartotojaiRepo ivertotojai, IPratymuSkaiciusRepo ipratymuSkaicius)
        {
            _sqlClient = sqlclient;
            _ivertotojai = ivertotojai;
            _ipratymuSkaicius = ipratymuSkaicius;
        }

        public async Task<Guid> Insert(string TrenerioID, string VartotojoId, string Pavadinimas, string Aprasymas, IEnumerable<string> vartId, IEnumerable<TreniruotesPratymai> prat)
        {
            var id = Guid.NewGuid();
            var SukurimoData = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            var insertQuery = string.Format(_insertQueryString, id, TrenerioID, VartotojoId, Pavadinimas, Aprasymas, SukurimoData);

            await _sqlClient.ExecuteNonQuery(insertQuery);

            foreach(var vart in vartId)
            {
                await _ivertotojai.Insert(id.ToString(), vart);
            }

            foreach(var pratymas in prat)
            {
                await _ipratymuSkaicius.Insert(id.ToString(), pratymas.id.ToString(), pratymas.priej, pratymas.skaic);
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

        public async Task<IEnumerable<TreniruotesWithDataDo>> GetEditData(Guid id)
        {
            var getQueryOfTreniruote = string.Format(_getTreniruotesEditData, id.ToString());

            var resultTreniruote = await _sqlClient.ExecuteQueryList<TreniruoteDto>(getQueryOfTreniruote, Func);
            var trenPratymai = await _ipratymuSkaicius.GetAll(id);
            var trenIDs = new List<Guid>();
            foreach (var prat in trenPratymai)
            {
                trenIDs.Add(prat.PratymoId);
            }
            var vartototojai = await _ivertotojai.GetAll(id);
            var resultTask = resultTreniruote.Select(d => new TreniruotesWithDataDo
            {
                TreniruotesId = new Guid(d.TreniruotesId),
                Pavadinimas = d.Pavadinimas,
                Aprasymas = d.Aprasymas,
                TreniruotesPratymai = trenPratymai,
                PratIds = trenIDs,
                UsersIds =  vartototojai
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