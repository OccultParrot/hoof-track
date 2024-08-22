﻿namespace HorseCalendar;

internal class Horse
{
    public static uint _idCounter { get; private set; } = 0;
    public uint ID { get; private set; }
    public string Name { get; set; }
    public int WeeksSinceLastShoe => (int)((DateTime.Now - LastShoeDate).TotalDays / 7);
    public int WeeksOverdue => Math.Max(0, WeeksSinceLastShoe - (int)RotationInterval);
    public bool IsTrim { get; set; }
    public uint RotationInterval { get; set; }
    public DateTime LastShoeDate { get; set; }

    

    public Horse(string name, uint rotationInterval, DateTime lastShoeDate, bool isTrim)
    {
        Name = name;
        RotationInterval = rotationInterval;
        LastShoeDate = lastShoeDate;
        ID = ++_idCounter;
        Update();
        IsTrim = isTrim;

    }

    public void Update()
    {

    }

    /// <summary>
    /// Resets the horse's last day it was shoed and weeks since last shoe
    /// </summary>
    /// <param name="date">The date to set the last date the horse was shod to</param>
    public void Reset(DateTime date)
    {
        LastShoeDate = date;
        Update();
    }
}
