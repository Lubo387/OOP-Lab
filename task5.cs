using System;
class task5
{
    static void Main()
    {
        Console.Write("N: ");
        int n = int.Parse(Console.ReadLine());
        int[,] a = new int[n, n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                a[i, j] = int.Parse(Console.ReadLine());
            }
        }
        int sum1 = 0;
        int sum2 = 0;
        Console.Write("Головна: ");

        for (int i = 0; i < n; i++)
        {
            Console.Write(a[i, i] + " ");
            sum1 += a[i, i];
        }
        Console.WriteLine();
        Console.Write("Побічна: ");

        for (int i = 0; i < n; i++)
        {
            Console.Write(a[i, n - 1 - i] + " ");
            sum2 += a[i, n - 1 - i];
        }
        Console.WriteLine();
        Console.WriteLine("Сума головної: " + sum1);
        Console.WriteLine("Сума побічної: " + sum2);
    }
}