using Visma;
using System.IO;

string username = "";
while(username == "")
{
    Console.Write("Enter your nickname: ");
    username = Console.ReadLine();
}
var session = new Session(username);
session.Run();
