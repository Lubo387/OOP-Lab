using System;
class task3
{
    static void Main()
    {
        string[] days = { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Нд" };
        int[] a = new int[7];

        for (int i = 0; i < 7; i++)
        {
            Console.Write("Пацієнтів " + days[i] + ": ");
            a[i] = int.Parse(Console.ReadLine());
        }
        int sum = 0;
        int max = a[0];
        int min = a[0];
        int imax = 0;
        int imin = 0;

        for (int i = 0; i < 7; i++)
        {
            sum += a[i];
            if (a[i] > max)
            {
                max = a[i];
                imax = i;
            }
            if (a[i] < min)
            {
                min = a[i];
                imin = i;
            }
        }
        Console.WriteLine();
        Console.WriteLine("День\tПацієнти");

        for (int i = 0; i < 7; i++)
        {
            Console.WriteLine(days[i] + "\t" + a[i]);
        }
        Console.WriteLine("Всього: " + sum);
        Console.WriteLine("Найзавантаженіший: " + days[imax]);
        Console.WriteLine("Найтихіший: " + days[imin]);
    }
}