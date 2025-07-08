using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTrasedV3.Models
{
    /// <summary>
    /// A class representing a predefined activity status for the bot to display
    /// </summary>
    public class BotStatus
    {
        /// <summary>
        /// Unique identifier for the status
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The text for the activity status that the bot will display
        /// </summary>
        public string StatusText { get; set; } = string.Empty;

        /// <summary>
        /// The displayed activity type (Playing, Listening, Watching, etc.)
        /// </summary>
        public ActivityType ActivityType { get; set; }
    }
}
