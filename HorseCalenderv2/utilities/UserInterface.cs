using HorseCalenderv2.utilities;
using HorseCalenderv2.classes;
using Spectre.Console;

namespace HorseCalenderv2.utilities;

/// <summary>
/// Handles the main user interface and menu system
/// </summary>
public static class UserInterface {
  /// <summary>
  /// Runs the main application interface loop
  /// </summary>
  /// <param name="isDebug">If true, preserves console output between refreshes</param>
  public static void Run(bool isDebug = false) {
    AnsiConsole.Cursor.Hide();
    
    while (true) {
      // Clear screen unless in debug mode
      if (!isDebug)
        AnsiConsole.Clear();
        
      // Draw application header
      DrawHeader();
      
      // Display statistics
      ShowStats();
      
      AnsiConsole.Write(new Markup("[slowblink][[Press any action key]][/]").Centered());
      
      // Wait for and process user input
      ProcessInput();
      
      // Save data, so that all the changes made in this cycle are preserved
      Data.Save();
    }
  }
  
  /// <summary>
  /// Draws the application title and subtitle
  /// </summary>
  public static void DrawHeader() {
    AnsiConsole.Write(
      new FigletText(FigletFont.Load("./fonts/larry3d.flf"), "Hoof Track")
        .Centered());
    AnsiConsole.Write(
      new Markup("[bold]Farrier Scheduling Software[/]")
        .Centered());

    AnsiConsole.Write(new Rule("[yellow][[Main Menu]][/]").RuleStyle("grey").Centered());
  }
  
  /// <summary>
  /// Displays current horse count and overdue status
  /// </summary>
  public static void ShowStats() {
    AnsiConsole.Write(new Markup($"HORSES ACTIVE IN ROTATION: {Data.Horses.Count}").Centered());
    var overdue = Data.Horses.Count(horse => horse.WeeksSinceLastShoe > horse.RotationInterval);
    AnsiConsole.Write(
      new Markup($"HORSES OVERDUE: {overdue}")
        .Centered());
    
    AnsiConsole.WriteLine();
    AnsiConsole.WriteLine();
  }
  
  /// <summary>
  /// Handles keyboard input and executes corresponding actions
  /// </summary>
  private static void ProcessInput() {
    var key = Console.ReadKey(true).Key;
    if (!Data.KeyBinds.ContainsValue(key)) return;
    
    foreach (var bind in Data.KeyBinds.Where(bind => bind.Value == key)) {
      Data.Actions[bind.Key]();
      break;
    }
  }
}