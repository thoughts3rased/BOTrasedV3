using Microsoft.Data.SqlClient;

namespace BOTrasedV3.Interfaces
{
    public interface IDatabaseService
    {
        Task ExecuteNonQuery(string command, SqlParameter[] parameters);
        Task ExecuteNonQuery(string command);
        Task<T> ExecuteSprocJson<T>(string command, SqlParameter[] parameters);
        Task<T> ExecuteSprocJson<T>(string command);
        Task<SqlConnection> GetConnection();
    }
}
