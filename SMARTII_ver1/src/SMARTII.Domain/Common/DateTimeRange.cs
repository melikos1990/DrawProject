using System;

namespace SMARTII.Domain.Common
{
    public struct DateTimeRange
    {
        public DateTimeRange(DateTime start, DateTime end)
        {
            this.Start = start;
            this.End = end;
        }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public override string ToString()
        {
            return $"{Start.ToString("yyyy/MM/dd HH:mm:ss")} - {End.ToString("yyyy/MM/dd HH:mm:ss")}";
        }

        public string ToString(string format = "yyyy/MM/dd HH:mm:ss")
        {
            return $"{Start.ToString(format)} - {End.ToString(format)}";
        }
    }
}
