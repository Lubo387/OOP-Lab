using System;
using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Utils;

namespace ClinicApp.Models
{
    public class Doctor : ISchedulable
    {
        private static int _nextId = 1;

        private string _firstName = "";
        private string _lastName = "";
        private string _licenseNumber = "";
        private string _phone = "";

        public int Id { get; }

        public string FirstName
        {
            get => _firstName;
            set
            {
                ClinicValidator.ValidateName(value, nameof(FirstName));
                _firstName = value;
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                ClinicValidator.ValidateName(value, nameof(LastName));
                _lastName = value;
            }
        }

        public Speciality Speciality { get; set; }

        public string LicenseNumber
        {
            get => _licenseNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(
                        "Номер ліцензії не може бути порожнім.",
                        nameof(LicenseNumber));
                }

                _licenseNumber = value;
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                ClinicValidator.ValidatePhone(value);
                _phone = value;
            }
        }

        public WorkSchedule Schedule { get; set; }

        public string FullName => FirstName + " " + LastName;
        public int WorkingHoursPerDay => Schedule.HoursPerDay;
        public string WorkSchedule => Schedule.Display;
        public bool IsAvailableNow => Schedule.IsNow;

        public Doctor() : this("Невідомий", "Лікар", Speciality.General)
        {
        }

        public Doctor(string firstName, string lastName, Speciality speciality)
            : this(firstName, lastName, speciality, "LIC-000", "0000000000")
        {
        }

        public Doctor(string firstName, string lastName, string speciality)
            : this(firstName, lastName, ClinicFormatter.ParseSpeciality(speciality))
        {
        }

        public Doctor(
            string firstName,
            string lastName,
            string speciality,
            string licenseNumber,
            string phone)
            : this(
                firstName,
                lastName,
                ClinicFormatter.ParseSpeciality(speciality),
                licenseNumber,
                phone)
        {
        }

        public Doctor(
            string firstName,
            string lastName,
            Speciality speciality,
            string licenseNumber,
            string phone)
        {
            FirstName = firstName;
            LastName = lastName;
            Speciality = speciality;
            LicenseNumber = licenseNumber;
            Phone = phone;
            Schedule = new WorkSchedule(8, 17);
            Id = _nextId++;
        }

        public bool CanAcceptAt(int hour)
        {
            return Schedule.Contains(hour);
        }

        public bool CanSchedule(DateTime at)
        {
            return Schedule.Contains(at.Hour);
        }

        public DateTime[] GetAvailableSlots(DateTime date, int slotCount)
        {
            ClinicValidator.ValidatePositive(slotCount, nameof(slotCount));

            int count = Math.Min(slotCount, Schedule.HoursPerDay);
            DateTime[] slots = new DateTime[count];
            DateTime dayStart = date.Date.AddHours(Schedule.Start);

            for (int i = 0; i < count; i++)
                slots[i] = dayStart.AddHours(i);

            return slots;
        }

        public override string ToString()
        {
            string status = IsAvailableNow
                ? "доступний зараз"
                : "не в робочий час";

            return "[" + Id + "] " + FullName
                + " | " + ClinicFormatter.FormatSpeciality(Speciality)
                + " | " + LicenseNumber
                + " | Тел: " + ClinicFormatter.FormatPhone(Phone)
                + " | " + Schedule
                + " | " + status;
        }
    }
}
