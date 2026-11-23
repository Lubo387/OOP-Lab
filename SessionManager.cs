using System;
using System.Globalization;
using System.IO;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils
{
    public class SessionManager
    {
        private readonly string _sessionPath;

        public SessionManager(string sessionPath = "session.dat")
        {
            _sessionPath = sessionPath;
        }

        public bool Exists()
        {
            return File.Exists(_sessionPath);
        }

        public void Save(Clinic clinic)
        {
            using (StreamWriter writer = new StreamWriter(
                _sessionPath,
                false,
                Encoding.UTF8))
            {
                writer.WriteLine("[PATIENTS]");

                Patient[] patients = clinic.Patients.GetAll();

                foreach (Patient patient in patients)
                {
                    writer.WriteLine(
                        $"{patient.FirstName},{patient.LastName}," +
                        $"{patient.DateOfBirth:dd.MM.yyyy}," +
                        $"{patient.BloodType},{patient.Phone}");
                }

                writer.WriteLine("[END]");
            }
        }

        public int Load(Clinic clinic)
        {
            if (!File.Exists(_sessionPath))
                return 0;

            string[] lines = File.ReadAllLines(_sessionPath, Encoding.UTF8);
            bool readingPatients = false;
            int loaded = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                if (line.Length == 0)
                    continue;

                if (line.StartsWith("["))
                {
                    if (line == "[PATIENTS]")
                        readingPatients = true;
                    else if (line == "[END]")
                        readingPatients = false;
                    else
                        readingPatients = false;

                    continue;
                }

                if (!readingPatients)
                    continue;

                try
                {
                    string[] parts = line.Split(',');

                    if (parts.Length != 5)
                        continue;

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

                    if (AlreadyExists(
                        clinic,
                        firstName,
                        lastName,
                        dateOfBirth))
                    {
                        continue;
                    }

                    Patient patient = new Patient(
                        firstName,
                        lastName,
                        dateOfBirth,
                        bloodType,
                        phone);

                    clinic.Patients.Add(patient);
                    loaded++;
                }
                catch
                {

                }
            }

            return loaded;
        }

        private bool AlreadyExists(
            Clinic clinic,
            string firstName,
            string lastName,
            DateTime dateOfBirth)
        {
            Patient[] patients = clinic.Patients.GetAll();

            foreach (Patient patient in patients)
            {
                if (string.Equals(
                        patient.FirstName,
                        firstName,
                        StringComparison.OrdinalIgnoreCase)
                    && string.Equals(
                        patient.LastName,
                        lastName,
                        StringComparison.OrdinalIgnoreCase)
                    && patient.DateOfBirth.Date == dateOfBirth.Date)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
