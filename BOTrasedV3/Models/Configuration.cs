using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTrasedV3.Models
{
    /// <summary>
    /// Holds configuration settings for the bot
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// The token that the bot will use to connect to Discord
        /// </summary>
        public string BotToken { get; set; } = string.Empty;

        /// <summary>
        /// The ID of the guild (server) to register commands to in debug mode
        /// </summary>
        public ulong? TestGuildId { get; set; }

        /// <summary>
        /// Connection string for Microsoft SQL Server database
        /// </summary>
        public string DbConnectionString { get; set; } = string.Empty;
    }
}
