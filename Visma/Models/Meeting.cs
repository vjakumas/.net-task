using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visma;

namespace Visma.Models
{
    public class Meeting
    {
        public string Name { get; set; }
        public string ResponsibleUser { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public List<string> AttendingUsers { get; set; }

        public Meeting(string name, string responsibleUser, string description, string category, string type, DateTime startDate, DateTime endDate, List<string> attendingUsers)
        {
            Name = name;
            ResponsibleUser = responsibleUser;
            Description = description;
            Category = category;
            Type = type;
            DateStart = startDate;
            DateEnd = endDate;
            AttendingUsers = attendingUsers;
        }

        public Meeting(string name, string responsibleUser, string description, string category, string type, DateTime startDate, DateTime endDate)
        {
            Name = name;
            ResponsibleUser = responsibleUser;
            Description = description;
            Category = category;
            Type = type;
            DateStart = startDate;
            DateEnd = endDate;
            AttendingUsers = new List<string>();
        }

        public Meeting() { }

    }
}
