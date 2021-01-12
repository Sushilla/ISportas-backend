using Models.dto;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.PrasymaiPakeistRole
{
    public class PrasymaiPakeistRoleRepo : IPrasymaiPakeistRoleRepo
    {
        private readonly ISqlClient _sqlClient;

        private readonly string _insertQueryString = "INSERT INTO PrasymaiPakeistRole (PakvietimoId, Id, SukurimoData) VALUES ('{0}', '{1}', '{2}')";
        private readonly string _deleteQueryString = "DELETE FROM PrasymaiPakeistRole WHERE PakvietimoId='{0}'";
        private readonly string _getAllQueryString = "SELECT p.PakvietimoId, p.SukurimoData, v.Vardas, v.Pavarde FROM PrasymaiPakeistRole as p, Vartotojas as v WHERE p.Id=v.Id";
        private readonly string _acceptAndChangeRoleToAdminQueryString = "UPDATE v SET v.RolesId=r.RolesId FROM Vartotojas as v, PrasymaiPakeistRole as p INNER JOIN Role as r ON r.Pavadinimas='Trainer' WHERE p.PakvietimoId='{0}' AND v.Id=p.Id DELETE FROM PrasymaiPakeistRole WHERE PrasymaiPakeistRole.PakvietimoId='{0}'";
        private readonly string _updateQueryString =
            "UPDATE PrasymaiPakeistRole SET Id='{0}', TrenerioID='{1}', Statusas='{2}' WHERE PakvietimoId='{3}'";
        public PrasymaiPakeistRoleRepo(ISqlClient sqlclient)
        {
            _sqlClient = sqlclient;
        }

        public async Task<Guid> Insert(Guid Id)
        {
            var id = Guid.NewGuid();
            var SukurimoData = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            var insertQuery = string.Format(_insertQueryString, id, Id.ToString(), SukurimoData);

            await _sqlClient.ExecuteNonQuery(insertQuery);

            return id;
        }


        public async Task<Guid> AcceptUserToBecomeTrainer(Guid Id)
        {
           // var id = Guid.NewGuid();
            var insertQuery = string.Format(_acceptAndChangeRoleToAdminQueryString, Id.ToString());

            await _sqlClient.ExecuteNonQuery(insertQuery);

            return Id;
        }

        public async Task Delete(Guid id)
        {
            var deleteQuery = string.Format(_deleteQueryString, id.ToString());

            await _sqlClient.ExecuteNonQuery(deleteQuery);
        }

        public async Task<IEnumerable<PrasymaiPakeistRoleDo>> GetAll()
        {
            var getAllQuery = string.Format(_getAllQueryString);

            var result = await _sqlClient.ExecuteQueryList<PrasymaiPakeistRoleDto>(getAllQuery, Func);
            var resultTask = result.Select(d => new PrasymaiPakeistRoleDo
            {
                PakvietimoId = new Guid(d.PakvietimoId),
                SukurimoData = DateTime.Parse(d.SukurimoData),
                Vardas = d.Vardas,
                Pavarde = d.Pavarde
            });

            return resultTask;
        }

        private async Task<PrasymaiPakeistRoleDto> Func(SqlDataReader reader) //pagalbine fnkc
        {
            var PakvietimoID = await reader.GetFieldValueAsync<string>("PakvietimoID");
            var SukurimoData = await reader.GetFieldValueAsync<DateTime>("SukurimoData");
            var Vardas = await reader.GetFieldValueAsync<string>("Vardas");
            var Pavarde = await reader.GetFieldValueAsync<string>("Pavarde");


            return new PrasymaiPakeistRoleDto
            {
                PakvietimoId = PakvietimoID,
                SukurimoData = SukurimoData.ToString(),
                Vardas = Vardas,
                Pavarde = Pavarde
            };
        }

        /*ublic async Task Update(Guid PakvietimoId, Guid Id, string stat, Guid TrenerioID)
        {
            var queryString = string.Format(_updateQueryString, Id, TrenerioID, stat, PakvietimoId);

            await _sqlClient.ExecuteNonQuery(queryString);
        }*/ 

        //update nenaudojam kolkas
    }
}
