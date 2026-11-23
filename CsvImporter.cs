using System;
using System.Globalization;
using System.IO;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils
{
    public class CsvImporter
    {
        public ImportResult ImportPatients(Clinic clinic, string filePath)
        {
            ImportResult result = new ImportResult();

            if (!File.Exists(filePath))
            {
                result.AddError(0, "Файл не знайдено.");
                return result;
            }

            string[] lines = File.ReadAllLines(filePath);

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    string[] parts = line.Split(',');

                    if (parts.Length != 5)
                    {
                        throw new FormatException(
                            $"Рядок має {parts.Length} полів замість 5.");
                    }

                    string firstName = parts[0].Trim();
                    string lastName = parts[1].Trim();
                    DateTime dateOfBirth = DateTime.ParseExact(
                        parts[2].Trim(),
                        "dd.MM.yyyy",
                        CultureInfo.InvariantCulture);

                    BloodType bloodType = (BloodType)Enum.Parse(
                        typeof(BloodType),
                        parts[3].Trim(),
                        true);

                    string phone = parts[4].Trim();

                    Patient patient = new Patient(
                        firstName,
                        lastName,
                        dateOfBirth,
                        bloodType,
                        phone);

                    clinic.Patients.Add(patient);
                    result.AddSuccess();
                }
                catch (Exception ex)
                {
                    result.AddError(i + 1, ex.Message);
                }
            }

            return result;
        }
    }
}
