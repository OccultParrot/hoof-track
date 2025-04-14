namespace HorseCalenderv2.utilities;

using Spectre.Console;

/// <summary>
/// Provides color-coded console output utilities using Spectre.Console
/// Supports success, error, warning, info, and custom colored messages
/// </summary>
public static class Print {
  /// <summary>Default color for success messages</summary>
  public static string SuccessColor { get; set; } = "green";
  /// <summary>Default color for error messages</summary>
  public static string ErrorColor { get; set; } = "red";
  /// <summary>Default color for warning messages</summary>
  public static string WarningColor { get; set; } = "yellow";
  /// <summary>Default color for info messages</summary>
  public static string InfoColor { get; set; } = "blue";
  
  /// <summary>
  /// Formats the message to the success color and prints it.
  /// *Best Practice: End with exclamation mark (!)*
  /// </summary>
  /// <param name="message">The message to send</param>
  public static void Success(string message) {
    AnsiConsole.MarkupLine($"[{SuccessColor}]{message}[/]");
  }

  /// <summary>
  /// Formats the message to the error color and prints it.
  /// *Best Practice: End with exclamation mark (!)*
  /// </summary>
  /// <param name="message">The message to send</param>
  public static void Error(string message) {
    AnsiConsole.MarkupLine($"[{ErrorColor}]{message}[/]");
  }

  /// <summary>
  /// Formats the message to the warning color and prints it
  /// </summary>
  /// <param name="message">The message to send</param>
  public static void Warning(string message) {
    AnsiConsole.MarkupLine($"[{WarningColor}]{message}[/]");
  }

  /// <summary>
  /// Formats the message to the info color and prints it
  /// </summary>
  /// <param name="message">The message to send</param>
  public static void Info(string message) {
    AnsiConsole.MarkupLine($"[{InfoColor}]{message}[/]");
  }

  /// <summary>
  /// Formats the message with a custom color and prints it
  /// </summary>
  /// <param name="message">The message to send</param>
  /// <param name="color">Valid Spectre.Console color name</param>
  public static void Custom(string message, string color) {
    AnsiConsole.MarkupLine($"[{color}]{message}[/]");
  }
  
  /// <summary>
  /// Displays and logs an exception
  /// </summary>
  /// <param name="e">Exception to display and log</param>
  public static void DisplayException(Exception e) {
    AnsiConsole.WriteException(e);
    Logging.Log(e.Message);
  }
}