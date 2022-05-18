using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visma.Models
{
    public class User
    {
        public string Username { get; set; }
        public List<Meeting> MeetingsList { get; set; }

        public User (string username, List<Meeting> meetingsList)
        {
            Username = username;
            MeetingsList = meetingsList;
        }

        public User() { }
    }
}
