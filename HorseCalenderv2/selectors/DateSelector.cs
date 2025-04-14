using Spectre.Console;

namespace HorseCalenderv2.selectors;

public static class DateSelector {
  private static void ClearCurrentConsoleLine() {
    var currentLineCursor = Console.CursorTop;
    Console.SetCursorPosition(0, Console.CursorTop);
    Console.Write(new string(' ', Console.WindowWidth));
    Console.SetCursorPosition(0, currentLineCursor);
  }

  public static DateTime DateInput(DateTime? startDate = null) {
    var selected = 0;
    
    if (startDate == null)
      startDate = DateTime.Now;
    
    // MM/DD/YYYY
    var date = new string[3];
    var month = startDate.Value.Month;
    var day = startDate.Value.Day;
    var year = startDate.Value.Year;

    while (true) {
      switch (selected) {
        case 0:
          AnsiConsole.Markup($"[black on white]{month:D2}[/]/{day:D2}/{year:D4}");
          break;
        case 1:
          AnsiConsole.Markup($"{month:D2}/[black on white]{day:D2}[/]/{year:D4}");
          break;
        case 2:
          AnsiConsole.Markup($"{month:D2}/{day:D2}/[black on white]{year:D4}[/]");
          break;
      }

      var key = Console.ReadKey(true);
      switch (key.Key) {
        case ConsoleKey.RightArrow when selected < 2:
          selected++;
          break;
        case ConsoleKey.RightArrow:
          selected = 0;
          break;
        case ConsoleKey.LeftArrow when selected > 0:
          selected--;
          break;
        case ConsoleKey.LeftArrow:
          selected = 2;
          break;
        case ConsoleKey.UpArrow when selected == 0: {
          month++;
          if (month > 12)
            month = 1;
          break;
        }
        case ConsoleKey.UpArrow when selected == 1: {
          day++;
          if (day > DateTime.DaysInMonth(year, month))
            day = 1;
          break;
        }
        case ConsoleKey.UpArrow: {
          if (selected == 2) {
            year++;
          }

          break;
        }
        case ConsoleKey.DownArrow when selected == 0: {
          month--;
          if (month < 1)
            month = 12;
          break;
        }
        case ConsoleKey.DownArrow when selected == 1: {
          day--;
          if (day < 1)
            day = DateTime.DaysInMonth(year, month);
          break;
        }
        case ConsoleKey.DownArrow: {
          if (selected == 2) {
            year--;
          }

          break;
        }
        case ConsoleKey.Enter:
          Console.WriteLine();
          return DateTime.Parse($"{month:D2}/{day:D2}/{year:D4}");
      }

      ClearCurrentConsoleLine();
    }
  }
}