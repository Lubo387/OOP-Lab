using System;

namespace ClinicApp.Models
{
    public struct WorkSchedule
    {
        public int Start { get; }
        public int End { get; }

        public WorkSchedule(int start, int end)
        {
            if (start < 0 || start > 23)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(start),
                    "Початок робочого дня повинен бути від 0 до 23.");
            }

            if (end < 1 || end > 24)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(end),
                    "Кінець робочого дня повинен бути від 1 до 24.");
            }

            if (start >= end)
            {
                throw new ArgumentException(
                    "Початок робочого дня повинен бути раніше за кінець.",
                    nameof(start));
            }

            Start = start;
            End = end;
        }

        public int HoursPerDay
        {
            get => End - Start;
        }

        public string Display
        {
            get => Start.ToString("D2") + ":00–" + End.ToString("D2") + ":00";
        }

        public bool IsNow
        {
            get => Contains(DateTime.Now.Hour);
        }

        public bool Contains(int hour)
        {
            return hour >= Start && hour < End;
        }

        public override string ToString()
        {
            return Display + " (" + HoursPerDay + " год)";
        }
    }
}
