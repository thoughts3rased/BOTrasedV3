using BOTrasedV3.Interfaces;
using BOTrasedV3.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Data;

namespace BOTrasedV3.DAO
{
    public class DatabaseService : IDatabaseService
    {
        private readonly Configuration _config;

        public DatabaseService(IOptions<Configuration> config)
        {
            _config = config.Value;
        }

        /// <summary>
        /// Executes a stored procedure that returns JSON and serialises it to <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type to deserialise the JSON to</typeparam>
        /// <param name="command">The name of the stored procedure to execute</param>
        /// <returns>A deserialised <typeparamref name="T"/> from the result of the stored proc</returns>
        public async Task<T> ExecuteSprocJson<T>(string command)
        {
            return await ExecuteSprocJson<T>(command, []);
        }

        /// <summary>
        /// Executes a stored procedure that returns JSON and serialises it to <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type to deserialise the JSON to</typeparam>
        /// <param name="command">The name of the stored procedure to execute</param>
        /// <param name="parameters">The parameters to pass to the stored procedure</param>
        /// <returns>A deserialised <typeparamref name="T"/> from the result of the stored proc</returns>
        public async Task<T> ExecuteSprocJson<T>(string command, SqlParameter[] parameters)
        {
            using (SqlConnection connection = await GetConnection())
            {
                SqlCommand cmd = new SqlCommand(command, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                // Consider making this method async and using ExecuteReaderAsync for better performance
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        string jsonResult = reader.GetString(0); // Get the JSON string from the first column
                        return JsonConvert.DeserializeObject<T>(jsonResult); // Deserialize and return
                    }
                    else
                    {
                        return default;
                    }
                }
            }
        }

        /// <summary>
        /// Executes a non-returning stored procedure
        /// </summary>
        /// <param name="command">The stored procedure to update</param>
        public async Task ExecuteNonQuery(string command)
        {
            await ExecuteNonQuery(command, []);
        }

        /// <summary>
        /// Executes a non-returning stored procedure
        /// </summary>
        /// <param name="command">The stored procedure to update</param>
        /// <param name="parameters">The parameters to pass to the stored procedure</param>
        public async Task ExecuteNonQuery(string command, SqlParameter[] parameters)
        {
            using (SqlConnection connection = await GetConnection())
            {
                SqlCommand cmd = new SqlCommand(command, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                await cmd.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Gets an open MSSQL connection.
        /// </summary>
        /// <returns>An open SqlConnection object.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the connection string is not set or if the connection cannot be opened.</exception>
        public async Task<SqlConnection> GetConnection()
        {
            if (string.IsNullOrWhiteSpace(_config.DbConnectionString))
            {
                throw new InvalidOperationException("Connection string is not set. Please ensure it's configured correctly.");
            }

            SqlConnection connection = new SqlConnection(_config.DbConnectionString);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                    Console.WriteLine("MSSQL Connection opened successfully.");
                }
                return connection;
            }
            catch (SqlException ex)
            {
                Console.Error.WriteLine($"Error opening MSSQL connection: {ex.Message}");
                throw new InvalidOperationException("Could not open database connection.", ex);
            }
        }
    }
}
