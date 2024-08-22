using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseCalendar;

public static class Utils
{
    /// <summary>
    /// Writes the text to the console centered
    /// </summary>
    /// <param name="text"> The text you want displayed </param>
    public static void WriteLineCentered(string text)
    {
        Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
        Console.WriteLine(text);
    }

    /// <summary>
    /// Draws a horizontal rule to the console using a character of choice
    /// </summary>
    /// <param name="character"> The character you want the rule to be composed of </param>
    public static void Rule(char character = '-')
    {
        Console.WriteLine(new string(character, Console.WindowWidth));
    }

    /// <summary>
    /// Displays a menu of options to the user and returns the selected option
    /// </summary>
    /// <param name="options">An array of possible options for the user to select</param>
    /// <returns> The selected option</returns>
    public static string TabMenu(string[] options)
    {
        int selected = 0;
        while (true)
        {
            WriteLineCentered("Select an Option:");
            Rule();
            for (int i = 0; i < options.Length; i++)
            {
                if (i == selected)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    WriteLineCentered(options[i]);
                    Console.ResetColor();
                }
                else
                {
                    WriteLineCentered(options[i]);
                }
            }
            Rule();
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.UpArrow)
            {
                selected--;
                if (selected < 0)
                {
                    selected = options.Length - 1;
                }
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                selected++;
                if (selected >= options.Length)
                {
                    selected = 0;
                }
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                return options[selected];
            }
            for (int i = 0; i < options.Length + 3; i++)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                ClearCurrentConsoleLine();
            }
        }
    }

    /// <summary>
    /// Allows the user to input text and auto-complete from a list of options
    /// </summary>
    /// <param name="options">The list of auto complete choices</param>
    /// <param name="isChildInput">Whether or not there was an input before this one</param>
    /// <returns>The input string</returns>
    public static string? AutoCompleteInput(string[] options, bool isChildInput = false)
    {
        Console.CursorVisible = true;
        StringBuilder input = new();

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);

            // Enter
            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                Console.CursorVisible = false;
                return input.ToString();
            }
            // Escape
            else if (key.Key == ConsoleKey.Escape)
            {
                ClearCurrentConsoleLine();
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                ClearCurrentConsoleLine();
                if (isChildInput)
                {
                    // Clears the parent input so that it can display again but not be stacked
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    ClearCurrentConsoleLine();
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    ClearCurrentConsoleLine();
                }
                Console.CursorVisible = false;
                return null;
            }
            // Backspace
            else if (key.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input.Remove(input.Length - 1, 1);
                Console.Write("\b \b");
            }
            // Tab
            else if (key.Key == ConsoleKey.Tab)
            {
                string? match = options.FirstOrDefault(o => o.StartsWith(input.ToString(), StringComparison.OrdinalIgnoreCase));
                if (match != null)
                {
                    ClearCurrentConsoleLine();
                    input.Clear();
                    input.Append(match);
                    Console.Write(match);
                }
            }
            // Any other key
            else if (!char.IsControl(key.KeyChar))
            {
                input.Append(key.KeyChar);
                Console.Write(key.KeyChar);
            }
        }
        
    }
    
    /// <summary>
    /// Clears the current line in the console
    /// </summary>
    public static void ClearCurrentConsoleLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor);
    }

    /// <summary>
    /// Prompts the user to select a date using arrow keys and enter
    /// </summary>
    /// <returns>The date selected by the user</returns>
    public static DateTime DateInput()
    {
        int selected = 0;
        // MM/DD/YYYY
        string[] date = new string[3];
        int month = DateTime.Now.Month;
        int day = DateTime.Now.Day;
        int year = DateTime.Now.Year;

        while (true)
        {
            if (selected == 0)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write($"{month:D2}");
                Console.ResetColor();
                Console.Write("/");
                Console.Write($"{day:D2}");
                Console.Write("/");
                Console.Write($"{year:D4}");
            }
            else if (selected == 1)
            {
                Console.Write($"{month:D2}");
                Console.Write("/");
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write($"{day:D2}");
                Console.ResetColor();
                Console.Write("/");
                Console.Write($"{year:D4}");
            }
            else if (selected == 2)
            {
                Console.Write($"{month:D2}");
                Console.Write("/");
                Console.Write($"{day:D2}");
                Console.Write("/");
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write($"{year:D4}");
                Console.ResetColor();
            }
            
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.RightArrow)
            {
                if (selected < 2)
                    selected++;
                else
                    selected = 0;
            }
            else if (key.Key == ConsoleKey.LeftArrow)
            {
                if (selected > 0)
                    selected--;
                else
                    selected = 2;
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                if (selected == 0)
                {
                    month++;
                    if (month > 12)
                        month = 1;
                }
                else if (selected == 1)
                {
                    day++;
                    if (day > DateTime.DaysInMonth(year, month))
                        day = 1;
                }
                else if (selected == 2)
                {
                    year++;
                }
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                if (selected == 0)
                {
                    month--;
                    if (month < 1)
                        month = 12;
                }
                else if (selected == 1)
                {
                    day--;
                    if (day < 1)
                        day = DateTime.DaysInMonth(year, month);
                }
                else if (selected == 2)
                {
                    year--;
                }
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                return DateTime.Parse($"{month:D2}/{day:D2}/{year:D4}");
            }

            Utils.ClearCurrentConsoleLine();
        }
    }
}
