using System;
class task6
{
    static void Main()
    {
        Console.Write("Кількість лікарів: ");
        int n = int.Parse(Console.ReadLine());
        double[][] a = new double[n][];

        for (int i = 0; i < n; i++)
        {
            Console.Write("Кількість прийомів лікаря " + (i + 1) + ": ");
            int m = int.Parse(Console.ReadLine());
            a[i] = new double[m];

            for (int j = 0; j < m; j++)
            {
                Console.Write("Вартість: ");
                a[i][j] = double.Parse(Console.ReadLine());
            }
        }
        double max = 0;
        int doctor = 0;

        for (int i = 0; i < n; i++)
        {
            double sum = 0;
            for (int j = 0; j < a[i].Length; j++)
            {
                sum += a[i][j];
            }
            Console.WriteLine("Лікар " + (i + 1) + ": " + sum);
            if (sum > max)
            {
                max = sum;
                doctor = i;
            }
        }
        Console.WriteLine("Найбільший дохід у лікаря " + (doctor + 1));
        Console.WriteLine("Дохід: " + max);
    }
}