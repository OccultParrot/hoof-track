using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HorseCalendar;

internal static class Data
{
    public static List<Horse> Horses = [];
    public static Dictionary<string, ConsoleKey> KeyBinds = [];
    public static int minimumOverDueMainMenu = 6;

    public static bool Start()
    {
        Console.WriteLine("Loading...");
        if (!Directory.Exists("./Data"))
        {
            Directory.CreateDirectory("./Data");
            Console.WriteLine("Created Data Directory");
        }
        else { Console.WriteLine("Data Directory Exists"); }

        if (!File.Exists("./Data/Horses.txt"))
        {
            using StreamWriter writer = new("./Data/Horses.txt");
            Console.WriteLine("Created Horses File");
        }
        else { Console.WriteLine("Horses File Exists"); }

        if (!File.Exists("./Data/Binds.txt"))
        {
            using StreamWriter writer = new("./Data/Binds.txt");
            Console.WriteLine("Created Binds File");
        }
        else { Console.WriteLine("General File Exists"); }

        LoadHorses();
        LoadBinds();
        return true;
    }

    public static void Exit()
    {
        Console.Clear();
        SaveHorses();
        SaveBinds();
        Console.WriteLine("\nExiting...");
    }

    private static void LoadHorses()
    {
        using StreamReader reader = new("./Data/Horses.txt");
        string[] lines = reader.ReadToEnd().Split('\n');
        // If there is no horses in the file, then don't bother trying to load them
        if (lines.Length == 0)
        {
            Console.WriteLine("No Horses Found");
            return;
        }

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            try
            {
                // Horse Format: ID,Name,WeekCount,RotationInterval
                string[] parts = line.Split(',');
                string name = parts[0];
                DateTime lastShoe = DateTime.Parse(parts[1]);
                uint rotationInterval = uint.Parse(parts[2]);
                Horse horse = new(name, rotationInterval, lastShoe);

                try
                {
                    Horses.Add(horse);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error Loading Horse: {e.Message}");
                    continue;
                }

                Console.WriteLine($"Loaded Horse: {name}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error Reading Horse Data: {e.Message}");
                continue;
            }
        }
        Console.WriteLine("Horses Loaded");
        reader.Close();
    }

    private static void SaveHorses()
    {
        using StreamWriter writer = new("./Data/Horses.txt");
        foreach (Horse horse in Horses)
        {
            writer.WriteLine($"{horse.Name},{horse.LastShoeDate},{horse.RotationInterval}");
            Console.WriteLine($"Saved Horse: {horse.Name}");
        }
        Console.WriteLine("Horses Saved");
        writer.Close();
    }

    public static void AddHorse(string name, uint rotationInterval, DateTime lastShoe)
    {
        Horse horse = new(name, rotationInterval, lastShoe);
        Horses.Add(horse);
        Console.WriteLine($"Added Horse: {name}");
        UpdateHorses();
    }

    public static void RemoveHorse(string name)
    {
        Horse? horse = Horses.FirstOrDefault(h => h.Name == name);
        if (horse is null)
        {
            Console.WriteLine($"Horse Not Found: {name}");
            UpdateHorses();
            return;
        }

        Horses.Remove(horse);
        Console.WriteLine($"Removed Horse: {name}");
        UpdateHorses();
    }

    public static void UpdateHorses()
    {
        foreach (Horse horse in Horses)
        {
            horse.Update();
        }
    }

    /// <summary>
    /// Orders the Horses list, by a given style. 1 = Name, 2 = Weeks Since Last Shoe, 3 = Weeks Overdue, and an optional descending parameter
    /// </summary>
    /// <param name="style"></param>
    /// <param name="descending"></param>
    public static void OrderHorses(int style, bool descending = false)
    {
        UpdateHorses();
        // Force update of all horses before sorting
        foreach (var horse in Horses)
        {
            // This will trigger the calculation of WeeksSinceLastShoe and WeeksOverdue
            _ = horse.WeeksSinceLastShoe;
            _ = horse.WeeksOverdue;
        }

        switch (style)
        {
            case 1:
                Horses = descending
                    ? [.. Horses.OrderByDescending(h => h.Name)]
                    : [.. Horses.OrderBy(h => h.Name)];
                break;
            case 2:
                Horses = descending
                    ? [.. Horses.OrderByDescending(h => h.WeeksSinceLastShoe)]
                    : [.. Horses.OrderBy(h => h.WeeksSinceLastShoe)];
                break;
            case 3:
                Horses = descending
                    ? [.. Horses.OrderByDescending(h => h.WeeksOverdue)]
                    : [.. Horses.OrderBy(h => h.WeeksOverdue)];
                break;
        }

        // If you need to perform any additional updates after sorting
        UpdateHorses();
    }


    public static void ListBindsMain()
    {
        if (UserInterface.IsHotKeysHidden)
        {
            return;
        }
        Console.WriteLine($"Horse Data Menu ({KeyBinds["HorseManipulation"]}), Master List ({KeyBinds["MasterList"]}), Due Date Calculator ({KeyBinds["DueDateCalculator"]}), Settings ({KeyBinds["OpenSettings"]}), Exit ({KeyBinds["Exit"]}), Hide ({KeyBinds["HideHotKeys"]})");
    }
    public static void LoadBinds()
    {
        using StreamReader reader = new("./Data/Binds.txt");
        string[] lines = reader.ReadToEnd().Split('\n');
        reader.Close(); // Close the reader before opening the writer

        Console.WriteLine(lines.Length);
        if (lines.Length == 1)
        {
            Console.WriteLine("Writing Default Keybinds");
            using StreamWriter writer = new("./Data/Binds.txt");
            // Horse Manipulation
            writer.WriteLine("HorseManipulation,F1");

            writer.WriteLine("AddHorse,F1");
            writer.WriteLine("RemoveHorse,F2");
            writer.WriteLine("ResetToday,F3");
            writer.WriteLine("ResetCustom,F4");
            writer.WriteLine("EditHorse,F5");

            // Master List
            writer.WriteLine("MasterList,F2");

            writer.WriteLine("OrderName,F1");
            writer.WriteLine("OrderWeeks,F2");
            writer.WriteLine("OrderOverdue,F3");
            writer.WriteLine("TotalHorses,F4");

            // General
            writer.WriteLine("HideHotKeys,H");

            writer.WriteLine("DueDateCalculator,F3");

            writer.WriteLine("OpenSettings,F4");

            writer.WriteLine("Exit,Escape");

            writer.WriteLine("Back,Escape");
            writer.Close();
            LoadBinds();
        }
        else
        {
            Console.WriteLine("Binds File Exists");
        }

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            try
            {
                // Bind Format: Command,Key
                string[] parts = line.Split(',');
                string command = parts[0];
                ConsoleKey key = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), parts[1]);
                KeyBinds.Add(command, key);
                Console.WriteLine($"Loaded Bind: {command}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error Reading Binds Data: {e.Message}");
                continue;
            }
        }
        Console.WriteLine("Horses Loaded");
    }

    public static void SaveBinds()
    {
        using StreamWriter writer = new("./Data/Binds.txt");
        foreach (KeyValuePair<string, ConsoleKey> bind in KeyBinds)
        {
            writer.WriteLine($"{bind.Key},{bind.Value}");
            Console.WriteLine($"Saved Bind: {bind.Key}");
        }
        Console.WriteLine("Binds Saved");
        writer.Close();
    }
    public static void SetBind()
    {
        //TODO: Set a keybind
    }
}