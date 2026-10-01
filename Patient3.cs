namespace ClinicApp
{
    public class Patient
    {
        private static int _nextId = 1;

        public int Id { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public BloodType BloodType { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FullName => FirstName + " " + LastName;

        public int Age
        {
            get
            {
                int age = DateTime.Today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > DateTime.Today.AddYears(-age))
                    age--;
                return age;
            }
        }

        public bool IsAdult => Age >= 18;

        public Patient() : this("Невідомий", "Пацієнт")
        {
        }

        public Patient(string firstName, string lastName)
            : this(firstName, lastName, DateTime.Today.AddYears(-26), BloodType.Unknown, "0000000000")
        {
        }

        public Patient(string firstName, string lastName, DateTime dateOfBirth, BloodType bloodType, string phone)
        {
            Id = _nextId++;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            BloodType = bloodType;
            Phone = phone;
            Email = "";
        }

        public Patient(string firstName, string lastName, DateTime dateOfBirth, string bloodType, string phone)
            : this(firstName, lastName, dateOfBirth, ClinicFormatter.ParseBloodType(bloodType), phone)
        {
        }

        public string GetAgeCategory()
        {
            return Age < 18 ? "дитина" : Age < 60 ? "дорослий" : "літній";
        }

        public override string ToString()
        {
            return "[" + Id + "] " + FullName
                + " | Вік: " + ClinicFormatter.FormatAge(Age)
                + " (" + GetAgeCategory() + ")"
                + " | Кров: " + ClinicFormatter.FormatBloodType(BloodType)
                + " | Тел: " + ClinicFormatter.FormatPhone(Phone);
        }
    }
}
//13