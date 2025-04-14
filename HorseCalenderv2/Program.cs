// Main entry point for the Hoof Track application
// Handles debug mode initialization and program startup
// Command line arguments:
//   -D: Enables debug mode with additional console output


using HorseCalenderv2.utilities;
using Spectre.Console;

try {
  // Initialize logging system with debug flag
  Logging.Init(args.Contains("-D"));

// Configure debug mode if -D flag is present
  if (args.Contains("-D")) {
    AnsiConsole.MarkupLine("[red bold]Debug mode enabled![/]");
    Logging.Log("Debug mode enabled!");
  }

// Initialize data systems
  Data.Start();

// Start UI with debug mode setting
  UserInterface.Run(args.Contains("-D"));
}
catch (Exception e) {
  Console.WriteLine(e.Message);
  Console.ReadLine();
}
