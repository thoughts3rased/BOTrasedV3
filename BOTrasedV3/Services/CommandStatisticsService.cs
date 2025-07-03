using BOTrasedV3.Interfaces;
using Microsoft.Data.SqlClient;

namespace BOTrasedV3.Services
{
    /// <summary>
    /// Utility class responsible for performing actions related to the logging of command statistics 
    /// </summary>
    public class CommandStatisticsService : ICommandStatisticsService
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
        public async Task LogCommandUsage(string commandName)
        {
            SqlParameter[] parameters =
            [
                new SqlParameter("commandName", commandName)
            ];

            await _databaseService.ExecuteNonQuery("LogCommandUsage", parameters);
        }
    }
}
