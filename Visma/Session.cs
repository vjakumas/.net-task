using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Visma.Models;

namespace Visma
{
    public class Session
    {
        private readonly string Username; // Saved username
        private readonly string MeetingsFile = "../../../Data/Meetings.json"; // Data file path


        public Session() { }
        public Session(string username)
        {
            Username = username;
        }

        /// <summary>
        /// Method to run program.
        /// </summary>
        public void Run()
        {
            Console.WriteLine(new String('-',30));
            Console.WriteLine("Hi, " + Username + "\nChoose command to manage meetings:");
            Dictionary<string, string> Commands = InOut.GetCommands();
            List<string> FilteringOptions = InOut.GetFilterOptions();
            List<Meeting> Meetings = InOut.GetMeetings(MeetingsFile);
            InOut.PrintCommands(Commands);

            string SelectedCommand = Console.ReadLine();
            while (SelectedCommand != "/exit")
            {
                switch (SelectedCommand)
                {
                    case "/createmeeting":
                        CreateMeeting(Meetings);
                        InOut.PrintCommands(Commands);
                        SelectedCommand = Console.ReadLine();
                        break;
                    case "/deletemeeting":
                        DeleteMeeting(Meetings);
                        InOut.PrintCommands(Commands);
                        SelectedCommand = Console.ReadLine();
                        break;
                    case "/removeuser":
                        RemoveUser(ref Meetings);
                        InOut.PrintCommands(Commands);
                        SelectedCommand = Console.ReadLine();
                        break;
                    case "/adduser":
                        AddUser(ref Meetings);
                        InOut.PrintCommands(Commands);
                        SelectedCommand = Console.ReadLine();
                        break;
                    case "/showmeetings":
                        ShowMeetings(Meetings, FilteringOptions);
                        InOut.PrintCommands(Commands);
                        SelectedCommand = Console.ReadLine();
                        break;
                    default:
                        Console.WriteLine("Command not found! Commands list:");
                        InOut.PrintCommands(Commands);
                        SelectedCommand = Console.ReadLine();
                        break;
                }
            }
            if (SelectedCommand == "/exit")
                Console.WriteLine("Program exited successsfuly!");
        }

        /// <summary>
        /// Method to display all meetings.
        /// </summary>
        /// <param name="Meetings">List of all meetings.</param>
        /// <param name="FilterOptions">List of filter options.</param>
        public void ShowMeetings(List<Meeting> Meetings, List<string> FilterOptions)
        {

            string Answer = "";
            string FilterOption = "";
            bool valid = false;

            InOut.PrintMeetings(Meetings);
            Console.WriteLine("Do you want to filter data? [Y/N]");

            while (Answer != "Y" && Answer !="y" && Answer != "N" && Answer != "n")
            {
                Answer = Console.ReadLine();

                if(Answer == "Y" || Answer == "y")
                {
                    Answer = "Y";
                    Console.WriteLine("Filter options:");
                    InOut.PrintFilterOptions(FilterOptions);
                    while (!valid)
                    {
                        Console.Write("Selected filter option: ");
                        FilterOption = Console.ReadLine();
                        valid = FilterOptions.Contains(FilterOption);
                        if(valid == false)
                            Console.WriteLine("Incorrect filter option! Select from the list above.");
                    }
                    FilterMeetings(Meetings, FilterOption);
                }

                Console.WriteLine("Do you want to filter data? [Y/N]");
            }

        }

        /// <summary>
        /// Method to filter meetings by filter option.
        /// </summary>
        /// <param name="Meetings">List of all meetings.</param>
        /// <param name="SelectedOption">Filter option.</param>
        public void FilterMeetings(List<Meeting> Meetings, string SelectedOption)
        {
            if (SelectedOption == "description" || SelectedOption == "responsible person" || SelectedOption == "category" || SelectedOption == "type")
            {
                Console.Write("Type search keyword: ");
                string Keyword = Console.ReadLine();
                string Pattern = @"" + Keyword;
                List<Meeting> matchedMeetings = new List<Meeting>();
                foreach (var meeting in Meetings)
                {
                    Match match = null;
                    if (SelectedOption == "description")
                    {
                        match = Regex.Match(meeting.Description, Pattern);
                    }
                    if(SelectedOption == "responsible person")
                    {
                        match = Regex.Match(meeting.ResponsibleUser, Pattern);
                    }
                    if (SelectedOption == "category")
                    {
                        match = Regex.Match(meeting.Category, Pattern);
                    }
                    if (SelectedOption == "type")
                    {
                        match = Regex.Match(meeting.Type, Pattern);
                    }


                    if (match.Success)
                    {
                        matchedMeetings.Add(meeting);
                    }
                }
                if (matchedMeetings.Count != 0)
                    InOut.PrintMeetings(matchedMeetings);
                else
                    Console.WriteLine("No meetings found!");
            }

            if(SelectedOption == "date")
            {
                DateTime StartDate = DateTime.Now;
                DateTime EndDate = DateTime.Now;
                bool Valid = false;
                List<Meeting> matchedMeetings = new List<Meeting>();

                while (!Valid)
                {
                    Console.Write("Start date: ");
                    Valid = DateTime.TryParse(Console.ReadLine(), out StartDate);
                    if (!Valid)
                        Console.WriteLine("Incorrect DateTime format! Use this format: YYYY/MM/DD HH:MM:SS");
                }

                Valid = false;
                while (!Valid)
                {
                    Console.Write("End date: ");
                    Valid = DateTime.TryParse(Console.ReadLine(), out EndDate);
                    if(!Valid)
                        Console.WriteLine("Incorrect DateTime format! Use this format: YYYY/MM/DD HH:MM:SS");
                }
                foreach (var meeting in Meetings)
                {
                    if (meeting.DateStart >= StartDate && meeting.DateEnd <= EndDate)
                    {
                        matchedMeetings.Add(meeting);
                    }
                }
                InOut.PrintMeetings(matchedMeetings);
            }

            if(SelectedOption == "attendees")
            {
                int AttendeesCount = 0;
                bool Valid = false;
                List<Meeting> matchedMeetings = new List<Meeting>();

                while (!Valid)
                {
                    Console.Write("Minimum attendees: ");
                    Valid = int.TryParse(Console.ReadLine(), out AttendeesCount);
                    if(!Valid)
                        Console.WriteLine("Incorrect input type. Use numbers only!");
                }

                foreach (var meeting in Meetings)
                {
                    if (meeting.AttendingUsers.Count >= AttendeesCount)
                        matchedMeetings.Add(meeting);
                }

                InOut.PrintMeetings(matchedMeetings);
            }
        }


        /// <summary>
        /// Method to add new user to meetings list.
        /// </summary>
        /// <param name="Meetings">List of all meetings.</param>
        public void AddUser(ref List<Meeting> Meetings)
        {
            InOut.PrintUsersInMeeting(Meetings[0]);
            int SelectedMeetingID;
            bool ValidId = false;

            InOut.PrintMeetings(Meetings);
            Console.WriteLine("Please select meeting ID to which the user should be added");

            while (!ValidId)
            {
                Console.Write("Meeting ID: ");
                ValidId = int.TryParse(Console.ReadLine(), out SelectedMeetingID);
                if (ValidId)
                {
                    Meeting meeting = Meetings.Where(m => m.Id == SelectedMeetingID).FirstOrDefault();
                    if (meeting == null)
                    {
                        ValidId = false;
                        Console.WriteLine("Meeting with selected ID does not exist. Try different one.");
                    }
                    else
                    {
                        Meeting? overlappingMeeting;
                        Console.Write("Type Username you want to add: ");
                        string SelectedUsername = Console.ReadLine();

                        if (meeting.AttendingUsers.Contains(SelectedUsername) && meeting.AttendingUsers.Count != 0)
                        {
                            Console.WriteLine("This user is already in selected meeting!");
                        }
                        else if (OverlapsWithAnotherMeeting(Meetings, meeting, SelectedUsername, out overlappingMeeting))
                        {
                            Console.WriteLine("WARNING! This user is already in a meeting that overlaps with the one being added!" +
                                              "\nOverlaping meeting id: " + meeting.Id);
                        }
                        else
                        {
                            Meetings.Where(m => m.Id == meeting.Id).FirstOrDefault().AttendingUsers.Add(SelectedUsername);
                            Console.WriteLine("User '" + SelectedUsername + "' was assigned to meeting - '" + meeting.Name + "'");
                        }
                    }
                }
                else
                    Console.WriteLine("Invalid ID format (use numbers only)");
            }
        }

        /// <summary>
        /// Checks if user is already in a meeting that overlaps with the one being added.
        /// </summary>
        /// <param name="Meetings">List of all meetings.</param>
        /// <param name="SelectedMeeting">Selected meeting.</param>
        /// <param name="Username">Selected user.</param>
        /// <param name="overlappingMeeting">references overlaping meeting.</param>
        /// <returns>True if overlaps.</returns>
        public bool OverlapsWithAnotherMeeting(List<Meeting> Meetings, Meeting SelectedMeeting, string Username, out Meeting overlappingMeeting)
        {
            bool result = false;
            overlappingMeeting = null;
            foreach(var meeting in Meetings)
            {

                if (meeting.Id != SelectedMeeting.Id)
                {
                    foreach (var user in meeting.AttendingUsers)
                    {
                        if (Username == user)
                        {
                            if (meeting.DateStart < SelectedMeeting.DateEnd && meeting.DateEnd > SelectedMeeting.DateStart)
                            {
                                result = true;
                                overlappingMeeting = meeting;
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Method to remove user from list of meetings.
        /// </summary>
        /// <param name="Meetings">list of all meetings.</param>
        public void RemoveUser(ref List<Meeting>Meetings)
        {
            string SelectedUser;
            int SelectedMeetingID;
            bool ValidId = false;

            Console.WriteLine("Please select meeting ID from which the user should be removed");
            InOut.PrintMeetings(Meetings);
            
            while(!ValidId)
            {
                Console.Write("Meeting ID: ");
                ValidId = int.TryParse(Console.ReadLine(), out SelectedMeetingID);
                if(ValidId)
                {
                    bool ValidUser = false;
                    Meeting meeting = Meetings.Where(m => m.Id == SelectedMeetingID).FirstOrDefault();
                    if (meeting == null)
                    {
                        ValidId = false;
                        Console.WriteLine("Meeting with selected ID does not exist. Try different one.");
                    }
                    else
                    {
                        while (!ValidUser)
                        {
                            InOut.PrintUsersInMeeting(meeting);
                            Console.Write("User you want to remove from the meeting: ");
                            SelectedUser = Console.ReadLine();
                            if (meeting.AttendingUsers.Contains(SelectedUser))
                            {
                                if (meeting.ResponsibleUser == Username && SelectedUser == Username)
                                {
                                    Console.WriteLine("You cannot remove yourself from the meeting you're responsible for!");
                                }
                                else
                                {
                                    meeting.AttendingUsers.Remove(SelectedUser);
                                    Meetings.Where(m => m.Id == SelectedMeetingID).ToList().RemoveAll(u => u.Name == SelectedUser);
                                    Console.WriteLine("User '" + SelectedUser + "' was successfuly removed from the meeting!\n");
                                    ValidUser = true;
                                }
                            }
                            else
                                Console.WriteLine("The selected user is not included in the meeting");
                        }
                    }
                } 
                else
                    Console.WriteLine("Invalid ID format (use numbers only)");
            }
        }

        /// <summary>
        /// Method to delete meeting.
        /// </summary>
        /// <param name="Meetings">List of all meeting.</param>
        public void DeleteMeeting(List<Meeting> Meetings)
        {
            int SelectedMeetingID;
            bool Valid = false;

            Console.WriteLine("You have choosen to delete an existing meeting.");
            InOut.PrintMeetings(Meetings);

            while (!Valid)
            {
                Console.Write("Please select meeting ID: ");
                Valid = int.TryParse(Console.ReadLine(), out SelectedMeetingID);
                if(!Valid)
                    Console.WriteLine("Invalid ID format (use numbers only)");
                else
                    Valid = DeleteMeetingByID(SelectedMeetingID, ref Meetings);
            }
        }

        /// <summary>
        /// Method to delete meeting by its ID.
        /// </summary>
        /// <param name="id">meeting id that should be deleted.</param>
        /// <param name="Meetings">List of all meetings.</param>
        /// <returns>True if deleted.</returns>
        public bool DeleteMeetingByID(int id, ref List<Meeting> Meetings)
        {
            bool successful = false;
            Meeting meeting = Meetings.Where(m => m.Id == id).FirstOrDefault();
            if(meeting != null)
            {
                if (meeting.ResponsibleUser == Username)
                {
                    Meetings.Remove(meeting);
                    successful = true;
                    Console.WriteLine("Meeting was deleted successfuly!\n");
                }
                else
                    Console.WriteLine("Only responsible user of this meeting can delete this meeting!");
            }
            else
                Console.WriteLine("Meeting not found, select existing meeting id!");

            return successful;
        }

        /// <summary>
        /// Method to create a new meeting.
        /// </summary>
        /// <param name="AllMeetings">LIst of all meeting.</param>
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

            Console.WriteLine("\nYou have chosen to create a meeting, please fill in the meeting's information");

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

            int NewMeetingID = AllMeetings[AllMeetings.Count - 1].Id + 1;
            Meeting NewMeeting = new Meeting(NewMeetingID, Name, ResponsiblePerson, Description, Category, Type, StartDate, EndDate);
            AllMeetings.Add(NewMeeting);

            InOut.SaveMeetings(AllMeetings, MeetingsFile);

            Console.WriteLine("\nNew meeting was created successfuly!\n");
        }
    }
}
