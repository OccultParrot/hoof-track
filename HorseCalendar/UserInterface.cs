using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using HorseCalendar;
using System.Reflection.Metadata.Ecma335;
using System.Diagnostics;

namespace HorseCalendar;

internal class UserInterface
{
    public static bool IsHotKeysHidden { get; private set; } = false;
    public static bool IsClosing { get; private set; } = false;
    private readonly static int _popUpDelay = 1000;
    public static void Run()
    {
        // TODO: display number of horses in active rotation
        // TODO: display number of horses overdue

        Console.CursorVisible = false;
        while (!IsClosing)
        {
            string date = DateTime.Now.ToLongDateString().ToUpper() + " (" + DateTime.Now.ToShortDateString() + ")";

            Console.Clear();

            Utils.WriteLineCentered("WELCOME TO HOOF TRACK, FARRIER BUSINESS MANAGEMENT SYSTEMS");

            Utils.Rule();

            Utils.WriteLineCentered(date);
            Utils.WriteLineCentered("HORSES ACTIVE IN ROTATION: [TODO]");
            Utils.WriteLineCentered("HORSES OVERDUE: [TODO]");

            CatchKeyMain();
        }
    }

    public static void CatchKeyMain()
    {
        ConsoleKey keyPress = Console.ReadKey(true).Key;
        switch (keyPress)
        {
            case ConsoleKey key when key == Data.KeyBinds["Exit"]:
                IsClosing = true;
                break;

            case ConsoleKey key when key == Data.KeyBinds["HorseManipulation"]:
                RotationManagementMenu();
                break;

            case ConsoleKey key when key == Data.KeyBinds["MasterList"]:
                MasterList();
                break;

            case ConsoleKey key when key == Data.KeyBinds["HideHotKeys"]:
                IsHotKeysHidden = !IsHotKeysHidden;
                break;

            case ConsoleKey key when key == Data.KeyBinds["DueDateCalculator"]:
                // TODO: Add Due Date Calculator
                break;

            case ConsoleKey key when key == Data.KeyBinds["OpenSettings"]:
                Options();
                break;
        }
    }

    public static void RotationManagementMenu()
    {
        Console.WriteLine("\n=== Shoeing Rotation Menu ===");
        Console.WriteLine("Do you want to:");
        Console.WriteLine("F1 - Add Horse");
        Console.WriteLine("F2 - Reset Horse");
        Console.WriteLine("F3 - Move Horse to Inactive");
        Console.WriteLine("F4 - Edit Horse Data");
        Console.WriteLine("Escape - Go back");

        CatchKeyRotationManagementMenu();
    }

    public static void CatchKeyRotationManagementMenu()
    {
        ConsoleKey keyPress = Console.ReadKey(true).Key;

        switch (keyPress)
        {
            case ConsoleKey.F1:
                AddHorse();
                break;
            case ConsoleKey.F2:
                ResetHorse();
                break;
        }
    }

    private static void AddHorse()
    {
        int step = 1;
        string? name = "";
        bool isTrim = false;
        while (true)
        {
            if (step == 1)
            {
                Console.WriteLine("Enter the name of the horse");
                name = Utils.AutoCompleteInput([], true);
                if (name == null)
                    return;
                step++;
            }
            else if (step == 2)
            {
                Console.WriteLine("Is the horse due for a trim? (y/n)");
                bool backOneStep = false;
                while (true)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Escape)
                    {
                        // This part is done in AutoCompleteInput usually
                        Utils.ClearCurrentConsoleLine();
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        Utils.ClearCurrentConsoleLine();
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        Utils.ClearCurrentConsoleLine(); 
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        Utils.ClearCurrentConsoleLine();

                        step--;
                        backOneStep = true;
                        break;
                    }
                    else if (key.Key == ConsoleKey.Y)
                    {
                        Console.WriteLine("Yes");
                        isTrim = true;
                        break;
                    }
                    else if (key.Key == ConsoleKey.N)
                    {
                        Console.WriteLine("No");
                        isTrim = false;
                        break;
                    }
                }
                if (backOneStep)
                    continue;
                step++;
            }
            else if (step == 3)
            {
                Console.WriteLine("Enter the rotation interval in weeks");
                string? rotationInterval = Utils.AutoCompleteInput([], true);
                if (rotationInterval == null)
                {
                    step--;
                    continue;
                }
                step++;
            }
            else if (step == 4)
            {
                Console.WriteLine("Press \"ENTER\" to  set the last shod date to today, or press \"D\" to set to custom date");
                ConsoleKey keyPress = Console.ReadKey(true).Key;
                if (keyPress == ConsoleKey.Enter)
                {
                    Data.AddHorse(name, 0, DateTime.Now, isTrim);
                    break;
                } else if (keyPress == ConsoleKey.D)
                {
                    // TODO: Add custom date
                    Console.WriteLine("TODO lol");
                } else if (keyPress == ConsoleKey.Escape)
                {
                    step--;
                    continue;
                }
            }
            else
            {
                break;
            }
        }
        Data.SaveHorses();
    }

    private static void ResetHorse()
    {
        Console.WriteLine("");
        string choice = Utils.TabMenu(["Reset to Today", "Reset to Custom Date"]);
        if (choice == "Reset to Today")
        {
            Console.WriteLine("Enter the name of the horse you want to reset");
            string? name = Utils.AutoCompleteInput([.. Data.HorseNames], true);
            
            if (name == null)
                return;

            Data.ResetHorse(name, DateTime.Now);
        }
        else if (choice == "Reset to Custom Date")
        {
            Console.WriteLine("Enter the name of the horse you want to reset");
            string? name = Utils.AutoCompleteInput([.. Data.HorseNames], true);
            
            if (name == null) 
                return;

            Console.WriteLine("Enter the date you want to reset the horse to");
            DateTime resetDate = Utils.DateInput();

            Data.ResetHorse(name, resetDate);
        }

        Data.SaveHorses();
    }
    public static void MasterList()
    {
        bool isRunning = true;
        while (isRunning)
        {
            Console.Clear();
            Console.WriteLine("\n=== Master List ===");
            Data.OrderHorses(1, false);
            for (int i = 0; i < Data.Horses.Count; i++)
            {
                Console.WriteLine($"({Data.Horses[i].ID}) {Data.Horses[i].Name} {Data.Horses[i].WeeksSinceLastShoe} weeks - {Data.Horses[i].LastShoeDate.Date.ToShortDateString()}");
            }

            Console.WriteLine($"\n{Data.KeyBinds["OrderName"]} - Order the horses alphabetically");
            Console.WriteLine($"{Data.KeyBinds["OrderWeeks"]} - Order the horses by weeks since last shoe");
            Console.WriteLine($"{Data.KeyBinds["OrderOverdue"]} - Order the horses by weeks overdue");
            Console.WriteLine($"{Data.KeyBinds["TotalHorses"]} - Prints the total horses in the schedule");
            Console.WriteLine($"{Data.KeyBinds["Back"]} - Go back");
            isRunning = CatchKeyMasterList();
        }
    }

    public static bool CatchKeyMasterList()
    {
        ConsoleKey keyPress = Console.ReadKey(true).Key;
        switch (keyPress)
        {
            case ConsoleKey key when key == Data.KeyBinds["OrderName"]:
                Data.OrderHorses(1, false);
                return true;

            case ConsoleKey key when key == Data.KeyBinds["OrderWeeks"]:
                Data.OrderHorses(2, false);
                return true;

            case ConsoleKey key when key == Data.KeyBinds["OrderOverdue"]:
                Data.OrderHorses(3, false);
                return true;

            case ConsoleKey key when key == Data.KeyBinds["TotalHorses"]:
                Console.WriteLine($"Total Horses: {Data.Horses.Count}");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey(true);
                return true;
            case ConsoleKey key when key == Data.KeyBinds["Back"]:
                return false;

            default:
                return true;
        }
    }

    public static void Options()
    {
        Console.CursorVisible = false;
        bool isRunning = true;
        while (isRunning)
        {
            Console.Clear();
            Console.WriteLine("\n=== Options ===");

            Console.WriteLine("1 - General Settings");
            Console.WriteLine("2 - Key Binds");
            Console.WriteLine("\nEscape - Go back");

            ConsoleKey keyPress = Console.ReadKey(true).Key;
            switch (keyPress)
            {
                case ConsoleKey key when key == ConsoleKey.D1:
                    isRunning = GeneralSettings();
                    break;

                case ConsoleKey key when key == ConsoleKey.D2:
                    KeyBinds();
                    break;

                case ConsoleKey key when key == ConsoleKey.Escape:
                    isRunning = false;
                    break;
                default:
                    break;
            }
        }
    }

    private static bool GeneralSettings()
    {
        Console.Clear();
        Console.WriteLine("\n=== General Settings ===");
        Console.WriteLine("1 - Main menu minimum overdue weeks to display");
        Console.WriteLine("\nEscape - Go back");

        ConsoleKey keyPress = Console.ReadKey(true).Key;
        switch (keyPress)
        {
            case ConsoleKey key when key == ConsoleKey.D1:
                Console.WriteLine("Enter the minimum overdue weeks to display");
                ToggleCursorVisibility(true);
                Console.Write("> ");
                string? input = Console.ReadLine();
                ToggleCursorVisibility(false);
                if (input == null)
                    break;
                if (int.TryParse(input, out int weeks))
                {
                    Data.minimumOverDueMainMenu = weeks;
                }
                break;

            case ConsoleKey key when key == ConsoleKey.Escape:
                return false;

            default:
                break;
        }
        return true;
    }

    private static void KeyBinds()
    {
        Console.Clear();
        Console.WriteLine("\n=== Key Binds ===");
        for (int i = 0; i < Data.KeyBinds.Count; i++)
        {
            Console.WriteLine($"{i} - {Data.KeyBinds.Keys.ElementAt(i)}: {Data.KeyBinds.Values.ElementAt(i)}");
        }
        Console.WriteLine("Type \"back\" to leave this page");

        Console.WriteLine("Enter the number of the key you want to change or type back to exit this page");
        ToggleCursorVisibility(true);
        Console.Write("> ");
        string? input = Console.ReadLine();
        ToggleCursorVisibility(false);
        if (input == null)
            return;
        if (input.ToLower() == "back")
            return;
        if (int.TryParse(input, out int index))
        {
            Console.WriteLine("Press the new key");
            Console.CursorVisible = false;

            ConsoleKeyInfo key = Console.ReadKey(true);
            var keyValuePair = Data.KeyBinds.ToList()[index];
            string dictKey = keyValuePair.Key;
            Data.KeyBinds[dictKey] = key.Key;
        }
        Data.SaveBinds();
    }

    private static void ToggleCursorVisibility(bool isVisible)
    {
        Console.CursorVisible = isVisible;
    }
}
