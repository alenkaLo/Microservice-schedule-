﻿namespace TimeTable.Models.Entity
{
    public class Period
    {
        public DateOnly StartDate { get; set; } 
        public DateOnly EndDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
