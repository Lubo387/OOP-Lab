using System;
using System.IO;
using System.Text;

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
