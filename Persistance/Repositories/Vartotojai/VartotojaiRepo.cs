﻿using Models.dto;
using Models.dto.Workout;
using Models.Models;
using Models.Models.Treniruotes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Vartotojai
{
    public class VartotojaiRepo : IVartotojaiRepo
    {
        private readonly ISqlClient _sqlClient;

        private readonly string _insertQueryString = "INSERT INTO Vartotojai (TreniruotesId, VartotojoId) VALUES ('{0}', '{1}')";
        private readonly string _deleteQueryString = "DELETE FROM Vartotojai WHERE TreniruotesId='{0}' AND VartotojoId='{1}'";
        private readonly string _deleteAll = "DELETE FROM Vartotojai WHERE TreniruotesId='{0}'";
        private readonly string _getAllQueryString = "SELECT * FROM Vartotojai WHERE TreniruotesId = '{0}'";
        private readonly string _getSelectedWorkoutUsers = "SELECT v.Id, v.Email, v.Vardas, v.Pavarde FROM Vartotojai as u, Vartotojas as v WHERE u.TreniruotesId = '{0}' and u.VartotojoId=v.Id";

        private readonly string _updateQueryString =
            "UPDATE Vartotojai SET TrenerioID='{0}', VartotojoId='{1}', Pavadinimas='{2}', Aprasymas='{3}' WHERE TreniruotesId='{4}'";

        public VartotojaiRepo(ISqlClient sqlclient)
        {
            _sqlClient = sqlclient;
        }

        public async Task<Guid> Insert(string id, string VartotojoId)
        {
            var insertQuery = string.Format(_insertQueryString, id, VartotojoId);

            await _sqlClient.ExecuteNonQuery(insertQuery);

            return new Guid();
        }

        public async Task Delete(Guid id, Guid vartId)
        {
            var deleteQuery = string.Format(_deleteQueryString, id.ToString(), vartId.ToString());

            await _sqlClient.ExecuteNonQuery(deleteQuery);
        }

        public async Task DeleteAll(Guid id)
        {
            var deleteQuery = string.Format(_deleteAll, id.ToString());

            await _sqlClient.ExecuteNonQuery(deleteQuery);
        }

        public async Task<IEnumerable<Guid>> GetAll(Guid id)
        {
            var getAllQuery = string.Format(_getAllQueryString, id.ToString());

            var result = await _sqlClient.ExecuteQueryList<VartotojaiDto>(getAllQuery, Func);
            var resultTask = result.Select(d => new Guid(d.VartotojoId));

            return resultTask;
        }


        private async Task<VartotojaiDto> Func(SqlDataReader reader) //pagalbine fnkc
        {
            var TreniruotesId = await reader.GetFieldValueAsync<string>("TreniruotesId");
            var VartotojoId = await reader.GetFieldValueAsync<string>("VartotojoId");

            return new VartotojaiDto
            {
                TreniruotesId = TreniruotesId,
                VartotojoId = VartotojoId
            };
        }

        public async Task<IEnumerable<WorkoutUsersDo>> GetWorkoutUsers(Guid id)
        {
            var getAllQuery = string.Format(_getSelectedWorkoutUsers, id.ToString());

            var result = await _sqlClient.ExecuteQueryList<WorkoutUsersDto>(getAllQuery, Funkc);
            var resultTask = result.Select(d => new WorkoutUsersDo
            {
                Id = new Guid(d.Id),
                Email = d.Email,
                Vardas = d.Vardas,
                Pavarde = d.Pavarde
            });

            return resultTask;
        }

        private async Task<WorkoutUsersDto> Funkc(SqlDataReader reader) //pagalbine fnkc
        {
            var Id = await reader.GetFieldValueAsync<string>("Id");
            var Email = await reader.GetFieldValueAsync<string>("Email");
            var Vardas = await reader.GetFieldValueAsync<string>("Vardas");
            var Pavarde = await reader.GetFieldValueAsync<string>("Pavarde");

            return new WorkoutUsersDto
            {
                Id = Id,
                Email = Email,
                Vardas = Vardas,
                Pavarde = Pavarde
            };
        }

        /*public async Task Update(Guid TreniruotesId, Guid TrenerioID, Guid VartotojoId, string Pavadinimas, string Aprasymas)
        {
            var queryString = string.Format(_updateQueryString, TrenerioID, VartotojoId, Pavadinimas, Aprasymas, TreniruotesId);

            await _sqlClient.ExecuteNonQuery(queryString);
        }*/
        //kolkas nereikia irgi
    }
}
