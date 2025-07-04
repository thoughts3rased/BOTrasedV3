using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTrasedV3.Models
{
    public class UserData
    {
        public UserData(IUser user)
        {

        }
        
        public int Id { get; set; }

        public long ExperiencePoints { get; set; }

        public int Level { get; set; }
    }
}
