using System;

namespace ClinicApp.Utils
{
    public static class ClinicValidator
    {
        public static void ValidateName(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Значення не може бути порожнім.",
                    fieldName);
            }

            if (value.Length > 50)
            {
                throw new ArgumentException(
                    "Значення не може містити більше 50 символів.",
                    fieldName);
            }
        }

        public static void ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                throw new ArgumentException(
                    "Телефон не може бути порожнім.",
                    nameof(phone));
            }

            if (phone.Length != 10)
            {
                throw new ArgumentException(
                    "Телефон повинен містити рівно 10 символів.",
                    nameof(phone));
            }

            for (int i = 0; i < phone.Length; i++)
            {
                if (!char.IsDigit(phone[i]))
                {
                    throw new ArgumentException(
                        "Телефон повинен містити лише цифри.",
                        nameof(phone));
                }
            }
        }

        public static void ValidateDate(DateTime value, string fieldName)
        {
            if (value.Date > DateTime.Today)
            {
                throw new ArgumentOutOfRangeException(
                    fieldName,
                    "Дата не може бути в майбутньому.");
            }

            if (value.Year < 1900)
            {
                throw new ArgumentOutOfRangeException(
                    fieldName,
                    "Дата не може бути раніше 1900 року.");
            }
        }

        public static void ValidatePositive(int value, string fieldName)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    fieldName,
                    "Значення повинно бути більшим за 0.");
            }
        }
    }
}
