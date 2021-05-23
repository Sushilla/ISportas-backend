using Models.dto;
using Models.dto.Trainers;
using Models.dto.Users;
using Models.Models;
using Models.Models.Trainers;
using Models.Models.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Vartotojas
{
    public class VartotojasRepo : IVartotojasRepo
    {
        private readonly ISqlClient _sqlClient;

        private readonly string _insertQueryString = "INSERT INTO Vartotojas (Id, RolesId, Vardas, Pavarde, Email, Password) VALUES ('{0}', '445e5161-bef7-432e-9329-30c4ffd09541', '{1}', '{2}', '{3}', '{4}')";
        private readonly string _deleteQueryString = "DELETE FROM Vartotojas WHERE Id='{0}'";
        private readonly string _getAllQueryString = "SELECT * FROM Vartotojas";
        private readonly string _getLoginInfoIfUserExist = "IF EXISTS (SELECT * FROM Vartotojas as v WHERE v.Email='{0}' AND v.Password='{1}') BEGIN SELECT v.Id, v.Vardas, v.Pavarde, v.Email, r.Pavadinimas FROM Vartotojas as v, Role as r WHERE v.Email='{0}' AND v.Password='{1}' AND v.RolesId=r.RolesId END ELSE BEGIN SELECT 0 as empty END";
        private readonly string _getRegisteredUserInfo = "SELECT v.Id, v.Vardas, v.Pavarde, v.Email, r.Pavadinimas FROM Vartotojas as v, Role as r WHERE v.Id='{0}' AND v.RolesId=r.RolesId";
        private readonly string _getTrainerQueryString = "SELECT Id, Email FROM Vartotojas WHERE RolesId='c2103b14-4be9-43e3-b11c-a8da83e83a78'";
        private readonly string _regUSer = "IF NOT EXISTS (SELECT * FROM Vartotojas WHERE Email = '{0}') BEGIN INSERT INTO Vartotojas (Id, RolesId, Vardas, Pavarde, Email, Password) VALUES ('{1}', '445e5161-bef7-432e-9329-30c4ffd09541', '{2}', '{3}', '{4}', '{5}') END";
        private readonly string _regUSeris = "IF NOT EXISTS (SELECT * FROM Vartotojas WHERE Email = @email) BEGIN INSERT INTO Vartotojas (Id, RolesId, Vardas, Pavarde, Email, Password) VALUES (@id, '445e5161-bef7-432e-9329-30c4ffd09541', @vardas, @pavarde, @email, @password) END";
        private readonly string _getUserData = "SELECT v.Vardas, v.Pavarde, v.Email FROM Vartotojas as v WHERE v.Id='{0}'";

        private readonly string _updateQueryString =
            "UPDATE Vartotojas SET RolesId='{0}', Vardas='{1}', Pavarde='{2}', Email='{3}', Password='{4}' WHERE Id='{5}'";
        private readonly string _changeUserPassword = "IF EXISTS (SELECT * FROM Vartotojas as v WHERE v.Id='{0}' AND v.Password='{1}') BEGIN UPDATE Vartotojas SET Password='{2}' WHERE Id='{0}' AND Password='{1}' END ELSE BEGIN SELECT 0 as empty END";
        public VartotojasRepo(ISqlClient sqlclient)
        {
            _sqlClient = sqlclient;
        }

        SHA256 sha256Hash = SHA256.Create();

        public async Task<IEnumerable<LoginResponseDo>> UserRegister(string Vardas, string Pavarde, string Email, string Password)
        {
            SqlCommand sqlCom = new SqlCommand();
            sqlCom.CommandText = _regUSeris;

            var id = Guid.NewGuid();

            sqlCom.Parameters.AddWithValue("@email", Email);
            sqlCom.Parameters.AddWithValue("@id", id);
            sqlCom.Parameters.AddWithValue("@vardas", Vardas);
            sqlCom.Parameters.AddWithValue("@pavarde", Pavarde);
            sqlCom.Parameters.AddWithValue("@password", GetHash(sha256Hash, Password));

            await _sqlClient.newFunc(sqlCom);

            /*var insertQuery = string.Format(_regUSer, Email, id, Vardas, Pavarde, Email, GetHash(sha256Hash, Password));

             await _sqlClient.ExecuteNonQuery(insertQuery);*/
            var getAllQuery = string.Format(_getRegisteredUserInfo, id.ToString());

            var result = await _sqlClient.ExecuteQueryList<LoginResponseDto>(getAllQuery, FuncToGetDataForLogin);
           var resultTask = result.Select(d => new LoginResponseDo
            {
                Id = new Guid(d.Id),
                Vardas = d.Vardas,
                Pavarde = d.Pavarde,
                Email = d.Email,
                Pavadinimas = d.Pavadinimas

            });
            return resultTask;
        }
        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public async Task Delete(Guid id)
        {
            var deleteQuery = string.Format(_deleteQueryString, id.ToString());

            await _sqlClient.ExecuteNonQuery(deleteQuery);
        }

        public async Task<IEnumerable<VartotojasDo>> GetAll()
        {
            var getAllQuery = string.Format(_getAllQueryString);

            var result = await _sqlClient.ExecuteQueryList<VartotojasDto>(getAllQuery, Func);
            var resultTask = result.Select(d => new VartotojasDo
            {
                Id = new Guid(d.Id),
                RolesId = new Guid(d.RolesId),
                Vardas = d.Vardas,
                Pavarde = d.Pavarde,
                Email = d.Email,
                Password = d.Password

            });

            return resultTask;
        }

        private async Task<VartotojasDto> Func(SqlDataReader reader) //pagalbine fnkc
        {
            var Id = await reader.GetFieldValueAsync<string>("Id");
            var RolesId = await reader.GetFieldValueAsync<string>("RolesId");
            var Vardas = await reader.GetFieldValueAsync<string>("Vardas");
            var Pavarde = await reader.GetFieldValueAsync<string>("Pavarde");
            var Email = await reader.GetFieldValueAsync<string>("Email");
            var Password = await reader.GetFieldValueAsync<string>("Password");

            return new VartotojasDto
            {
                Id = Id,
                RolesId = RolesId,
                Vardas = Vardas,
                Pavarde = Pavarde,
                Email = Email,
                Password = Password
            };
        }

        public async Task<IEnumerable<TrainerListDo>> GetTrainers()
        {
            var getAllQuery = string.Format(_getTrainerQueryString);

            var result = await _sqlClient.ExecuteQueryList<TrainerListDto>(getAllQuery, FuncToGetTRainersList);
            var resultTask = result.Select(d => new TrainerListDo
            {
                Id = new Guid(d.Id),
                Email = d.Email
            });

            return resultTask;
        }

        private async Task<TrainerListDto> FuncToGetTRainersList(SqlDataReader reader) //pagalbine fnkc
        {
            var Id = await reader.GetFieldValueAsync<string>("Id");
            var Email = await reader.GetFieldValueAsync<string>("Email");

            return new TrainerListDto
            {
                Id = Id,
                Email = Email
            };
        }

        public async Task Update(Guid id, Guid rolesId, string vardas, string pavarde, string email, string password)
        {
            var queryString = string.Format(_updateQueryString, rolesId, vardas, pavarde, email, password, id);

            await _sqlClient.ExecuteNonQuery(queryString);
        }

        public async Task<Boolean> UpdateUserPassword(Guid id, string oldPass, string newPass)
        {
            var queryString = string.Format(_changeUserPassword, id, GetHash(sha256Hash, oldPass), GetHash(sha256Hash, newPass));

            return await _sqlClient.ExecuteQueryCheck(queryString);

        }

        public async Task<IEnumerable<LoginResponseDo>> GetLoginUserInfo(string email, string pass)
        {
            var getAllQuery = string.Format(_getLoginInfoIfUserExist, email, GetHash(sha256Hash, pass));

            var result = await _sqlClient.ExecuteQueryList<LoginResponseDto>(getAllQuery, FuncToGetDataForLogin);
            var resultTask = result.Select(d => new LoginResponseDo
            {
                Id = new Guid(d.Id),
                Vardas = d.Vardas,
                Pavarde = d.Pavarde,
                Email = d.Email,
                Pavadinimas = d.Pavadinimas

            });

            return resultTask;
        }

 
        private async Task<LoginResponseDto> FuncToGetDataForLogin(SqlDataReader reader) //pagalbine fnkc
        {
            var Id = await reader.GetFieldValueAsync<string>("Id");
            var Vardas = await reader.GetFieldValueAsync<string>("Vardas");
            var Pavarde = await reader.GetFieldValueAsync<string>("Pavarde");
            var Email = await reader.GetFieldValueAsync<string>("Email");
            var Pavadinimas = await reader.GetFieldValueAsync<string>("Pavadinimas");

            return new LoginResponseDto
            {
                Id = Id,
                Vardas = Vardas,
                Pavarde = Pavarde,
                Email = Email,
                Pavadinimas = Pavadinimas
            };
        }

        public async Task<IEnumerable<UserDataDo>> GetUserData(Guid userId)
        {
            var getAllQuery = string.Format(_getUserData, userId);

            var result = await _sqlClient.ExecuteQueryList<UserDataDto>(getAllQuery, FuncForData);
            var resultTask = result.Select(d => new UserDataDo
            {
                Vardas = d.Vardas,
                Pavarde = d.Pavarde,
                Email = d.Email
            });

            return resultTask;
        }

        private async Task<UserDataDto> FuncForData(SqlDataReader reader) //pagalbine fnkc
        {
            var Vardas = await reader.GetFieldValueAsync<string>("Vardas");
            var Pavarde = await reader.GetFieldValueAsync<string>("Pavarde");
            var Email = await reader.GetFieldValueAsync<string>("Email");

            return new UserDataDto
            {
                Vardas = Vardas,
                Pavarde = Pavarde,
                Email = Email
            };
        }
    }
}
