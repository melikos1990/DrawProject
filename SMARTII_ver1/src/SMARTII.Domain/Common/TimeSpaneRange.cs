using System;

namespace SMARTII.Domain.Common
{
    public struct TimeSpanRange
    {

        public TimeSpanRange(string start, string end)
        {
            this.Start = TimeSpan.Parse(start);
            this.End = TimeSpan.Parse(end);
        }

        public TimeSpanRange(TimeSpan start, TimeSpan end)
        {
            this.Start = start;
            this.End = end;
        }

        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }

        public override string ToString()
        {
            return $"{Start.ToString("HH:mm:ss")} - {End.ToString("HH:mm:ss")}";
        }

        public string ToString(string format = "HH:mm:ss")
        {
            return $"{Start.ToString(format)} - {End.ToString(format)}";
        }
    }
}
