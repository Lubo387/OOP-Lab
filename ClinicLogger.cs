using System;
using System.IO;
using System.Text;
using ClinicApp.Events;

namespace ClinicApp.Utils
{
    public class ClinicLogger
    {
        private readonly string _logPath;

        public ClinicLogger(string logPath = "clinic.log")
        {
            _logPath = logPath;
        }

        public void LogInfo(string message)
        {
            Write("INFO ", message);
        }

        public void LogWarning(string message)
        {
            Write("WARN ", message);
        }

        public void LogError(string message)
        {
            Write("ERROR", message);
        }

        private void Write(string level, string message)
        {
            string line =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";

            File.AppendAllText(
                _logPath,
                line + Environment.NewLine,
                Encoding.UTF8);
        }


        public void OnPatientAdded(object? sender, PatientEventArgs e)
        {
            LogInfo(
                $"Новий пацієнт #{e.PatientId}: {e.FullName}");
        }

        public void OnAppointmentBooked(
            object? sender,
            AppointmentEventArgs e)
        {
            LogInfo(
                $"Запис #{e.AppointmentId} створено: " +
                $"пацієнт #{e.PatientId} -> лікар #{e.DoctorId}");
        }

        public void OnUrgentAppointmentBooked(
            object? sender,
            AppointmentEventArgs e)
        {
            string message =
                $"ТЕРМІНОВИЙ запис #{e.AppointmentId}: {e.Notes}";

            LogWarning(message);

            string directory = "alerts";
            Directory.CreateDirectory(directory);

            string fileName =
                $"urgent_{DateTime.Now:yyyy-MM-dd}.txt";

            string path = Path.Combine(directory, fileName);
            string line =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

            File.AppendAllText(
                path,
                line + Environment.NewLine,
                Encoding.UTF8);
        }

        public void OnAppointmentCancelled(
            object? sender,
            AppointmentEventArgs e)
        {
            LogInfo(
                $"Запис #{e.AppointmentId} скасовано. Причина: {e.Notes}");
        }

        public void OnAppointmentCompleted(
            object? sender,
            AppointmentEventArgs e)
        {
            LogInfo($"Запис #{e.AppointmentId} завершено.");
        }

        public void OnPaymentReceived(object? sender, PaymentEventArgs e)
        {
            LogInfo(
                $"Оплата запису #{e.AppointmentId}: {e.Amount:F2} грн");
        }

        public void OnPlanCompleted(
            object? sender,
            TreatmentPlanEventArgs e)
        {
            LogInfo(
                $"План лікування #{e.PlanId} завершено: {e.Diagnosis}");
        }

        public string[] GetLastLines(int n)
        {
            if (!File.Exists(_logPath) || n <= 0)
                return Array.Empty<string>();

            string[] allLines = File.ReadAllLines(_logPath, Encoding.UTF8);
            int startIndex = Math.Max(0, allLines.Length - n);
            int resultLength = allLines.Length - startIndex;
            string[] result = new string[resultLength];

            Array.Copy(allLines, startIndex, result, 0, resultLength);
            return result;
        }

        public void Clear()
        {
            if (File.Exists(_logPath))
                File.Delete(_logPath);
        }

        public bool Exists()
        {
            return File.Exists(_logPath);
        }
    }
}
