using BOTrasedV3.Interfaces;
using BOTrasedV3.Models;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace BOTrasedV3.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly IDatabaseService _databaseService;

        public UserDataService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        /// <summary>
        /// Writes a user to the database, whether they exist or not
        /// </summary>
        /// <param name="user"></param>
        /// <returns>True if the write was successful, else false</returns>
        public async Task<bool> WriteUser(UserData user)
        {
            string userJson = JsonSerializer.Serialize(user);

            SqlParameter[] parameters = [
                new SqlParameter("@json", userJson)
                ];

            try
            {
                await _databaseService.ExecuteNonQuery("WriteUserJson", parameters);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a new user with default values in the database from a user ID. If the user already exists, it does nothing
        /// </summary>
        /// <param name="userId">The ID of the user to create</param>
        /// <returns>True if the write was successful, else false</returns>
        public async Task<bool> CreateUserFromId(string userId)
        {
            SqlParameter[] parameters = [
                new SqlParameter("@userId", userId)
            ];

            try
            {
                await _databaseService.ExecuteNonQuery("CreateUserFromId", parameters);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
