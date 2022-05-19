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
        public int Id { get; set; }
        public string Name { get; set; }
        public string ResponsibleUser { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public List<string> AttendingUsers { get; set; }

        public Meeting(int id, string name, string responsibleUser, string description, string category, string type, DateTime startDate, DateTime endDate, List<string> attendingUsers)
        {
            Id = id;
            Name = name;
            ResponsibleUser = responsibleUser;
            Description = description;
            Category = category;
            Type = type;
            DateStart = startDate;
            DateEnd = endDate;
            AttendingUsers = attendingUsers;
        }

        public Meeting(int id, string name, string responsibleUser, string description, string category, string type, DateTime startDate, DateTime endDate)
        {
            Id = id;
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


        public override string ToString()
        {
            string users = "";
            foreach (var user in AttendingUsers)
                if (!AttendingUsers.Last().Equals(user))
                    users += user + ", ";
                else
                    users += user;
            return String.Format("| {0,3} | {1,20} | {2,16} | {3,40} | {4,10} | {5,8} | {6,21} | {7,21} | {8}",
                Id, Name, ResponsibleUser, Description, Category, Type, DateStart, DateEnd, users);
        }
    }
}
