using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTrasedV3.Models
{
    public class Configuration
    {
        public string BotToken { get; set; } = string.Empty;

        public ulong? TestGuildId { get; set; }

        public string DbConnectionString { get; set; } = string.Empty;
    }
}
