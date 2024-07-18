using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseCalendar
{
    internal static class UserInterface
    {
        public static void Run()
        {
            bool exitFlag = false;
            while (!exitFlag)
            {
                Console.Clear();
                Console.WriteLine("Welcome to Hoof Track Farrier Scheduling!");
                Console.WriteLine("-----------------------------------------");
                for (int i = 0; i < Data.Horses.Count; i++)
                {
                    Console.WriteLine($"{i}. {Data.Horses[i].Name} - {Data.Horses[i].WeekCount}");
                }

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.F1:
                        ResetHorseRotation();
                        break;
                    case ConsoleKey.F2:
                        Console.WriteLine("Master List (F2)");
                        break;
                    case ConsoleKey.Escape:
                        Console.Write("S");
                        exitFlag = true;
                        break;
                }
            }
        }
        private static void ResetHorseRotation()
        {
            Console.WriteLine("Enter Index of Horse to Reset");
            Console.Write(">");
            string? index = Console.ReadLine();

            for (int i = 0; i < Data.Horses.Count; i++)
            {
                if (i.ToString() == index)
                {
                    Data.Horses[i].WeekCount = 0;
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid Index");
                }
            }
        }
    }
}
