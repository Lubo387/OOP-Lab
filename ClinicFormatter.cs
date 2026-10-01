namespace ClinicApp
{
    public static class ClinicFormatter
    {
        public static string FormatBloodType(BloodType bt) => bt switch
        {
            BloodType.APositive => "A+",
            BloodType.ANegative => "A-",
            BloodType.BPositive => "B+",
            BloodType.BNegative => "B-",
            BloodType.ABPositive => "AB+",
            BloodType.ABNegative => "AB-",
            BloodType.OPositive => "O+",
            BloodType.ONegative => "O-",
            _ => "Невідомо"
        };

        public static string FormatSpeciality(Speciality s) => s switch
        {
            Speciality.Cardiology => "Кардіологія",
            Speciality.Neurology => "Неврологія",
            Speciality.Pediatrics => "Педіатрія",
            Speciality.Surgery => "Хірургія",
            Speciality.Orthopedics => "Ортопедія",
            Speciality.Dermatology => "Дерматологія",
            Speciality.General => "Терапевт",
            Speciality.Emergency => "Швидка допомога",
            _ => "Невідомо"
        };

        public static string FormatAge(int age)
        {
            int lastTwo = age % 100;
            int last = age % 10;

            if (lastTwo >= 11 && lastTwo <= 19)
                return age + " років";

            if (last == 1)
                return age + " рік";

            if (last == 2 || last == 3 || last == 4)
                return age + " роки";

            return age + " років";
        }

        public static string FormatPhone(string phone)
        {
            if (phone == null || phone.Length != 10)
                return phone ?? "";

            for (int i = 0; i < phone.Length; i++)
            {
                if (!char.IsDigit(phone[i]))
                    return phone;
            }

            return "(" + phone.Substring(0, 3) + ") "
                + phone.Substring(3, 3) + "-"
                + phone.Substring(6, 4);
        }

        public static BloodType ParseBloodType(string value)
        {
            if (value == null)
                return BloodType.Unknown;

            string text = value.Trim().ToUpperInvariant();

            return text switch
            {
                "A+" or "APOSITIVE" => BloodType.APositive,
                "A-" or "ANEGATIVE" => BloodType.ANegative,
                "B+" or "BPOSITIVE" => BloodType.BPositive,
                "B-" or "BNEGATIVE" => BloodType.BNegative,
                "AB+" or "ABPOSITIVE" => BloodType.ABPositive,
                "AB-" or "ABNEGATIVE" => BloodType.ABNegative,
                "O+" or "OPOSITIVE" => BloodType.OPositive,
                "O-" or "ONEGATIVE" => BloodType.ONegative,
                _ => BloodType.Unknown
            };
        }

        public static Speciality ParseSpeciality(string value)
        {
            if (value == null)
                return Speciality.General;

            string text = value.Trim().ToLowerInvariant();

            return text switch
            {
                "загальна практика" or "терапевт" or "general" => Speciality.General,
                "кардіологія" or "cardiology" => Speciality.Cardiology,
                "неврологія" or "neurology" => Speciality.Neurology,
                "педіатрія" or "pediatrics" => Speciality.Pediatrics,
                "хірургія" or "surgery" => Speciality.Surgery,
                "ортопедія" or "orthopedics" => Speciality.Orthopedics,
                "дерматологія" or "dermatology" => Speciality.Dermatology,
                "невідкладна допомога" or "швидка допомога" or "emergency" => Speciality.Emergency,
                _ => Speciality.General
            };
        }
    }
}
