using System.Diagnostics;
using HorseCalenderv2.classes;
using HorseCalenderv2.utilities;
using Spectre.Console;
using HorseCalenderv2.selectors;

namespace HorseCalenderv2.utilities;

public static class Actions {
  private static void BoilerPlate() {
    AnsiConsole.Clear();
    UserInterface.DrawHeader();
    UserInterface.ShowStats();
  }

  public static void AddHorseAction() {
    BoilerPlate();

    Logging.Log("Caught Add Horse action");

    var name = AnsiConsole.Prompt(
      new TextPrompt<string>("Enter the horse's name:")
        .PromptStyle("grey")
        .AllowEmpty()
    );

    if (string.IsNullOrWhiteSpace(name))
      return; // Exit if empty name

    // Get rotation interval
    var rotationInterval = AnsiConsole.Prompt(
      new TextPrompt<int>("Enter the horse's rotation interval:")
        .PromptStyle("grey")
        .Validate(interval =>
          interval >= 0 ? ValidationResult.Success() : ValidationResult.Error("Rotation interval cannot be negative"))
    );
    
    // Ask if the user wants to set the last shod date to today
    var dateFlag = AnsiConsole.Confirm("Set last shod date to today?", false);
    
    // Get last shoe date
    var lastShoeDate = !dateFlag ? DateSelector.DateInput() : DateTime.Now;
    
    // All data collected successfully, add the horse
    Logging.Log($"Adding horse: {name}, {rotationInterval}, {lastShoeDate}");
    Data.Horses.Add(new Horse(name, rotationInterval, lastShoeDate));
    AnsiConsole.MarkupLine("[green]Horse added successfully![/]");
    
    // Pause so that the user can see the success message
    Thread.Sleep(1000);
  }

  public static void RemoveHorseAction() {
    Logging.Log("Caught Remove Horse action");
    
    BoilerPlate();
    
    // If there are more than 10 horses, show a message indicating more horses are available
    var horses = AnsiConsole.Prompt(
      new MultiSelectionPrompt<string>()
        .Title("Select horses to remove:")
        .PageSize(10)
        .AddChoices(Data.Horses.Select(h => $"{h.Name} ({h.Id})"))
        .AddChoices("[red]Cancel[/]")
        .MoreChoicesText("[grey](Move up and down to reveal more horses)[/]")
        .InstructionsText(
          "[grey](Press [blue]<space>[/] to toggle a horses, " + 
          "[green]<enter>[/] to accept)[/]")
    );
    
    if (horses.Contains("[red]Cancel[/]"))
      return; // Exit if user cancels
    
    // Remove the selected horses
    foreach (var horse in horses) {
      Data.Horses.RemoveAll(h => $"{h.Name} ({h.Id})" == horse);
      Logging.Log($"Removed horse: {horse}");
    }
    
    AnsiConsole.MarkupLine($"[green]Removed {horses.Count} horse(s) successfully![/]");
    Thread.Sleep(1000);
  }

  public static void EditMenuAction() {
    Logging.Log("Caught Edit Menu action");
    
    BoilerPlate();

    var horse = AnsiConsole.Prompt(
      new SelectionPrompt<string>()
        .Title("Select a horse to edit:")
        .PageSize(10)
        .AddChoices(Data.Horses.Select(h => $"{h.Name} ({h.Id})"))
        .AddChoices("[red]Cancel[/]")
        .MoreChoicesText("[grey](Move up and down to reveal more horses)[/]")
        .EnableSearch()
    );
    if (horse == "[red]Cancel[/]")
      return;
    
    var selected = Data.Horses.First(h => $"{h.Name} ({h.Id})" == horse);
    
    var name = AnsiConsole.Prompt(
      new TextPrompt<string>("Enter the horse's name:")
        .PromptStyle("grey")
        .DefaultValue(selected.Name)
        .AllowEmpty()
    );
    
    if (string.IsNullOrWhiteSpace(name))
      return; // Exit if empty name
    
    // Get rotation interval
    var rotationInterval = AnsiConsole.Prompt(
      new TextPrompt<int>("Enter the horse's rotation interval:")
        .PromptStyle("grey")
        .DefaultValue(selected.RotationInterval)
        .Validate(interval =>
          interval >= 0 ? ValidationResult.Success() : ValidationResult.Error("Rotation interval cannot be negative"))
    );
    
    var lastShoeDate = DateSelector.DateInput(selected.LastShoeDate);
    
    // All data collected successfully, add the horse
    Logging.Log($"Editing horse: {name}, {selected.RotationInterval}, {lastShoeDate}");
    selected.Name = name;
    selected.RotationInterval = rotationInterval;
    selected.LastShoeDate = lastShoeDate;
    AnsiConsole.MarkupLine("[green]Horse edited successfully![/]");
  }

  public static void MasterListAction() {
    // TODO: Implement MasterList
    Logging.Log("Caught Master List action");
    var exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MasterList.exe");
    Logging.Log("Path to masterlist.exe: " + exePath);

    try {
      var startInfo = new ProcessStartInfo
      {
        FileName = exePath,
        UseShellExecute = true,     // Use system shell to start process
        CreateNoWindow = false      // Allow new window creation
      };
      Process.Start(startInfo);
    }
    catch (Exception e) {
      Logging.Log(e.Message);
      AnsiConsole.MarkupLine("[red]Failed to open Master List![/]");
    }
    BoilerPlate();
    
  }

  public static void BulkResetAction() {
    Logging.Log("Caught Bulk Reset action");
    
    BoilerPlate();
    
    // If there are more than 10 horses, show a message indicating more horses are available
    var horses = AnsiConsole.Prompt(
      new MultiSelectionPrompt<string>()
        .Title("Select horses to reset:")
        .PageSize(10)
        .AddChoices(Data.Horses.Select(h => $"{h.Name} ({h.Id})"))
        .AddChoices("[red]Cancel[/]")
        .MoreChoicesText("[grey](Move up and down to reveal more horses)[/]")
        .InstructionsText(
          "[grey](Press [blue]<space>[/] to toggle a horses, " + 
          "[green]<enter>[/] to accept)[/]")
    );
    
    // Ask if the user wants to set the last shod date to today
    var dateFlag = AnsiConsole.Confirm("Set reset date to today?", false);
    
    // Get last shoe date
    var resetDate = !dateFlag ? DateSelector.DateInput() : DateTime.Now;
    
    // Remove the selected horses
    foreach (var horse in horses) {
      Data.Horses.First(h => $"{h.Name} ({h.Id})" == horse).LastShoeDate = resetDate;
      Logging.Log($"Reset horse {horse} to {resetDate.Date}");
    }
    
    Logging.Log("Bulk reset complete");
    AnsiConsole.MarkupLine($"[green]Reset {horses.Count} horse(s) successfully![/]");
    Thread.Sleep(1000);
  }

  /// <summary>
  /// Quits the application, while saving all data and safely closing.
  /// </summary>
  public static void QuitAction() {
    Logging.Log("Caught Exit action");

    Logging.Log("Exiting...");
    Data.Exit();

    Logging.Log("Goodbye!");

    Console.WriteLine("Press any key to exit...");
    Console.ReadKey(true);
    Environment.Exit(0);
  }
}