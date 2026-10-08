using System;
using ClinicApp.Enums;
using ClinicApp.Models;
using ClinicApp.Utils;

namespace ClinicApp.Managers
{
    public class DoctorManager
    {
        private const int MaxDoctors = 50;
        private Doctor[] _doctors = new Doctor[MaxDoctors];
        private int _count = 0;

        public int Count => _count;

        public Doctor? this[int index]
        {
            get
            {
                if (index >= 0 && index < _count)
                    return _doctors[index];

                return null;
            }
        }

        public bool TryFindById(int id, out Doctor? doctor)
        {
            doctor = FindById(id);
            return doctor != null;
        }

        public Doctor[] FindBySpeciality(Speciality speciality)
        {
            Doctor[] result = new Doctor[_count];
            int found = 0;

            for (int i = 0; i < _count; i++)
            {
                if (_doctors[i].Speciality == speciality)
                {
                    result[found] = _doctors[i];
                    found++;
                }
            }

            Array.Resize(ref result, found);
            return result;
        }

        public void Add(Doctor doctor)
        {
            if (_count >= MaxDoctors)
            {
                Console.WriteLine("Досягнуто ліміту лікарів (50).");
                return;
            }

            _doctors[_count] = doctor;
            _count++;

            Console.WriteLine(
                "Лікаря [" + doctor.Id + "] " +
                doctor.FullName + " додано.");
        }

        public Doctor? FindById(int id)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_doctors[i].Id == id)
                    return _doctors[i];
            }

            return null;
        }

        public Doctor[] FindBySpeciality(string query)
        {
            string search = query.ToLower();
            int found = 0;

            for (int i = 0; i < _count; i++)
            {
                if (ClinicFormatter.FormatSpeciality(_doctors[i].Speciality)
                        .ToLower()
                        .Contains(search)
                    || _doctors[i].Speciality.ToString()
                        .ToLower()
                        .Contains(search))
                {
                    found++;
                }
            }

            Doctor[] result = new Doctor[found];
            int index = 0;

            for (int i = 0; i < _count; i++)
            {
                if (ClinicFormatter.FormatSpeciality(_doctors[i].Speciality)
                        .ToLower()
                        .Contains(search)
                    || _doctors[i].Speciality.ToString()
                        .ToLower()
                        .Contains(search))
                {
                    result[index] = _doctors[i];
                    index++;
                }
            }

            return result;
        }

        public Doctor[] GetAll()
        {
            Doctor[] copy = new Doctor[_count];

            for (int i = 0; i < _count; i++)
                copy[i] = _doctors[i];

            return copy;
        }

        public bool Remove(int id)
        {
            int index = -1;

            for (int i = 0; i < _count; i++)
            {
                if (_doctors[i].Id == id)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
                return false;

            for (int i = index; i < _count - 1; i++)
                _doctors[i] = _doctors[i + 1];

            _count--;
            _doctors[_count] = null!;
            return true;
        }

        public void DisplayAll()
        {
            Console.WriteLine(
                "=== Лікарі (" + _count + " / " + MaxDoctors + ") ===");

            if (_count == 0)
            {
                Console.WriteLine("Список порожній.");
                return;
            }

            for (int i = 0; i < _count; i++)
                Console.WriteLine(_doctors[i]);
        }

        public void DisplayStats()
        {
            Console.WriteLine("=== Статистика лікарів ===");

            if (_count == 0)
            {
                Console.WriteLine("Список порожній.");
                Console.WriteLine("==========================");
                return;
            }

            int availableNow = 0;

            for (int i = 0; i < _count; i++)
            {
                if (_doctors[i].IsAvailableNow)
                    availableNow++;
            }

            Console.WriteLine("Всього:         " + _count);
            Console.WriteLine("Доступні зараз: " + availableNow);
            Console.WriteLine("По спеціальностях:");

            for (int i = 0; i < _count; i++)
            {
                bool alreadyCounted = false;

                for (int j = 0; j < i; j++)
                {
                    if (_doctors[j].Speciality == _doctors[i].Speciality)
                    {
                        alreadyCounted = true;
                        break;
                    }
                }

                if (alreadyCounted)
                    continue;

                int specialityCount = 0;

                for (int k = 0; k < _count; k++)
                {
                    if (_doctors[k].Speciality == _doctors[i].Speciality)
                        specialityCount++;
                }

                Console.WriteLine(
                    "  " +
                    ClinicFormatter.FormatSpeciality(_doctors[i].Speciality) +
                    ": " + specialityCount);
            }

            Console.WriteLine("==========================");
        }
    }
}
