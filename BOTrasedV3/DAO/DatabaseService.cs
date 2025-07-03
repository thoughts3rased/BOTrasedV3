using BOTrasedV3.Models;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTrasedV3.DAO
{
    public class DatabaseService
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
        /// <param name="parameters">The parameters to pass to the stored procedure</param>
        /// <returns></returns>
        public T ExecuteSprocJson<T>(string command, MySqlParameter[] parameters)
        {
            using (MySqlConnection connection = GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand(command, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                using (MySqlDataReader reader = cmd.ExecuteReader())
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
        /// <param name="parameters">The parameters to pass to the stored procedure</param>
        public void ExecuteNonQuery(string command, MySqlParameter[] parameters)
        {
            using (MySqlConnection connection = GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand(command, connection);
                cmd.CommandType= CommandType.StoredProcedure;

                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Gets an open MySQL connection.
        /// </summary>
        /// <returns>An open MySqlConnection object.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the connection string is not set or if the connection cannot be opened.</exception>
        public MySqlConnection GetConnection()
        {
            if (string.IsNullOrWhiteSpace(_config.ConnectionString))
            {
                throw new InvalidOperationException("Connection string is not set. Please ensure it's configured correctly.");
            }

            MySqlConnection connection = new MySqlConnection(_config.ConnectionString);
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    Console.WriteLine("MySQL Connection opened successfully.");
                }
                return connection;
            }
            catch (MySqlException ex)
            {
                Console.Error.WriteLine($"Error opening MySQL connection: {ex.Message}");
                throw new InvalidOperationException("Could not open database connection.", ex);
            }
        }

        /// <summary>
        /// Closes a MySQL connection if it is open.
        /// </summary>
        /// <param name="connection">The MySqlConnection object to close.</param>
        public void CloseConnection(MySqlConnection connection)
        {
            if (connection == null)
            {
                Console.WriteLine("Attempted to close a null connection.");
                return;
            }

            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("MySQL Connection closed successfully.");
                }
            }
            catch (MySqlException ex)
            {
                Console.Error.WriteLine($"Error closing MySQL connection: {ex.Message}");
            }
            finally
            {
                // Dispose of the connection to release resources.
                connection.Dispose();
            }
        }
    }
}
