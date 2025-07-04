using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTrasedV3.Models
{
    public class BotStatus
    {
        public int Id { get; set; }

        public string StatusText { get; set; }

        public ActivityType ActivityType { get; set; }
    }
}
