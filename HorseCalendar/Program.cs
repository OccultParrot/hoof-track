using HorseCalendar;
using System.IO;

Data.Start();
Console.WriteLine("Welcome to Horse Calendar");
Utils.AutoCompleteInput(["test"]);
UserInterface.Run();
Data.Exit();