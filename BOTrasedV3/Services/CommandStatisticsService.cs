using BOTrasedV3.Interfaces;
using Microsoft.Data.SqlClient;

namespace BOTrasedV3.Services
{
    public class CommandStatisticsService
    {
        private readonly IDatabaseService _databaseService;

        public CommandStatisticsService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        /// <summary>
        /// Records a usage of a command in the database
        /// </summary>
        /// <param name="commandName">the name of the command that was used</param>
        public async void LogCommandUsage(string commandName)
        {
            SqlParameter[] parameters =
            [
                new SqlParameter("commandName", commandName)
            ];

            await _databaseService.ExecuteNonQuery("LogCommandUsage", parameters);
        }
    }
}
