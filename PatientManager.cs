using System;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Managers
{
    public class PatientManager
    {
        private readonly List<Patient> _patients = new List<Patient>();

        public int Count => _patients.Count;

        public Patient this[int index]
        {
            get
            {
                if (index < 0 || index >= _patients.Count)
                    return null!;

                return _patients[index];
            }
        }

        public bool TryFindById(int id, out Patient? patient)
        {
            patient = FindById(id);
            return patient != null;
        }

        public Patient[] FindByBloodType(BloodType bloodType)
        {
            List<Patient> result = new List<Patient>();

            for (int i = 0; i < _patients.Count; i++)
            {
                if (_patients[i].BloodType == bloodType)
                    result.Add(_patients[i]);
            }

            return result.ToArray();
        }

        public void Add(Patient patient)
        {
            _patients.Add(patient);

            Console.WriteLine(
                "Пацієнта [" + patient.Id + "] " +
                patient.FullName + " додано.");
        }

        public Patient? FindById(int id)
        {
            for (int i = 0; i < _patients.Count; i++)
            {
                if (_patients[i].Id == id)
                    return _patients[i];
            }

            return null;
        }

        public Patient[] FindByName(string name)
        {
            string search = name.ToLower();
            List<Patient> result = new List<Patient>();

            for (int i = 0; i < _patients.Count; i++)
            {
                Patient patient = _patients[i];

                if (patient.FirstName.ToLower().Contains(search)
                    || patient.LastName.ToLower().Contains(search))
                {
                    result.Add(patient);
                }
            }

            return result.ToArray();
        }

        public bool Remove(int id)
        {
            Patient? patient = FindById(id);

            if (patient == null)
                return false;

            return _patients.Remove(patient);
        }

        public Patient[] GetAll()
        {
            return _patients.ToArray();
        }

        public void DisplayAll()
        {
            Console.WriteLine("=== Пацієнти (" + _patients.Count + ") ===");

            if (_patients.Count == 0)
            {
                Console.WriteLine("Список порожній.");
                return;
            }

            for (int i = 0; i < _patients.Count; i++)
                Console.WriteLine(_patients[i]);
        }

        public void DisplayStats()
        {
            Console.WriteLine("=== Статистика пацієнтів ===");

            if (_patients.Count == 0)
            {
                Console.WriteLine("Список порожній.");
                Console.WriteLine("============================");
                return;
            }

            int sumAges = 0;
            int youngestIndex = 0;
            int oldestIndex = 0;
            int adultCount = 0;

            for (int i = 0; i < _patients.Count; i++)
            {
                sumAges += _patients[i].Age;

                if (_patients[i].Age < _patients[youngestIndex].Age)
                    youngestIndex = i;

                if (_patients[i].Age > _patients[oldestIndex].Age)
                    oldestIndex = i;

                if (_patients[i].IsAdult)
                    adultCount++;
            }

            double averageAge = (double)sumAges / _patients.Count;

            Console.WriteLine("Всього:       " + _patients.Count);
            Console.WriteLine(
                "Середній вік: " + averageAge.ToString("F1") + " р.");
            Console.WriteLine(
                "Наймолодший:  " +
                _patients[youngestIndex].FullName +
                " (" + _patients[youngestIndex].Age + " р.)");
            Console.WriteLine(
                "Найстарший:   " +
                _patients[oldestIndex].FullName +
                " (" + _patients[oldestIndex].Age + " р.)");
            Console.WriteLine(
                "Дорослих:     " + adultCount + " з " + _patients.Count);
            Console.WriteLine("============================");
        }
    }
}
