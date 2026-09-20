using System;
class task4
{
    static void Main()
    {
        Console.Write("Кількість лікарів: ");
        int n = int.Parse(Console.ReadLine());
        Console.Write("Кількість днів: ");
        int m = int.Parse(Console.ReadLine());
        int[,] a = new int[n, m];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                Console.Write("[" + i + "," + j + "]: ");
                a[i, j] = int.Parse(Console.ReadLine());
            }
        }
        Console.WriteLine();

        for (int i = 0; i < n; i++)
        {
            int sum = 0;
            for (int j = 0; j < m; j++)
            {
                sum += a[i, j];
            }
            Console.WriteLine("Лікар " + (i + 1) + ": " + sum);
        }

        for (int j = 0; j < m; j++)
        {
            int sum = 0;

            for (int i = 0; i < n; i++)
            {
                sum += a[i, j];
            }
            Console.WriteLine("День " + (j + 1) + ": " + sum);
        }
        int max = a[0, 0];
        int x = 0;
        int y = 0;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (a[i, j] > max)
                {
                    max = a[i, j];
                    x = i;
                    y = j;
                }
            }
        }
        Console.WriteLine("Максимум: " + max);
        Console.WriteLine("Позиція: [" + x + "," + y + "]");
    }
}