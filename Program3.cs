using ClinicApp;
using System.Numerics;

Clinic clinic = new Clinic("Медична Клініка");
SeedData(clinic);

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
    string choice = Console.ReadLine()!;

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
            Console.Write("Дата (дд.мм.рррр): ");
            DateTime scheduleDate;
            if (DateTime.TryParse(Console.ReadLine(), out scheduleDate))
            {
                clinic.DisplaySchedule(scheduleDate);
            }
            else
            {
                Console.WriteLine("Некоректна дата.");
            }
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

void SeedData(Clinic c)
{
    c.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 3, 12), "A+", "0501234567"));
    c.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1993, 7, 5), "B-", "0672345678"));
    c.Patients.Add(new Patient("Максим", "Бойко", new DateTime(2010, 11, 20), "O+", "0933456789"));
    c.Patients.Add(new Patient());
    c.Patients.Add(new Patient("Марія", "Ткач"));

    c.Doctors.Add(new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567"));
    c.Doctors.Add(new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678"));
    c.Doctors.Add(new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789"));

    c.Appointments.Book(1, 1, DateTime.Today.AddDays(1).AddHours(10));
    c.Appointments.Book(2, 2, DateTime.Today.AddDays(1).AddHours(11), 45);
    c.Appointments.Book(3, 3, DateTime.Today.AddDays(2).AddHours(9), 20);
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
        string choice = Console.ReadLine()!;

        switch (choice)
        {
            case "1":
                c.Patients.DisplayAll();
                break;
            case "2":
                Console.Write("Ім'я: ");
                string firstName = Console.ReadLine()!;
                Console.Write("Прізвище: ");
                string lastName = Console.ReadLine()!;
                Console.Write("Дата народження (дд.мм.рррр): ");
                DateTime dob;
                DateTime.TryParse(Console.ReadLine(), out dob);
                Console.Write("Група крові: ");
                string bloodType = Console.ReadLine()!;
                Console.Write("Телефон: ");
                string phone = Console.ReadLine()!;
                c.Patients.Add(new Patient(firstName, lastName, dob, bloodType, phone));
                break;
            case "3":
                Console.Write("Ім'я або прізвище для пошуку: ");
                string search = Console.ReadLine()!;
                Patient[] found = c.Patients.FindByName(search);
                if (found.Length == 0)
                {
                    Console.WriteLine("Нікого не знайдено.");
                }
                else
                {
                    foreach (Patient p in found)
                    {
                        Console.WriteLine(p);
                    }
                }
                break;
            case "4":
                Console.Write("ID пацієнта для видалення: ");
                int removeId;
                int.TryParse(Console.ReadLine(), out removeId);
                if (c.Patients.Remove(removeId))
                {
                    Console.WriteLine("Пацієнта видалено.");
                }
                else
                {
                    Console.WriteLine("Пацієнта не знайдено.");
                }
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
        string choice = Console.ReadLine()!;

        switch (choice)
        {
            case "1":
                c.Doctors.DisplayAll();
                break;
            case "2":
                Console.Write("Ім'я: ");
                string firstName = Console.ReadLine()!;
                Console.Write("Прізвище: ");
                string lastName = Console.ReadLine()!;
                Console.Write("Спеціальність: ");
                string speciality = Console.ReadLine()!;
                Console.Write("Номер ліцензії: ");
                string license = Console.ReadLine()!;
                Console.Write("Телефон: ");
                string phone = Console.ReadLine()!;
                c.Doctors.Add(new Doctor(firstName, lastName, speciality, license, phone));
                break;
            case "3":
                Console.Write("Спеціальність для пошуку: ");
                string search = Console.ReadLine()!;
                Doctor[] found = c.Doctors.FindBySpeciality(search);
                if (found.Length == 0)
                {
                    Console.WriteLine("Нікого не знайдено.");
                }
                else
                {
                    foreach (Doctor d in found)
                    {
                        Console.WriteLine(d);
                    }
                }
                break;
            case "4":
                Console.Write("ID лікаря для видалення: ");
                int removeId;
                int.TryParse(Console.ReadLine(), out removeId);
                if (c.Doctors.Remove(removeId))
                {
                    Console.WriteLine("Лікаря видалено.");
                }
                else
                {
                    Console.WriteLine("Лікаря не знайдено.");
                }
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
        string choice = Console.ReadLine()!;

        switch (choice)
        {
            case "1":
                Console.WriteLine("Майбутні записи:");
                c.Appointments.DisplayList(c.Appointments.GetUpcoming());
                break;
            case "2":
                c.Patients.DisplayAll();
                c.Doctors.DisplayAll();
                Console.Write("ID пацієнта: ");
                int patientId;
                int.TryParse(Console.ReadLine(), out patientId);
                Console.Write("ID лікаря: ");
                int doctorId;
                int.TryParse(Console.ReadLine(), out doctorId);
                Console.Write("Дата і час (дд.мм.рррр гг:хх): ");
                DateTime scheduledAt;
                DateTime.TryParse(Console.ReadLine(), out scheduledAt);
                Console.Write("Тривалість у хвилинах (Enter = 30): ");
                int duration;
                if (!int.TryParse(Console.ReadLine(), out duration))
                {
                    duration = 30;
                }
                c.Appointments.Book(patientId, doctorId, scheduledAt, duration);
                break;
            case "3":
                Console.Write("ID запису для скасування: ");
                int cancelId;
                int.TryParse(Console.ReadLine(), out cancelId);
                Console.Write("Причина (Enter = без причини): ");
                string reason = Console.ReadLine()!;
                c.Appointments.Cancel(cancelId, reason);
                break;
            case "4":
                Console.Write("ID запису для завершення: ");
                int completeId;
                int.TryParse(Console.ReadLine(), out completeId);
                c.Appointments.Complete(completeId);
                break;
            case "5":
                Console.Write("ID пацієнта: ");
                int pId;
                int.TryParse(Console.ReadLine(), out pId);
                c.Appointments.DisplayList(c.Appointments.GetByPatient(pId));
                break;
            case "6":
                Console.Write("ID лікаря: ");
                int dId;
                int.TryParse(Console.ReadLine(), out dId);
                c.Appointments.DisplayList(c.Appointments.GetByDoctor(dId));
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

void TestGrowableManager()
{
    Console.WriteLine("=== Тест GrowablePatientManager ===");
    Console.WriteLine("Додаємо пацієнтів одного за одним...");

    GrowablePatientManager growable = new GrowablePatientManager();
    int[] createdIds = new int[20];

    for (int i = 0; i < 20; i++)
    {
        Patient p = new Patient("Тест", $"Пацієнт{i + 1}");
        growable.Add(p);
        createdIds[i] = p.Id;
        Console.WriteLine($"  Додано [{p.Id}]. Розмір: {growable.Count} / {growable.Capacity}");
    }

    Console.WriteLine();
    Console.WriteLine("Тест пошуку:");

    int searchId = createdIds[9];
    Patient? foundPatient = growable.FindById(searchId);
    if (foundPatient != null)
    {
        Console.WriteLine($"  FindById({searchId}) → {foundPatient.FullName}");
    }

    Patient? notFound = growable.FindById(-1);
    if (notFound == null)
    {
        Console.WriteLine("  FindById(-1) → не знайдено");
    }

    Console.WriteLine();
    Console.WriteLine("Порівняння:");
    Console.WriteLine("  PatientManager:         100 місць (фіксовано)");
    Console.WriteLine($"  GrowablePatientManager:  {growable.Capacity} місця (зросте при потребі)");
}
//12