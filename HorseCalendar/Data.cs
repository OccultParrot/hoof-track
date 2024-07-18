using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HorseCalendar
{
    internal static class Data
    {
        public static readonly DateTime LastClosedOpened;


        private static List<Horse> horses = [];
        public static List<Horse> Horses { get { return horses; } }

        public static bool Start()
        {
            Console.WriteLine("Loading...");
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
                Console.WriteLine("Created Data Directory");
            }
            else { Console.WriteLine("Data Directory Exists"); }

            if (!File.Exists("Data/Horses.txt"))
            {
                using StreamWriter writer = new StreamWriter("Data/Horses.txt");
                Console.WriteLine("Created Horses File");
            }
            else { Console.WriteLine("Horses File Exists"); }

            if (!File.Exists("Data/General.txt"))
            {
                using StreamWriter writer = new StreamWriter("Data/General.txt");
                Console.WriteLine("Created General File");
            }
            else { Console.WriteLine("General File Exists"); }

            LoadHorses();
            return true;
        }

        public static void Exit()
        {
            SaveHorses();
            Console.WriteLine("Exiting...");
        }

        private static void LoadHorses()
        {
            string[] lines = File.ReadAllLines("Data/Horses.txt");
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
                    int weekCount = int.Parse(parts[1]);
                    uint rotationInterval = uint.Parse(parts[2]);
                    Horse horse = new Horse(name, rotationInterval, weekCount);
                    try
                    {
                        horses.Add(horse);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error Loading Horse: {e.Message}");
                        continue;
                    }
                    Console.WriteLine($"Loaded Horse: {name}, Index of {horse.Index}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error Reading Horse Data: {e.Message}");
                    continue;
                }
            }
            Console.WriteLine("Horses Loaded");
        }
        private static void SaveHorses()
        {
            using StreamWriter writer = new StreamWriter("Data/Horses.txt");
            foreach (Horse horse in horses)
            {
                writer.WriteLine($"{horse.Name},{horse.WeekCount},{horse.RoationInterval}");
                Console.WriteLine($"Saved Horse: {horse.Name}");
            }
            Console.WriteLine("Horses Saved");
        }
    }
}
