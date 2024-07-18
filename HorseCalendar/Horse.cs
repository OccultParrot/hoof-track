using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseCalendar
{
    internal class Horse(string? name, uint roationInterval, int weekCount = 0)
    {
        public int Index { get; set; }
        public string? Name { get; set; } = name;
        public int WeekCount = weekCount;
        public uint RoationInterval = roationInterval;

        public void ResetToday()
        {
            WeekCount = 0;
        }

        public void ResetDate(DateTime date)
        {
            // TODO: Write reset to date formula
            Console.WriteLine(DateTime.Now);
            return;
        }
    }
}
