using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    
    class ProgramTV
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public string Title { get; set; }
        public ProgramTV(DateTime start, DateTime end, string title)
        {
            StartTime = start;
            EndTime = end;
            Title = title;
            TimeSpan = new TimeSpan(EndTime.Hour-StartTime.Hour, EndTime.Minute-StartTime.Minute, EndTime.Second - StartTime.Second);
        }
        public ProgramTV() { }
        public void Print()
        {
            Console.WriteLine($"Title: {Title}  Start: {StartTime.TimeOfDay}  End: {EndTime.TimeOfDay}  Lasts: {TimeSpan}");
        }
    }
}
