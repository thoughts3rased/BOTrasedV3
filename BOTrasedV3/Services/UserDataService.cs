using BOTrasedV3.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTrasedV3.Services
{
    public class UserDataService
    {
        private readonly IDatabaseService _databaseService;

        public UserDataService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task CreateUserIfNotExists(string userId)
        {
           
        }
    }
}
