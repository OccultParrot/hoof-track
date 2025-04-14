namespace HorseCalenderv2.utilities;

/// <summary>
/// Provides application-wide logging functionality with debug output support
/// </summary>
public static class Logging {
  private static string LogPath { get; set; } = "";
  private static bool IsDebug { get; set; } = false;
  
  /// <summary>
  /// Logs a message to file and optionally console if in debug mode
  /// </summary>
  /// <param name="message">Message to log</param>
  public static void Log(string message) {
    if (IsDebug)
      Console.WriteLine($"[{DateTime.Now:HH-mm-ss}] {message}");
    File.AppendAllText(LogPath, $"[{DateTime.Now:HH-mm-ss}] {message}\n");
  }

  /// <summary>
  /// Initializes the logging system
  /// </summary>
  /// <param name="isDebug">Enables console output if true</param>
  public static void Init(bool isDebug) {
    LogPath = $"./logs/log-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt";
    IsDebug = isDebug;
    
    if (!Directory.Exists("./logs"))
      Directory.CreateDirectory("./logs");
    
    File.Create(LogPath).Close();
    
    Log("Logging started");
  }
}