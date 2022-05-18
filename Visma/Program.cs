using Visma;
using System.IO;

string username = "";
while(username == "")
{
    Console.Write("Enter your nickname: ");
    username = Console.ReadLine();
    Console.WriteLine(DateTime.Now); ;
}
var session = new Session(username);
session.Run();
