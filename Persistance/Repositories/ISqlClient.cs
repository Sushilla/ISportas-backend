using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public interface ISqlClient
    {
        Task<IEnumerable<T>> ExecuteQueryList<T>(string query, Func<SqlDataReader, Task<T>> func);
        Task<T> ExecuteQuery<T>(string query, Func<SqlDataReader, Task<T>> func);
        Task ExecuteNonQuery(string query);
        Task<Boolean> ExecuteQueryCheck(string query);
    }
}
