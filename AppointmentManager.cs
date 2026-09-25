using System.Numerics;

namespace ClinicApp;

public class AppointmentManager
{
    private const int MaxAppointments = 500;
    private Appointment[] _appointments = new Appointment[MaxAppointments];
    private int _count = 0;
    private PatientManager _patients;
    private DoctorManager _doctors;

    public int Count => _count;

    public AppointmentManager(PatientManager patients, DoctorManager doctors)
    {
        _patients = patients;
        _doctors = doctors;
    }

    public bool Book(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        Patient? patient = _patients.FindById(patientId);
        if (patient == null)
        {
            Console.WriteLine($"Помилка: пацієнта з ID {patientId} не знайдено.");
            return false;
        }

        Doctor? doctor = _doctors.FindById(doctorId);
        if (doctor == null)
        {
            Console.WriteLine($"Помилка: лікаря з ID {doctorId} не знайдено.");
            return false;
        }

        if (_count >= MaxAppointments)
        {
            Console.WriteLine("Досягнуто ліміту записів (500).");
            return false;
        }

        Appointment appointment = new Appointment(patientId, doctorId, scheduledAt, durationMinutes);
        _appointments[_count] = appointment;
        _count++;

        Console.WriteLine($"Запис [{appointment.Id}] створено: {patient.FullName} → {doctor.FullName} о {scheduledAt:dd.MM.yyyy HH:mm}");
        return true;
    }

    private Appointment? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].Id == id)
            {
                return _appointments[i];
            }
        }

        return null;
    }

    public bool Cancel(int id, string reason = "")
    {
        Appointment? appointment = FindById(id);
        if (appointment == null)
        {
            Console.WriteLine($"Запис [{id}] не знайдено.");
            return false;
        }

        bool cancelled = appointment.Cancel(reason);
        if (cancelled)
        {
            Console.WriteLine($"Запис [{id}] скасовано.");
        }
        else
        {
            Console.WriteLine($"Запис [{id}] вже має статус {appointment.Status}, скасувати не можна.");
        }

        return cancelled;
    }

    public bool Complete(int id)
    {
        Appointment? appointment = FindById(id);
        if (appointment == null)
        {
            Console.WriteLine($"Запис [{id}] не знайдено.");
            return false;
        }

        bool completed = appointment.Complete();
        if (completed)
        {
            Console.WriteLine($"Запис [{id}] завершено.");
        }
        else
        {
            Console.WriteLine($"Запис [{id}] вже має статус {appointment.Status}, завершити не можна.");
        }

        return completed;
    }

    public Appointment[] GetByPatient(int patientId)
    {
        int found = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId)
            {
                found++;
            }
        }

        Appointment[] result = new Appointment[found];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId)
            {
                result[index] = _appointments[i];
                index++;
            }
        }

        return result;
    }

    public Appointment[] GetByDoctor(int doctorId)
    {
        int found = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId)
            {
                found++;
            }
        }

        Appointment[] result = new Appointment[found];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId)
            {
                result[index] = _appointments[i];
                index++;
            }
        }

        return result;
    }

    public Appointment[] GetByDate(DateTime date)
    {
        int found = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == date.Date)
            {
                found++;
            }
        }

        Appointment[] result = new Appointment[found];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == date.Date)
            {
                result[index] = _appointments[i];
                index++;
            }
        }

        return result;
    }

    public Appointment[] GetUpcoming()
    {
        int found = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].IsUpcoming)
            {
                found++;
            }
        }

        Appointment[] result = new Appointment[found];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].IsUpcoming)
            {
                result[index] = _appointments[i];
                index++;
            }
        }

        return result;
    }

    public void DisplayAppointment(Appointment appointment)
    {
        Patient? patient = _patients.FindById(appointment.PatientId);
        Doctor? doctor = _doctors.FindById(appointment.DoctorId);

        string patientName = patient != null ? patient.FullName : $"Пацієнт #{appointment.PatientId}";
        string doctorName = doctor != null ? doctor.FullName : $"Лікар #{appointment.DoctorId}";

        string line = $"[{appointment.Id}] {patientName} → {doctorName} | {appointment.ScheduledAt:dd.MM.yyyy HH:mm}–{appointment.EndsAt:HH:mm} | {appointment.Status}";

        if (appointment.Notes.Length > 0)
        {
            line += $" | {appointment.Notes}";
        }

        Console.WriteLine(line);
    }

    public void DisplayList(Appointment[] appointments)
    {
        if (appointments.Length == 0)
        {
            Console.WriteLine("Записів не знайдено.");
            return;
        }

        foreach (Appointment appointment in appointments)
        {
            DisplayAppointment(appointment);
        }
    }
}