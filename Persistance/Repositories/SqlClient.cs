using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class SqlClient : ISqlClient
    {
        private string _conString;
        public SqlClient(string conString)
        {
            _conString = conString;
        }

        public async Task<IEnumerable<T>> ExecuteQueryList<T>(string query, Func<SqlDataReader, Task<T>> func)
        {
            using var sqlConnection = new SqlConnection(_conString);
            var queryCommand = sqlConnection.CreateCommand();
            queryCommand.CommandText = query;

            List<T> queryObject = new List<T>();

            await sqlConnection.OpenAsync();

            using (var reader = await queryCommand.ExecuteReaderAsync())
                if (reader.HasRows)
                    while (reader.Read())
                        queryObject.Add(await func(reader));

            return queryObject;
        }

        public async Task<T> ExecuteQuery<T>(string query, Func<SqlDataReader, Task<T>> func)
        {
            using var sqlConnection = new SqlConnection(_conString);
            var queryCommand = sqlConnection.CreateCommand();
            queryCommand.CommandText = query;

            T queryObject = default;

            await sqlConnection.OpenAsync();

            using (var reader = await queryCommand.ExecuteReaderAsync())
                if (reader.HasRows)
                    while (reader.Read())
                        queryObject = await func(reader);

            return queryObject;
        }

        public async Task ExecuteNonQuery(string query)
        {
            using var sqlConnection = new SqlConnection(_conString);
            var queryCommand = sqlConnection.CreateCommand();
            queryCommand.CommandText = query;

            await sqlConnection.OpenAsync();

            await queryCommand.ExecuteNonQueryAsync();
        }

        public async Task<Boolean> ExecuteQueryCheck(string query)
        {
            using var sqlConnection = new SqlConnection(_conString);
            var queryCommand = sqlConnection.CreateCommand();
            queryCommand.CommandText = query;

            await sqlConnection.OpenAsync();

            using (var reader = await queryCommand.ExecuteReaderAsync())
                return (reader.HasRows);
        }
    }
}
