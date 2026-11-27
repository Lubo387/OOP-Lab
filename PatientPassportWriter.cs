using System;
using System.IO;
using System.Text;
using ClinicApp.Events;
using ClinicApp.Models;

namespace ClinicApp.Utils
{
    public class PatientPassportWriter
    {
        private readonly Clinic _clinic;
        private readonly string _directory;

        public PatientPassportWriter(
            Clinic clinic,
            string directory = "patients")
        {
            _clinic = clinic;
            _directory = directory;

            Directory.CreateDirectory(_directory);
        }

        public void OnPatientAdded(object? sender, PatientEventArgs e)
        {
            WritePassport(e.PatientId);
        }

        public void OnAppointmentCompleted(
            object? sender,
            AppointmentEventArgs e)
        {
            WritePassport(e.PatientId);
        }

        public void OnPlanCompleted(
            object? sender,
            TreatmentPlanEventArgs e)
        {
            WritePassport(e.PatientId);
        }

        private void WritePassport(int patientId)
        {
            Patient? patient = _clinic.Patients.FindById(patientId);

            if (patient == null)
                return;

            string path = Path.Combine(
                _directory,
                $"passport_{patientId}.txt");

            using (StreamWriter writer = new StreamWriter(
                path,
                false,
                Encoding.UTF8))
            {
                writer.WriteLine($"=== Паспорт пацієнта #{patient.Id} ===");
                writer.WriteLine(
                    $"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
                writer.WriteLine();

                writer.WriteLine("-- Особисті дані --");
                writer.WriteLine($"Ім'я: {patient.FullName}");
                writer.WriteLine(
                    $"Дата народження: {patient.DateOfBirth:dd.MM.yyyy} ({patient.Age} років)");
                writer.WriteLine(
                    $"Група крові: {ClinicFormatter.FormatBloodType(patient.BloodType)}");
                writer.WriteLine($"Телефон: {patient.Phone}");
                writer.WriteLine();

                writer.WriteLine("-- Діагнози --");
                Diagnosis[] diagnoses =
                    _clinic.MedicalRecords.GetDiagnoses(patientId);

                foreach (Diagnosis diagnosis in diagnoses)
                    writer.WriteLine(diagnosis.GetSummary());

                writer.WriteLine("-- Аналізи --");
                LabResult[] labResults =
                    _clinic.MedicalRecords.GetLabResults(patientId);

                foreach (LabResult result in labResults)
                    writer.WriteLine(result.GetSummary());

                writer.WriteLine("-- Рецепти --");
                Prescription[] prescriptions =
                    _clinic.MedicalRecords.GetPrescriptions(patientId);

                foreach (Prescription prescription in prescriptions)
                    writer.WriteLine(prescription.GetSummary());

                writer.WriteLine();
                writer.WriteLine("-- Записи на прийом --");
                Appointment[] appointments =
                    _clinic.Appointments.GetByPatient(patientId);

                foreach (Appointment appointment in appointments)
                {
                    writer.WriteLine(
                        $"[{appointment.Id}] {appointment.GetDescription()} | " +
                        $"{appointment.ScheduledAt:dd.MM.yyyy HH:mm} | " +
                        $"{appointment.Status} | {appointment.GetCost():F2} грн");
                }

                writer.WriteLine();
                writer.WriteLine("-- Плани лікування --");
                TreatmentPlan[] plans =
                    _clinic.TreatmentPlans.GetByPatient(patientId);

                foreach (TreatmentPlan plan in plans)
                    writer.WriteLine(plan);

                writer.WriteLine();
                writer.WriteLine(
                    $"Заборгованість: {_clinic.Billing.GetPatientDebt(patientId):F2} грн");
            }
        }
    }
}
