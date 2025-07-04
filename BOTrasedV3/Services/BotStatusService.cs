using BOTrasedV3.Constants;
using BOTrasedV3.Interfaces;
using BOTrasedV3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTrasedV3.Services
{
    public class BotStatusService : IBotStatusService
    {
        private readonly IDatabaseService _databaseService;

        public BotStatusService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }


        /// <summary>
        /// Fetches a random status for the bot from the database.
        /// </summary>
        /// <returns>A status for the bot.</returns>
        public async Task<BotStatus> GetRandomStatus()
        {
            return await _databaseService.ExecuteSprocJson<BotStatus>(StoredProcedureNames.GetRandomStatus);
        }
    }
}
