using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using HorseCalendar;

namespace HorseCalendar;

internal class UserInterface
{
    public static bool IsHotKeysHidden { get; private set; } = false;
    public static bool IsClosing { get; private set; } = false;
    public static void Run()
    {
        Console.CursorVisible = false;
        while (!IsClosing)
        {
            Console.Clear();

            Data.ListBindsMain();

            Console.WriteLine("Welcome to the Hoof Track!".PadLeft((Console.WindowWidth - "Welcome to the Hoof Track!".Length) / 2 + "Welcome to the Hoof Track!".Length));

            Console.WriteLine("\n== Horses over 6 weeks overdue ===");
            Data.OrderHorses(3, true);
            for (int i = 0; i < Data.Horses.Count; i++)
            {
                if (Data.Horses[i].WeeksOverdue > 6)
                {
                    Console.WriteLine($"({Data.Horses[i].ID}) {Data.Horses[i].Name} {Data.Horses[i].WeeksOverdue} weeks");
                }
            }
            CatchKeyMain();
        }
        
    }

    public static void CatchKeyMain()
    {
        ConsoleKey keyPress = Console.ReadKey().Key;
        switch (keyPress)
        {
            case ConsoleKey key when key == Data.KeyBinds["Exit"]:
                IsClosing = true;
                break;

            case ConsoleKey key when key == Data.KeyBinds["HorseManipulation"]:
                HorseManipulation();
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

    public static void HorseManipulation()
    {
        Console.WriteLine("\n=== Horse Manipulation ===");
        Console.WriteLine("Do you want to:");
        Console.WriteLine($"{Data.KeyBinds["AddHorse"]} - Add a horse");
        Console.WriteLine($"{Data.KeyBinds["RemoveHorse"]} - Remove a horse");
        Console.WriteLine($"{Data.KeyBinds["ResetToday"]} - Reset a horse to today");
        Console.WriteLine($"{Data.KeyBinds["ResetCustom"]} - Reset a horse to a custom date");
        Console.WriteLine($"{Data.KeyBinds["EditHorse"]} - Edit a horses data");
        Console.WriteLine($"{Data.KeyBinds["Back"]} - Go back");

        CatchKeyHorseManipulation();
    }

    public static void CatchKeyHorseManipulation()
    {
        ConsoleKey keyPress = Console.ReadKey().Key;

        string? input;
        string? name;
        string? rotInterval;

        switch (keyPress)
        {
            case ConsoleKey key when key == Data.KeyBinds["AddHorse"]:
                Console.WriteLine("Enter the name of the horse");
                Console.CursorVisible = true;
                Console.Write("> ");
                name = Console.ReadLine();
                if (name == null)
                    break;

                Console.WriteLine("Enter the rotation interval of the horse");
                Console.CursorVisible = true;
                Console.Write("> ");
                rotInterval = Console.ReadLine();
                if (rotInterval == null)
                    break;

                Data.Horses.Add(new Horse(name, uint.Parse(rotInterval), DateTime.Now));    
                Console.WriteLine(name + " added");
                break;

            case ConsoleKey key when key == Data.KeyBinds["RemoveHorse"]:
                Console.WriteLine("Enter the ID of the horse you want to remove");
                Console.CursorVisible = true;
                Console.Write("> ");
                input = Console.ReadLine();
                if (input == null)
                    break;
                if (uint.TryParse(input, out uint id1))
                    {
                    Horse? horse = Data.Horses.Find(horse => horse.ID == id1);
                    if (horse != null)
                    {
                        Data.Horses.Remove(horse);
                        Console.WriteLine("Horse removed");
                    } else
                    {
                        Console.WriteLine("Horse not found");
                        Thread.Sleep(1000);
                    }
                } else
                {
                    Console.WriteLine("Invalid ID");
                    Thread.Sleep(1000);
                }
                break;

            case ConsoleKey key when key == Data.KeyBinds["ResetToday"]:
                Console.WriteLine("Enter the ID of the horse you want to reset");
                Console.CursorVisible = true;
                Console.Write("> ");
                input = Console.ReadLine();
                if (input == null)
                    break;
                if (uint.TryParse(input, out uint id2))
                {
                    Horse? horse = Data.Horses.Find(horse => horse.ID == id2);
                    if (horse != null)
                    {
                        horse.Reset(DateTime.Now);
                    } else
                    {
                        Console.WriteLine("Horse not found");
                        Thread.Sleep(1000);
                    }
                } else
                {
                    Console.WriteLine("Invalid ID");
                    Thread.Sleep(1000);
                }
                break;

            case ConsoleKey key when key == Data.KeyBinds["ResetCustom"]:
                Console.WriteLine("Enter the ID of the horse you want to reset");
                Console.CursorVisible = true;
                Console.Write("> ");
                input = Console.ReadLine();
                if (input == null)
                    break;
                if (uint.TryParse(input, out uint id3))
                {
                    Horse? horse = Data.Horses.Find(horse => horse.ID == id3);
                    if (horse != null)
                    {
                        Console.WriteLine("Enter the date you shoed the horse (MM/DD/YYYY)");
                        Console.CursorVisible = true;
                        Console.Write("> ");
                        input = Console.ReadLine();
                        if (input == null)
                            break;
                        horse.Reset(DateTime.Parse(input));
                    }
                    else
                    {
                        Console.WriteLine("Horse not found");
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid ID");
                    Thread.Sleep(1000);
                }
                break;

            case ConsoleKey key when key == Data.KeyBinds["EditHorse"]:
                Console.WriteLine("Enter the ID of the horse you want to edit");
                Console.CursorVisible = true;
                Console.Write("> ");
                input = Console.ReadLine();
                if (input == null)
                    break;
                if (uint.TryParse(input, out uint id4))
                    {
                    Horse? horse = Data.Horses.Find(horse => horse.ID == id4);
                    if (horse != null)
                    {
                        Console.WriteLine("Enter the new name of the horse");
                        Console.CursorVisible = true;
                        Console.Write("> ");
                        name = Console.ReadLine();
                        if (name == null)
                            break;

                        Console.WriteLine("Enter the new rotation interval of the horse");
                        Console.CursorVisible = true;
                        Console.Write("> ");
                        rotInterval = Console.ReadLine();
                        if (rotInterval == null)
                            break;

                        horse.Name = name;
                        horse.RotationInterval = uint.Parse(rotInterval);
                        horse.Update();
                    }
                    else
                    {
                        Console.WriteLine("Horse not found");
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid ID");
                    Thread.Sleep(1000);
                }
                break;

            case ConsoleKey key when key == Data.KeyBinds["Back"]:
                break;

            default:
                break;
        }
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
                Console.WriteLine($"({Data.Horses[i].ID}) {Data.Horses[i].Name} {Data.Horses[i].WeeksOverdue} weeks");
            }

            Console.WriteLine($"\n{Data.KeyBinds["OrderName"]} - Order the horses by name");
            Console.WriteLine($"{Data.KeyBinds["OrderWeeks"]} - Order the horses by weeks since last shoe");
            Console.WriteLine($"{Data.KeyBinds["OrderOverdue"]} - Order the horses by weeks overdue");
            Console.WriteLine($"{Data.KeyBinds["TotalHorses"]} - Prints the total horses in the schedule");
            Console.WriteLine($"{Data.KeyBinds["Back"]} - Go back");
            isRunning = CatchKeyMasterList();
        }
        
    }

    public static bool CatchKeyMasterList()
    {

        ConsoleKey keyPress = Console.ReadKey().Key;
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
                Console.ReadKey();
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
            
            ConsoleKey keyPress = Console.ReadKey().Key;
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

        ConsoleKey keyPress = Console.ReadKey().Key;
        switch (keyPress)
        {
            case ConsoleKey key when key == ConsoleKey.D1:
                Console.WriteLine("Enter the minimum overdue weeks to display");
                Console.CursorVisible = true;
                Console.Write("> ");
                string? input = Console.ReadLine();
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
        Console.CursorVisible = true;
        Console.Write("> ");
        string? input = Console.ReadLine();
        Console.CursorVisible = false;
        if (input == null)
            return;
        if (input.ToLower() == "back")
            return;
        if (int.TryParse(input, out int index))
        {
            Console.WriteLine("Press the new key");
            Console.CursorVisible = false;
            
            ConsoleKeyInfo key = Console.ReadKey();
            var keyValuePair = Data.KeyBinds.ToList()[index];
            string dictKey = keyValuePair.Key;
            Data.KeyBinds[dictKey] = key.Key;
        }
    }
}
