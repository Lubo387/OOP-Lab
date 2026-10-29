using System;
using ClinicApp;
using ClinicApp.Enums;
using ClinicApp.Managers;
using ClinicApp.Interfaces;
using ClinicApp.Models;
using ClinicApp.Utils;
using System.Globalization;

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

Clinic clinic = new Clinic("Медична Клініка");
SeedData(clinic);
DemonstrateWorkScheduleValueType();
DemonstrateAppointmentPolymorphism();

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
    Console.WriteLine("╔════════════════════════════════════════════════╗");
    Console.WriteLine("║              МЕДИЧНА КЛІНІКА                  ║");
    Console.WriteLine("╠════════════════════════════════════════════════╣");
    Console.WriteLine("║  1. Пацієнти       — реєстрація, пошук         ║");
    Console.WriteLine("║  2. Лікарі         — персонал, розклад         ║");
    Console.WriteLine("║  3. Записи         — прийоми, скасування       ║");
    Console.WriteLine("║  4. Медична картка — діагнози, рецепти         ║");
    Console.WriteLine("║  5. Рахунки        — оплата, борги             ║");
    Console.WriteLine("║  6. Звіт           — загальна статистика       ║");
    Console.WriteLine("║  0. Вийти                                      ║");
    Console.WriteLine("╚════════════════════════════════════════════════╝");
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
            MedicalRecordsMenu(clinic);
            break;
        case "5":
            BillingMenu(clinic);
            break;
        case "6":
            clinic.GenerateReport();
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

    c.Appointments.BookUrgent(
        2,
        2,
        DateTime.Today.AddDays(1).AddHours(11),
        "сильний біль",
        45);

    c.Appointments.BookSpecialist(
        3,
        3,
        DateTime.Today.AddDays(2).AddHours(9),
        "педіатрична консультація",
        45);

    c.MedicalRecords.Add(new Diagnosis(1, 1, DateTime.Today.AddDays(-20), "I10", "Гіпертонічна хвороба", true) { Notes = "Контроль артеріального тиску" });
    c.MedicalRecords.Add(new LabResult(1, 1, DateTime.Today.AddDays(-5), "Гемоглобін", 145, "г/л", "120–160", true));
    c.MedicalRecords.Add(new Prescription(1, 1, DateTime.Today.AddDays(-3), "Лізиноприл", "10 мг", 30, "1 раз на добу вранці"));
    c.MedicalRecords.Add(new Diagnosis(2, 2, DateTime.Today.AddDays(-15), "J06.9", "Гостра інфекція верхніх дихальних шляхів"));
    c.MedicalRecords.Add(new LabResult(2, 2, DateTime.Today.AddDays(-2), "Глюкоза", 6.8, "ммоль/л", "3.9–5.5", false));
    c.MedicalRecords.Add(new Prescription(2, 2, DateTime.Today.AddDays(-45), "Ібупрофен", "200 мг", 5, "за потреби"));
    c.MedicalRecords.Add(new Diagnosis(3, 1, DateTime.Today.AddDays(-10), "M54.5", "Біль у попереку", true));
    c.MedicalRecords.Add(new LabResult(3, 1, DateTime.Today.AddDays(-1), "Холестерин", 4.7, "ммоль/л", "до 5.0", true));

}

void DemonstrateAppointmentPolymorphism()
{
    UrgentAppointment urgent = new UrgentAppointment(
        1,
        1,
        DateTime.Today.AddDays(1).AddHours(10),
        "тест");

    Appointment appointmentReference = urgent;
    UrgentAppointment urgentReference = urgent;

    Console.WriteLine();
    Console.WriteLine("== new vs override ==");
    Console.WriteLine(
        "Через Appointment:       " +
        appointmentReference.GetDescription() +
        " | пріоритет " + appointmentReference.GetPriority());
    Console.WriteLine(
        "Через UrgentAppointment: " +
        urgentReference.GetDescription() +
        " | пріоритет " + urgentReference.GetPriority());

    // GetDescription() є virtual, тому виконується sealed override з UrgentAppointment.
    // GetPriority() не virtual, тому вибирається за типом змінної: Appointment повертає 3,
    // а UrgentAppointment — 1 через метод new. Для поліморфного пріоритету треба
    // зробити Appointment.GetPriority() virtual, а UrgentAppointment.GetPriority() override.
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
        Console.WriteLine("5. Вільні години лікаря");
        Console.WriteLine("6. Статистика");
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
    try
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

        Patient patient = new Patient(
            firstName,
            lastName,
            dob,
            bloodType,
            phone);

        c.Patients.Add(patient);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine("Помилка: " + ex.Message);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine("Помилка: " + ex.Message);
    }
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
                ShowAvailableDoctorSlots(c);
                break;
            case "6":
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
    try
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

        Console.Write("Початок роботи (0-23): ");
        int startHour = int.Parse(Console.ReadLine() ?? "");

        Console.Write("Кінець роботи (1-24): ");
        int endHour = int.Parse(Console.ReadLine() ?? "");

        WorkSchedule schedule = new WorkSchedule(startHour, endHour);

        Doctor doctor = new Doctor(
            firstName,
            lastName,
            speciality,
            license,
            phone);

        doctor.Schedule = schedule;
        c.Doctors.Add(doctor);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine("Помилка: " + ex.Message);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine("Помилка: " + ex.Message);
    }
    catch (FormatException ex)
    {
        Console.WriteLine("Помилка: введіть число. " + ex.Message);
    }
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
        Console.WriteLine("8. Скасувати всі записи пацієнта");
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
            case "8":
                CancelPatientAppointments(c);
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
    try
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

        c.Appointments.Book(
            patientId,
            doctorId,
            scheduledAt,
            duration);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine("Помилка: " + ex.Message);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine("Помилка: " + ex.Message);
    }
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


void MedicalRecordsMenu(Clinic c)
{
    bool back = false;
    while (!back)
    {
        Console.WriteLine("\n=== Медична картка ===");
        Console.WriteLine("1. Картка пацієнта");
        Console.WriteLine("2. Усі записи пацієнта");
        Console.WriteLine("3. Додати діагноз");
        Console.WriteLine("4. Додати аналіз");
        Console.WriteLine("5. Додати рецепт");
        Console.WriteLine("6. Записи лікаря");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");
        string choice = Console.ReadLine() ?? "";

        switch (choice)
        {
            case "1":
                int summaryPatient = ReadPositiveInt("ID пацієнта: ");
                c.MedicalRecords.DisplayPatientSummary(summaryPatient);
                break;
            case "2":
                int patientId = ReadPositiveInt("ID пацієнта: ");
                c.MedicalRecords.DisplayList(c.MedicalRecords.GetByPatient(patientId));
                break;
            case "3": AddDiagnosis(c); break;
            case "4": AddLabResult(c); break;
            case "5": AddPrescription(c); break;
            case "6":
                int doctorId = ReadPositiveInt("ID лікаря: ");
                c.MedicalRecords.DisplayList(c.MedicalRecords.GetByDoctor(doctorId));
                break;
            case "0": back = true; break;
            default: Console.WriteLine("Невірний вибір."); break;
        }
    }
}

int ReadPositiveInt(string prompt)
{
    Console.Write(prompt);
    while (true)
    {
        if (int.TryParse(Console.ReadLine(), out int value) && value > 0) return value;
        Console.Write("Введіть ціле число більше 0: ");
    }
}

void AddDiagnosis(Clinic c)
{
    int patientId = ReadPositiveInt("ID пацієнта: ");
    int doctorId = ReadPositiveInt("ID лікаря: ");
    Console.Write("Код діагнозу: "); string code = Console.ReadLine() ?? "";
    Console.Write("Опис: "); string description = Console.ReadLine() ?? "";
    Console.Write("Хронічний? (так/ні): "); bool chronic = (Console.ReadLine() ?? "").Trim().ToLower() == "так";
    c.MedicalRecords.Add(new Diagnosis(patientId, doctorId, DateTime.Today, code, description, chronic));
    Console.WriteLine("Діагноз додано.");
}

void AddLabResult(Clinic c)
{
    int patientId = ReadPositiveInt("ID пацієнта: ");
    int doctorId = ReadPositiveInt("ID лікаря: ");
    Console.Write("Назва аналізу: "); string name = Console.ReadLine() ?? "";
    Console.Write("Значення: "); double value = double.Parse(Console.ReadLine() ?? "0", CultureInfo.InvariantCulture);
    Console.Write("Одиниця виміру: "); string unit = Console.ReadLine() ?? "";
    Console.Write("Референсний діапазон: "); string range = Console.ReadLine() ?? "";
    Console.Write("У нормі? (так/ні): "); bool normal = (Console.ReadLine() ?? "").Trim().ToLower() == "так";
    c.MedicalRecords.Add(new LabResult(patientId, doctorId, DateTime.Today, name, value, unit, range, normal));
    Console.WriteLine("Результат аналізу додано.");
}

void AddPrescription(Clinic c)
{
    int patientId = ReadPositiveInt("ID пацієнта: ");
    int doctorId = ReadPositiveInt("ID лікаря: ");
    Console.Write("Препарат: "); string name = Console.ReadLine() ?? "";
    Console.Write("Дозування: "); string dosage = Console.ReadLine() ?? "";
    int days = ReadPositiveInt("Тривалість (днів): ");
    Console.Write("Інструкції: "); string instructions = Console.ReadLine() ?? "";
    c.MedicalRecords.Add(new Prescription(patientId, doctorId, DateTime.Today, name, dosage, days, instructions));
    Console.WriteLine("Рецепт додано.");
}


void BillingMenu(Clinic c)
{
    bool back = false;

    while (!back)
    {
        Console.WriteLine("\n── Рахунки ───────────────────");
        Console.WriteLine("  1. Борги пацієнта");
        Console.WriteLine("  2. Всі неоплачені записи");
        Console.WriteLine("  3. Оплатити запис");
        Console.WriteLine("  4. Загальний борг клініки");
        Console.WriteLine("  0. Назад");
        Console.Write("Оберіть: ");

        string choice = Console.ReadLine() ?? "";

        switch (choice)
        {
            case "1":
                if (!TryReadPositiveInt("ID пацієнта: ", out int patientId))
                    break;

                Console.WriteLine("Неоплачені записи пацієнта #" + patientId + ":");
                IPayable[] patientItems = c.Billing.GetUnpaidByPatient(patientId);
                c.Billing.DisplayUnpaid(patientItems);
                Console.WriteLine("Борг: " + c.Billing.GetPatientDebt(patientId).ToString("F2") + " грн");
                break;
            case "2":
                c.Billing.DisplayUnpaid(c.Billing.GetAllUnpaid());
                break;
            case "3":
                if (!TryReadPositiveInt("ID запису для оплати: ", out int appointmentId))
                    break;

                if (c.Billing.PayAppointment(appointmentId))
                    Console.WriteLine("Запис [" + appointmentId + "] оплачено.");
                else
                    Console.WriteLine("Не вдалося оплатити: запис не знайдено, вже оплачено або скасовано.");
                break;
            case "4":
                Console.WriteLine("Загальний борг по клініці: " + c.Billing.GetTotalDebt().ToString("F2") + " грн");
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

void CancelPatientAppointments(Clinic c)
{
    if (!TryReadPositiveInt("ID пацієнта: ", out int patientId))
        return;

    Console.Write("Причина (Enter — без причини): ");
    string reason = Console.ReadLine() ?? "";
    Appointment[] appointments = c.Appointments.GetByPatient(patientId);
    int cancelled = AppointmentManager.CancelAll(appointments, reason);

    Console.WriteLine("Скасовано записів: " + cancelled);
}

void ShowAvailableDoctorSlots(Clinic c)
{
    if (!TryReadPositiveInt("ID лікаря: ", out int doctorId))
        return;

    Doctor? doctor = c.Doctors.FindById(doctorId);

    if (doctor == null)
    {
        Console.WriteLine("Лікаря не знайдено.");
        return;
    }

    Console.Write("Дата (dd.MM.yyyy): ");
    string dateText = Console.ReadLine() ?? "";

    if (!DateTime.TryParseExact(
        dateText,
        "dd.MM.yyyy",
        CultureInfo.InvariantCulture,
        DateTimeStyles.None,
        out DateTime date))
    {
        Console.WriteLine("Некоректна дата. Використайте формат dd.MM.yyyy.");
        return;
    }

    if (!TryReadPositiveInt("Скільки слотів: ", out int slotCount))
        return;

    ISchedulable schedulable = doctor;

    try
    {
        DateTime[] slots = schedulable.GetAvailableSlots(date, slotCount);

        for (int i = 0; i < slots.Length; i++)
            Console.WriteLine(slots[i].ToString("HH:mm"));
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine("Помилка: " + ex.Message);
    }
}

bool TryReadPositiveInt(string prompt, out int value)
{
    Console.Write(prompt);

    if (int.TryParse(Console.ReadLine(), out value) && value > 0)
        return true;

    Console.WriteLine("Введіть ціле число більше 0.");
    return false;
}
