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
    /// <summary>
    /// Class responsible for fetching data for the fun commands
    /// </summary>
    public class FunCommandDataService : IFunCommandDataService
    {
        private readonly IDatabaseService _databaseService;

        public FunCommandDataService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        /// <summary>
        /// Fetches a random fake secret from the database
        /// </summary>
        /// <returns>A random fake secret</returns>
        public async Task<FakeSecret> GetRandomSecretAsync()
        {
            return await _databaseService.ExecuteSprocJson<FakeSecret>(StoredProcedureNames.GetRandomFakeSecret);
        }
    }
}
