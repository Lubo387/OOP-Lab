using System;
using ClinicApp;
using ClinicApp.Enums;
using ClinicApp.Managers;
using ClinicApp.Models;
using ClinicApp.Utils;

Clinic clinic = new Clinic("Медична Клініка");
SeedData(clinic);
DemonstrateWorkScheduleValueType();

string firstPatientName =
    clinic.Patients[0]?.FullName ?? "Пацієнтів немає";

Console.WriteLine("Перший пацієнт: " + firstPatientName);

string secondDoctorName =
    clinic.Doctors[1]?.FullName ?? "Лікаря немає";

Console.WriteLine("Другий лікар: " + secondDoctorName);
Console.WriteLine("Вік: " + ClinicFormatter.FormatAge(21));
Console.WriteLine(
    "Група крові: " + ClinicFormatter.FormatBloodType(BloodType.APositive));

bool running = true;

while (running)
{
    Console.WriteLine();
    Console.WriteLine("=== Головне меню ===");
    Console.WriteLine("1. Пацієнти");
    Console.WriteLine("2. Лікарі");
    Console.WriteLine("3. Записи");
    Console.WriteLine("4. Розклад на дату");
    Console.WriteLine("5. Звіт клініки");
    Console.WriteLine("6. Тест GrowablePatientManager");
    Console.WriteLine("0. Вихід");
    Console.Write("Ваш вибір: ");

    string choice = Console.ReadLine() ?? "";

    switch (choice)
    {
        case "1":
            PatientsMenu(clinic);
            break;
        case "2":
            DoctorsMenu(clinic);
            break;
        case "3":
            AppointmentsMenu(clinic);
            break;
        case "4":
            ShowSchedule(clinic);
            break;
        case "5":
            clinic.GenerateReport();
            break;
        case "6":
            TestGrowableManager();
            break;
        case "0":
            running = false;
            break;
        default:
            Console.WriteLine("Невірний вибір.");
            break;
    }
}

void ShowSchedule(Clinic c)
{
    Console.Write("Дата (дд.мм.рррр): ");

    if (DateTime.TryParse(Console.ReadLine(), out DateTime scheduleDate))
    {
        c.DisplaySchedule(scheduleDate);
    }
    else
    {
        Console.WriteLine("Некоректна дата.");
    }
}

void SeedData(Clinic c)
{
    c.Patients.Add(
        new Patient(
            "Іван",
            "Петренко",
            new DateTime(1985, 3, 12),
            "A+",
            "0501234567"));

    c.Patients.Add(
        new Patient(
            "Олена",
            "Коваль",
            new DateTime(1993, 7, 5),
            "B-",
            "0672345678"));

    c.Patients.Add(
        new Patient(
            "Максим",
            "Бойко",
            new DateTime(2010, 11, 20),
            "O+",
            "0933456789"));

    c.Patients.Add(new Patient());
    c.Patients.Add(new Patient("Марія", "Ткач"));

    Doctor d1 = new Doctor(
        "Олег",
        "Сидоренко",
        "Кардіологія",
        "LIC-001",
        "0441234567");

    d1.Schedule = new WorkSchedule(8, 16);
    c.Doctors.Add(d1);

    Doctor d2 = new Doctor(
        "Наталія",
        "Мороз",
        "Неврологія",
        "LIC-002",
        "0442345678");

    c.Doctors.Add(d2);

    Doctor d3 = new Doctor(
        "Андрій",
        "Власенко",
        "Педіатрія",
        "LIC-003",
        "0443456789");

    c.Doctors.Add(d3);

    c.Appointments.Book(
        1,
        1,
        DateTime.Today.AddDays(1).AddHours(10));

    c.Appointments.Book(
        2,
        2,
        DateTime.Today.AddDays(1).AddHours(11),
        45);

    c.Appointments.Book(
        3,
        3,
        DateTime.Today.AddDays(2).AddHours(9),
        20);
}

void DemonstrateWorkScheduleValueType()
{
    WorkSchedule morning = new WorkSchedule(8, 16);
    WorkSchedule copy = morning;

    copy = new WorkSchedule(10, 18);

    Console.WriteLine("=== WorkSchedule value type ===");
    Console.WriteLine("morning: " + morning);
    Console.WriteLine("copy: " + copy);
}

void PatientsMenu(Clinic c)
{
    bool back = false;

    while (!back)
    {
        Console.WriteLine();
        Console.WriteLine("--- Пацієнти ---");
        Console.WriteLine("1. Показати всіх");
        Console.WriteLine("2. Додати");
        Console.WriteLine("3. Знайти за ім'ям");
        Console.WriteLine("4. Видалити");
        Console.WriteLine("5. Статистика");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        string choice = Console.ReadLine() ?? "";

        switch (choice)
        {
            case "1":
                c.Patients.DisplayAll();
                break;
            case "2":
                AddPatient(c);
                break;
            case "3":
                FindPatient(c);
                break;
            case "4":
                RemovePatient(c);
                break;
            case "5":
                c.Patients.DisplayStats();
                break;
            case "0":
                back = true;
                break;
            default:
                Console.WriteLine("Невірний вибір.");
                break;
        }
    }
}

void AddPatient(Clinic c)
{
    Console.Write("Ім'я: ");
    string firstName = Console.ReadLine() ?? "";

    Console.Write("Прізвище: ");
    string lastName = Console.ReadLine() ?? "";

    Console.Write("Дата народження (дд.мм.рррр): ");
    DateTime.TryParse(Console.ReadLine(), out DateTime dob);

    Console.Write("Група крові: ");
    string bloodType = Console.ReadLine() ?? "";

    Console.Write("Телефон: ");
    string phone = Console.ReadLine() ?? "";

    c.Patients.Add(
        new Patient(firstName, lastName, dob, bloodType, phone));
}

void FindPatient(Clinic c)
{
    Console.Write("Ім'я або прізвище для пошуку: ");
    string search = Console.ReadLine() ?? "";

    Patient[] found = c.Patients.FindByName(search);

    if (found.Length == 0)
    {
        Console.WriteLine("Нікого не знайдено.");
        return;
    }

    foreach (Patient patient in found)
        Console.WriteLine(patient);
}

void RemovePatient(Clinic c)
{
    Console.Write("ID пацієнта для видалення: ");
    int.TryParse(Console.ReadLine(), out int removeId);

    if (c.Patients.Remove(removeId))
        Console.WriteLine("Пацієнта видалено.");
    else
        Console.WriteLine("Пацієнта не знайдено.");
}

void DoctorsMenu(Clinic c)
{
    bool back = false;

    while (!back)
    {
        Console.WriteLine();
        Console.WriteLine("--- Лікарі ---");
        Console.WriteLine("1. Показати всіх");
        Console.WriteLine("2. Додати");
        Console.WriteLine("3. Знайти за спеціальністю");
        Console.WriteLine("4. Видалити");
        Console.WriteLine("5. Статистика");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        string choice = Console.ReadLine() ?? "";

        switch (choice)
        {
            case "1":
                c.Doctors.DisplayAll();
                break;
            case "2":
                AddDoctor(c);
                break;
            case "3":
                FindDoctor(c);
                break;
            case "4":
                RemoveDoctor(c);
                break;
            case "5":
                c.Doctors.DisplayStats();
                break;
            case "0":
                back = true;
                break;
            default:
                Console.WriteLine("Невірний вибір.");
                break;
        }
    }
}

void AddDoctor(Clinic c)
{
    Console.Write("Ім'я: ");
    string firstName = Console.ReadLine() ?? "";

    Console.Write("Прізвище: ");
    string lastName = Console.ReadLine() ?? "";

    Console.Write("Спеціальність: ");
    string speciality = Console.ReadLine() ?? "";

    Console.Write("Номер ліцензії: ");
    string license = Console.ReadLine() ?? "";

    Console.Write("Телефон: ");
    string phone = Console.ReadLine() ?? "";

    Doctor doctor = new Doctor(
        firstName,
        lastName,
        speciality,
        license,
        phone);

    c.Doctors.Add(doctor);
}

void FindDoctor(Clinic c)
{
    Console.Write("Спеціальність для пошуку: ");
    string search = Console.ReadLine() ?? "";

    Doctor[] found = c.Doctors.FindBySpeciality(search);

    if (found.Length == 0)
    {
        Console.WriteLine("Нікого не знайдено.");
        return;
    }

    foreach (Doctor doctor in found)
        Console.WriteLine(doctor);
}

void RemoveDoctor(Clinic c)
{
    Console.Write("ID лікаря для видалення: ");
    int.TryParse(Console.ReadLine(), out int removeId);

    if (c.Doctors.Remove(removeId))
        Console.WriteLine("Лікаря видалено.");
    else
        Console.WriteLine("Лікаря не знайдено.");
}

void AppointmentsMenu(Clinic c)
{
    bool back = false;

    while (!back)
    {
        Console.WriteLine();
        Console.WriteLine("--- Записи ---");
        Console.WriteLine("1. Показати майбутні");
        Console.WriteLine("2. Записати пацієнта");
        Console.WriteLine("3. Скасувати запис");
        Console.WriteLine("4. Завершити запис");
        Console.WriteLine("5. Записи пацієнта");
        Console.WriteLine("6. Записи лікаря");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        string choice = Console.ReadLine() ?? "";

        switch (choice)
        {
            case "1":
                Console.WriteLine("Майбутні записи:");
                c.Appointments.DisplayList(c.Appointments.GetUpcoming());
                break;
            case "2":
                BookAppointment(c);
                break;
            case "3":
                CancelAppointment(c);
                break;
            case "4":
                CompleteAppointment(c);
                break;
            case "5":
                ShowPatientAppointments(c);
                break;
            case "6":
                ShowDoctorAppointments(c);
                break;
            case "0":
                back = true;
                break;
            default:
                Console.WriteLine("Невірний вибір.");
                break;
        }
    }
}

void BookAppointment(Clinic c)
{
    c.Patients.DisplayAll();
    c.Doctors.DisplayAll();

    Console.Write("ID пацієнта: ");
    int.TryParse(Console.ReadLine(), out int patientId);

    Console.Write("ID лікаря: ");
    int.TryParse(Console.ReadLine(), out int doctorId);

    Console.Write("Дата і час (дд.мм.рррр гг:хх): ");
    DateTime.TryParse(Console.ReadLine(), out DateTime scheduledAt);

    Console.Write("Тривалість у хвилинах (Enter = 30): ");
    string durationInput = Console.ReadLine() ?? "";

    int duration = 30;

    if (!string.IsNullOrWhiteSpace(durationInput))
        int.TryParse(durationInput, out duration);

    c.Appointments.Book(patientId, doctorId, scheduledAt, duration);
}

void CancelAppointment(Clinic c)
{
    Console.Write("ID запису для скасування: ");
    int.TryParse(Console.ReadLine(), out int cancelId);

    Console.Write("Причина (Enter = без причини): ");
    string reason = Console.ReadLine() ?? "";

    c.Appointments.Cancel(cancelId, reason);
}

void CompleteAppointment(Clinic c)
{
    Console.Write("ID запису для завершення: ");
    int.TryParse(Console.ReadLine(), out int completeId);

    c.Appointments.Complete(completeId);
}

void ShowPatientAppointments(Clinic c)
{
    Console.Write("ID пацієнта: ");
    int.TryParse(Console.ReadLine(), out int patientId);

    c.Appointments.DisplayList(
        c.Appointments.GetByPatient(patientId));
}

void ShowDoctorAppointments(Clinic c)
{
    Console.Write("ID лікаря: ");
    int.TryParse(Console.ReadLine(), out int doctorId);

    c.Appointments.DisplayList(
        c.Appointments.GetByDoctor(doctorId));
}

void TestGrowableManager()
{
    Console.WriteLine("=== Тест GrowablePatientManager ===");
    Console.WriteLine("Додаємо пацієнтів одного за одним...");

    GrowablePatientManager growable = new GrowablePatientManager();
    int[] createdIds = new int[20];

    for (int i = 0; i < 20; i++)
    {
        Patient patient = new Patient(
            "Тест",
            "Пацієнт" + (i + 1));

        growable.Add(patient);
        createdIds[i] = patient.Id;

        Console.WriteLine(
            "  Додано [" + patient.Id + "]. Розмір: " +
            growable.Count + " / " + growable.Capacity);
    }

    Console.WriteLine();
    Console.WriteLine("Тест пошуку:");

    int searchId = createdIds[9];
    Patient? foundPatient = growable.FindById(searchId);

    if (foundPatient != null)
        Console.WriteLine(
            "  FindById(" + searchId + ") → " +
            foundPatient.FullName);

    Patient? notFound = growable.FindById(-1);

    if (notFound == null)
        Console.WriteLine("  FindById(-1) → не знайдено");

    Console.WriteLine();
    Console.WriteLine("Порівняння:");
    Console.WriteLine("  PatientManager:         100 місць (фіксовано)");
    Console.WriteLine(
        "  GrowablePatientManager: " +
        growable.Capacity + " місця (зросте при потребі)");
}
