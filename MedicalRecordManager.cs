using System;
using ClinicApp.Models;

namespace ClinicApp.Managers
{
    public class MedicalRecordManager
    {
        private readonly MedicalRecord[] _records = new MedicalRecord[1000];
        private int _count;

        public int Count => _count;

        public MedicalRecord this[int index]
        {
            get
            {
                if (index < 0 || index >= _count) return null;
                return _records[index];
            }
        }

        public void Add(MedicalRecord record)
        {
            if (record == null) throw new ArgumentNullException(nameof(record));
            if (_count >= _records.Length) throw new InvalidOperationException("Досягнуто ліміту медичних записів.");
            _records[_count++] = record;
        }

        public MedicalRecord FindById(int id)
        {
            for (int i = 0; i < _count; i++)
                if (_records[i].Id == id) return _records[i];
            return null;
        }

        public MedicalRecord[] GetByPatient(int patientId)
        {
            MedicalRecord[] result = new MedicalRecord[_count]; int count = 0;
            for (int i = 0; i < _count; i++)
                if (_records[i].PatientId == patientId) result[count++] = _records[i];
            return Trim(result, count);
        }

        public MedicalRecord[] GetByDoctor(int doctorId)
        {
            MedicalRecord[] result = new MedicalRecord[_count]; int count = 0;
            for (int i = 0; i < _count; i++)
                if (_records[i].DoctorId == doctorId) result[count++] = _records[i];
            return Trim(result, count);
        }

        public void DisplayAll() => DisplayList(Trim(_records, _count));

        public void DisplayList(MedicalRecord[] records)
        {
            if (records == null || records.Length == 0) { Console.WriteLine("Записів не знайдено."); return; }
            for (int i = 0; i < records.Length; i++)
                if (records[i] != null) Console.WriteLine(records[i]);
        }

        private static MedicalRecord[] Trim(MedicalRecord[] source, int count)
        {
            MedicalRecord[] result = new MedicalRecord[count];
            for (int i = 0; i < count; i++) result[i] = source[i];
            return result;
        }
    }
}
