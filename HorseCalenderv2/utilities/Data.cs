using System.IO;
using System.Text.Json;
using HorseCalenderv2.classes;
using static HorseCalenderv2.utilities.Print;
using static HorseCalenderv2.utilities.Actions;

namespace HorseCalenderv2.utilities;

/// <summary>
/// Static utility class for managing application data, including horses and key bindings
/// </summary>
public static class Data {
  // JSON serialization settings
  private static readonly JsonSerializerOptions JsonOptions = new() {
    IncludeFields = true,
    WriteIndented = true
  };

  // Main data collections
  public static List<Horse> Horses { get; private set; } = [];
  public static Dictionary<string, ConsoleKey> KeyBinds { get; private set; } = [];
  public static Dictionary<string, Action> Actions { get; } = [];

  /// <summary>
  /// Initializes the data utility by checking data folders, loading data, and binding actions
  /// </summary>
  public static void Start() {
    Logging.Log("Loading...");
    CheckDataFolder();
    Load();

    Logging.Log("Binding actions...");
    BindActions();

    Logging.Log("Data utility initialized!");
  }

  /// <summary>
  /// Saves all data and performs cleanup before application exit
  /// </summary>
  public static void Exit() {
    Logging.Log("Exiting...");
    Logging.Log("Saving Data...");
    Save();

    Logging.Log("Data utility safely closed!");
  }

  /// <summary>
  /// Loads all data from disk, including horses and key bindings
  /// </summary>
  public static void Load() {
    Logging.Log("Loading horses...");
    LoadHorses();
    Logging.Log("Loading Binds...");
    LoadBinds();

    Logging.Log("Data loaded!");
  }

  /// <summary>
  /// Saves all current data to disk, including horses and key bindings
  /// </summary>
  public static void Save() {
    Logging.Log("Saving horses...");
    SaveHorses();
    Logging.Log("Saving Binds...");
    SaveBinds();

    Logging.Log("Data saved!");
  }

  /// <summary>
  /// Ensures required data directories and files exist, creates them if missing
  /// </summary>
  private static void CheckDataFolder() {
    // Create or verify data directory
    if (!Directory.Exists("./Data")) {
      Directory.CreateDirectory("./Data");
      Logging.Log("Data folder created.");
    }
    else
      Logging.Log("Data folder found!");

    // Create or verify horses.json
    if (!File.Exists("./Data/horses.json")) {
      File.Create("./Data/horses.json").Close();
      Logging.Log("Horses file created.");
    }
    else
      Logging.Log("Horses file found!");

    // Create or verify binds.json, initialize if new
    if (!File.Exists("./Data/binds.json")) {
      File.Create("./Data/binds.json").Close();
      Logging.Log("Binds file created.");

      InitBinds();
      Logging.Log("Binds file seeded.");
    }
    else
      Logging.Log("Binds file found!");
  }

  /// <summary>
  /// Sets up default key bindings for application functions
  /// </summary>
  private static void InitBinds() {
    KeyBinds.Add("Add Horse", ConsoleKey.F1);
    KeyBinds.Add("Remove Horse", ConsoleKey.F2);
    KeyBinds.Add("Edit Menu", ConsoleKey.F3);
    KeyBinds.Add("Master List", ConsoleKey.F4);
    KeyBinds.Add("Bulk Reset", ConsoleKey.F5);
    KeyBinds.Add("Exit", ConsoleKey.Escape);

    SaveBinds(false);
  }

  /// <summary>
  /// Maps action names to their corresponding functions and logs the bindings
  /// </summary>
  private static void BindActions() {
    try {
      // Associate each action name with its implementation
      Actions.Add("Add Horse", AddHorseAction);
      Logging.Log($"Bound Add Horse to {KeyBinds["Add Horse"]}");

      Actions.Add("Remove Horse", RemoveHorseAction);
      Logging.Log($"Bound Remove Horse to {KeyBinds["Remove Horse"]}");

      Actions.Add("Edit Menu", EditMenuAction);
      Logging.Log($"Bound Edit Menu to {KeyBinds["Edit Menu"]}");

      Actions.Add("Master List", MasterListAction);
      Logging.Log($"Bound Master List to {KeyBinds["Master List"]}");

      Actions.Add("Bulk Reset", BulkResetAction);
      Logging.Log($"Bound Bulk Reset to {KeyBinds["Bulk Reset"]}");

      Actions.Add("Exit", QuitAction);
      Logging.Log($"Bound Quit to {KeyBinds["Exit"]}");
    }
    catch (Exception e) {
      Logging.Log("Failed to bind actions!");
      DisplayException(e);
    }
  }

  /// <summary>
  /// Loads horse data from JSON file, initializes empty list if file is empty or corrupted
  /// </summary>
  private static void LoadHorses() {
    try {
      string rawString = File.ReadAllText("./Data/horses.json");
      if (string.IsNullOrWhiteSpace(rawString)) {
        Horses = [];
        Logging.Log("No horses found.");
        return;
      }

      Horses = JsonSerializer.Deserialize<List<Horse>>(rawString, JsonOptions) ?? [];
      Logging.Log($"Successfully loaded {Horses.Count} horses!");
    }
    catch (Exception e) {
      Logging.Log("Failed to load horses!");
      DisplayException(e);
      Horses = [];
    }
  }

  /// <summary>
  /// Saves current horse data to JSON file
  /// </summary>
  private static void SaveHorses() {
    try {
      var rawString = JsonSerializer.Serialize(Horses, JsonOptions);
      File.WriteAllText("./Data/horses.json", rawString);

      Logging.Log("Successfully saved horses!");
    }
    catch (Exception e) {
      Logging.Log("Failed to save horses!");
      DisplayException(e);
    }
  }

  /// <summary>
  /// Loads key bindings from JSON file, resets to defaults if file is corrupted
  /// </summary>
  private static void LoadBinds() {
    try {
      var rawString = File.ReadAllText("./Data/binds.json");
      if (string.IsNullOrWhiteSpace(rawString)) {
        KeyBinds = [];
        Logging.Log("No binds found.");
      }

      KeyBinds = JsonSerializer.Deserialize<Dictionary<string, ConsoleKey>>(rawString, JsonOptions) ?? [];
      Logging.Log($"Successfully loaded {KeyBinds.Count} binds!");
    }
    catch (JsonException e) {
      Logging.Log("Failed to load binds!");
      Logging.Log("Binds file corrupted! Resetting binds...");
      InitBinds();
      Logging.Log("Binds reset!");
    }
    catch (Exception e) {
      Logging.Log("Failed to load binds!");
      DisplayException(e);
      Environment.Exit(1);
    }
  }
  
  /// <summary>
  /// Saves current key bindings to JSON file
  /// </summary>
  /// <param name="displayMessage">Whether to log a success message</param>
  private static void SaveBinds(bool displayMessage = true) {
    try {
      var rawString = JsonSerializer.Serialize(KeyBinds, JsonOptions);
      File.WriteAllText("./Data/binds.json", rawString);

      if (displayMessage)
        Logging.Log("Successfully saved binds!");
    }
    catch (Exception e) {
      Logging.Log("Failed to save binds!");
      DisplayException(e);
    }
  }
}