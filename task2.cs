using System;
class task2
{
    static void Main()
    {
        Console.Write("N: ");
        int n = int.Parse(Console.ReadLine());

        double[] a = new double[n];
        for (int i = 0; i < n; i++)
        {
            Console.Write("Вартість: ");
            a[i] = double.Parse(Console.ReadLine());
        }

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (a[j] > a[j + 1])
                {
                    double t = a[j];
                    a[j] = a[j + 1];
                    a[j + 1] = t;
                }
            }
        }
        Console.WriteLine("Результат:");
        for (int i = 0; i < n; i++)
        {
            Console.Write(a[i] + " ");
        }
    }
}