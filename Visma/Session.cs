using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Visma.Models;

namespace Visma
{
    public class Session
    {
        private readonly string Username;
        private readonly string MeetingsFile = "./Data/Meetings.json";


        public Session() { }
        public Session(string username)
        {
            Username = username;
        }

        public void Run()
        {
            Console.WriteLine(new String('-',30));
            Console.WriteLine("Hi, " + Username + "\nChoose command for further moves:");
            Dictionary<string, string> Commands = GetCommands();
            List<Meeting> Meetings = GetMeetings();
            PrintCommands(Commands);

            string SelectedCommand = Console.ReadLine();
            while (SelectedCommand != "/exit")
            {
                switch (SelectedCommand)
                {
                    case "/createmeeting":
                        CreateMeeting(Meetings);
                        PrintCommands(Commands);
                        Console.ReadLine();
                        break;
                    case "/deletemeeting":
                        Console.WriteLine("You have choosen to " + Commands[SelectedCommand]);
                        break;
                    default:
                        Console.WriteLine("Command not found! Commands list:");
                        PrintCommands(Commands);
                        SelectedCommand = Console.ReadLine();
                        break;
                }
            }
            if (SelectedCommand == "/exit")
                Console.WriteLine("Program exited successsfuly!");
        }


        public void CreateMeeting(List<Meeting> AllMeetings)
        {
            string Name = "";
            string ResponsiblePerson = "";
            string Description = "";
            string Category = "";
            string Type = "";
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            bool Valid = false;

            Console.WriteLine("You have chosen to create a meeting, please fill in the meeting's information");

            while (Name == "")
            {
                Console.Write("Name: ");
                Name = Console.ReadLine();
            }

            while (ResponsiblePerson == "")
            {
                Console.Write("Responsible person: ");
                ResponsiblePerson = Console.ReadLine();
            }

            Console.Write("Description: ");
            Description = Console.ReadLine();

            while (Category != "CodeMonkey" && Category != "Hub" && Category != "Short" && Category != "TeamBuilding")
            {
                Console.Write("Category (CodeMonkey/Hub/Short/TeamBuilding): ");
                Category = Console.ReadLine();
            }

            while (Type != "Live" && Type != "InPerson")
            {
                Console.Write("Type (Live/InPerson): ");
                Type = Console.ReadLine();
            }

            while (!Valid)
            {
                Console.Write("Start date: ");
                Valid = DateTime.TryParse(Console.ReadLine(), out StartDate);
            }

            Valid = false;
            while(!Valid)
            {
                Console.Write("End date: ");
                Valid = DateTime.TryParse(Console.ReadLine(), out EndDate);
            }

            Meeting NewMeeting = new Meeting(Name, ResponsiblePerson, Description, Category, Type, StartDate, EndDate);
            AllMeetings.Add(NewMeeting);

            Console.WriteLine("\nNew meeting was created successfuly!\n");
        }

        public void PrintCommands(Dictionary<string, string> Commands)
        {
            foreach (KeyValuePair<string, string> command in Commands)
            {
                Console.WriteLine("* {0,-15} - {1,-30}", command.Key, command.Value);
            }
            Console.WriteLine("Please type in command:");
        }

        public Dictionary<string, string> GetCommands()
        {
            Dictionary<string, string> Commands = new Dictionary<string, string>();
            Commands.Add("/createmeeting", "Create a new meeting");
            Commands.Add("/deletemeeting", "Delete an existing meeting");
            Commands.Add("/removeperson", "Remove user from existing meeting");
            Commands.Add("/addperson", "Add user to existing meeting");
            Commands.Add("/showmeetings", "Show a list of meetings");
            Commands.Add("/exit", "Exit program");
            return Commands;
        }

        public List<Meeting> GetMeetings()
        {
            List<Meeting> meetings = new List<Meeting>();
            try
            {
                meetings = JsonConvert.DeserializeObject<List<Meeting>>(File.ReadAllText(MeetingsFile));
            }
            catch (Exception)
            {
                Console.WriteLine("Error while reading data file! check " + MeetingsFile);
                Environment.Exit(-1);
            }
            return meetings;
        }
    }
}
