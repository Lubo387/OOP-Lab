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

        public Diagnosis[] GetDiagnoses(int patientId)
        {
            Diagnosis[] result = new Diagnosis[_count]; int count = 0;
            for (int i = 0; i < _count; i++)
            {
                Diagnosis diagnosis = _records[i] as Diagnosis;
                if (diagnosis != null && diagnosis.PatientId == patientId)
                    result[count++] = diagnosis;
            }
            return TrimDiagnoses(result, count);
        }

        public LabResult[] GetLabResults(int patientId)
        {
            LabResult[] result = new LabResult[_count]; int count = 0;
            for (int i = 0; i < _count; i++)
                if (_records[i].PatientId == patientId && _records[i] is LabResult labResult) result[count++] = labResult;
            LabResult[] trimmed = new LabResult[count]; for (int i = 0; i < count; i++) trimmed[i] = result[i]; return trimmed;
        }

        public Prescription[] GetPrescriptions(int patientId)
        {
            Prescription[] result = new Prescription[_count]; int count = 0;
            for (int i = 0; i < _count; i++)
                if (_records[i].PatientId == patientId && _records[i] is Prescription prescription) result[count++] = prescription;
            Prescription[] trimmed = new Prescription[count]; for (int i = 0; i < count; i++) trimmed[i] = result[i]; return trimmed;
        }

        public Diagnosis[] GetChronicDiagnoses(int patientId)
        {
            Diagnosis[] all = GetDiagnoses(patientId); Diagnosis[] result = new Diagnosis[all.Length]; int count = 0;
            for (int i = 0; i < all.Length; i++) if (all[i].IsChronic) result[count++] = all[i];
            return TrimDiagnoses(result, count);
        }

        public Prescription[] GetActivePrescriptions(int patientId)
        {
            Prescription[] all = GetPrescriptions(patientId); Prescription[] result = new Prescription[all.Length]; int count = 0;
            for (int i = 0; i < all.Length; i++) if (all[i].IsActive()) result[count++] = all[i];
            Prescription[] trimmed = new Prescription[count]; for (int i = 0; i < count; i++) trimmed[i] = result[i]; return trimmed;
        }

        public void DisplayPatientSummary(int patientId)
        {
            MedicalRecord[] records = GetByPatient(patientId);
            Diagnosis[] diagnoses = GetDiagnoses(patientId);
            LabResult[] labs = GetLabResults(patientId);
            Prescription[] prescriptions = GetPrescriptions(patientId);
            Diagnosis[] chronic = GetChronicDiagnoses(patientId);
            Prescription[] active = GetActivePrescriptions(patientId);

            Console.WriteLine("=== Медична картка пацієнта №" + patientId + " ===");
            Console.WriteLine("Усього записів: " + records.Length);
            Console.WriteLine("Діагнозів: " + diagnoses.Length);
            Console.WriteLine("Аналізів: " + labs.Length);
            Console.WriteLine("Рецептів: " + prescriptions.Length);

            if (chronic.Length > 0)
            {
                Console.WriteLine("Хронічні діагнози:");
                for (int i = 0; i < chronic.Length; i++) Console.WriteLine("- " + chronic[i].GetSummary());
            }
            if (active.Length > 0)
            {
                Console.WriteLine("Активні рецепти:");
                for (int i = 0; i < active.Length; i++)
                    Console.WriteLine("- " + active[i].GetSummary() + "; до " + active[i].ExpiresAt.ToString("dd.MM.yyyy"));
            }
        }

        private static Diagnosis[] TrimDiagnoses(Diagnosis[] source, int count)
        {
            Diagnosis[] result = new Diagnosis[count]; for (int i = 0; i < count; i++) result[i] = source[i]; return result;
        }

    }
}
