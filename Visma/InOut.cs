using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Visma.Models;
using Newtonsoft.Json;

namespace Visma
{
    public class InOut
    {
        public static void PrintMeetings(List<Meeting> Meetings)
        {
            PrintMeetingsHeader();
            foreach (Meeting meeting in Meetings)
            {
                Console.WriteLine(meeting.ToString());
            }
            Console.WriteLine(new String('-', 184));
        }

        public static void PrintMeetingsHeader()
        {
            Console.WriteLine(new String('-', 184));
            Console.WriteLine("| {0,3} | {1,20} | {2,15} | {3,40} | {4,10} | {5,8} | {6,21} | {7,21} | Users in meeting: |",
                "ID", "Meeting name", "Responsible user", "Description", "Category", "Type", "Start date", "End date");
            Console.WriteLine(new String('-', 184));
        }

        public static void PrintCommands(Dictionary<string, string> Commands)
        {
            Console.WriteLine();
            foreach (KeyValuePair<string, string> command in Commands)
            {
                Console.WriteLine("* {0,-15} - {1,-30}", command.Key, command.Value);
            }
            Console.WriteLine("Please type in command:");
        }

        public static void PrintUsersInMeeting(Meeting meeting)
        {
            Console.WriteLine("Users included in meeting ID #" + meeting.Id +":");
            foreach (var user in meeting.AttendingUsers)
            {
                Console.WriteLine("* " + user);
            }
        }

        public static void PrintFilterOptions(List<string> FilterOptions)
        {
            foreach (var opt in FilterOptions)
            {
                Console.WriteLine("* " + opt);
            }
        }

        /// <summary>
        /// Method to get all meetings from .json data file.
        /// </summary>
        /// <returns>List of meetings.</returns>
        public static List<Meeting> GetMeetings(string MeetingsFile)
        {
            List<Meeting> meetings = new List<Meeting>();
            try
            {
                meetings = JsonConvert.DeserializeObject<List<Meeting>>(File.ReadAllText(MeetingsFile));
            }
            catch (Exception)
            {
                Console.WriteLine("Error while reading data file! check " + Path.GetFullPath(MeetingsFile));
                Environment.Exit(-1);
            }
            return meetings;
        }

        /// <summary>
        /// Method to get all available commands.
        /// </summary>
        /// <returns>Dictionary of commands.</returns>
        public static Dictionary<string, string> GetCommands()
        {
            Dictionary<string, string> Commands = new Dictionary<string, string>();
            Commands.Add("/createmeeting", "Create a new meeting");
            Commands.Add("/deletemeeting", "Delete an existing meeting");
            Commands.Add("/removeuser", "Remove user from existing meeting");
            Commands.Add("/adduser", "Add user to existing meeting");
            Commands.Add("/showmeetings", "Show a list of meetings");
            Commands.Add("/exit", "Exit program");
            return Commands;
        }

        /// <summary>
        /// Method to get list of filter options.
        /// </summary>
        /// <returns>List of filter options.</returns>
        public static List<string> GetFilterOptions()
        {
            List<string> FilteringOptions = new List<string>();

            FilteringOptions.Add("description");
            FilteringOptions.Add("responsible person");
            FilteringOptions.Add("category");
            FilteringOptions.Add("type");
            FilteringOptions.Add("date");
            FilteringOptions.Add("attendees");


            return FilteringOptions;
        }

        /// <summary>
        /// Method to save meetings to .json data file.
        /// </summary>
        /// <param name="Meetings">List of all meetings.</param>
        public static void SaveMeetings(List<Meeting> Meetings, string MeetingsFile)
        {
            string jsonString = JsonConvert.SerializeObject(Meetings, Formatting.Indented);
            File.WriteAllText(MeetingsFile, jsonString);
        }
    }
}
