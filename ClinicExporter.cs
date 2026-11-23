using System;
using System.IO;
using System.Text;
using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Utils
{
    public class ClinicExporter
    {
        private readonly Clinic _clinic;
        private readonly string _baseDir;

        public ClinicExporter(Clinic clinic, string baseDir = "reports")
        {
            _clinic = clinic;
            _baseDir = baseDir;
        }

        public string ExportPatients()
        {
            Patient[] patients = _clinic.Patients.GetAll();
            string[] lines = new string[patients.Length];

            for (int i = 0; i < patients.Length; i++)
                lines[i] = $"{i + 1,3}. {patients[i]}";

            return WriteReport("Пацієнти", "patients.txt", lines,
                $"Всього: {patients.Length}");
        }

        public string ExportAppointments()
        {
            Appointment[] appointments = _clinic.Appointments.GetAll();
            string[] lines = new string[appointments.Length];

            for (int i = 0; i < appointments.Length; i++)
                lines[i] = $"{i + 1,3}. {appointments[i]}";

            return WriteReport("Записи на прийом", "appointments.txt", lines,
                $"Всього: {appointments.Length}");
        }

        public string ExportBilling()
        {
            IPayable[] unpaid = _clinic.Billing.GetAllUnpaid();
            string[] lines = new string[unpaid.Length];

            for (int i = 0; i < unpaid.Length; i++)
            {
                string details = unpaid[i] is Appointment appointment
                    ? appointment.ToString()
                    : unpaid[i].ToString() ?? "Неоплачений запис";

                lines[i] = $"{i + 1,3}. {details} | Борг: {unpaid[i].GetCost():F2} грн";
            }

            string summary =
                $"Неоплачених записів: {unpaid.Length} | Загальний борг: {_clinic.Billing.GetTotalDebt():F2} грн";

            return WriteReport("Неоплачені рахунки", "billing.txt", lines, summary);
        }

        public string ExportTreatmentPlans()
        {
            TreatmentPlan[] plans = _clinic.TreatmentPlans.GetAll();
            string[] lines = new string[plans.Length];

            for (int i = 0; i < plans.Length; i++)
                lines[i] = $"{i + 1,3}. {plans[i]}";

            return WriteReport("Плани лікування", "treatment_plans.txt", lines,
                $"Всього: {plans.Length}");
        }

        public string[] ExportAll()
        {
            return new string[]
            {
                ExportPatients(),
                ExportAppointments(),
                ExportBilling(),
                ExportTreatmentPlans()
            };
        }

        private string WriteReport(
            string title,
            string fileName,
            string[] lines,
            string summary)
        {
            string directory = GetReportDirectory();
            string path = Path.Combine(directory, fileName);

            using (StreamWriter writer = new StreamWriter(
                path,
                false,
                Encoding.UTF8))
            {
                writer.WriteLine($"=== {title} — звіт ===");
                writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
                writer.WriteLine(new string('-', 50));

                foreach (string line in lines)
                    writer.WriteLine(line);

                writer.WriteLine(new string('-', 50));
                writer.WriteLine(summary);
            }

            return path;
        }

        private string GetReportDirectory()
        {
            string dateFolder = DateTime.Today.ToString("yyyy-MM-dd");
            string directory = Path.Combine(_baseDir, dateFolder);

            Directory.CreateDirectory(directory);
            return directory;
        }
    }
}
