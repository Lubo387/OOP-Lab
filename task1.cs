using System;
class task1
{
    static void Main()
    {
        Console.Write("N: ");
        int n = int.Parse(Console.ReadLine());

        double[] a = new double[n];
        for (int i = 0; i < n; i++)
        {
            Console.Write("Вага: ");
            a[i] = double.Parse(Console.ReadLine());
        }
        double sum = 0;
        double min = a[0];
        double max = a[0];

        for (int i = 0; i < n; i++)
        {
            sum += a[i];
            if (a[i] < min)
                min = a[i];

            if (a[i] > max)
                max = a[i];
        }
        double avg = sum / n;
        int count = 0;

        for (int i = 0; i < n; i++)
        {
            if (a[i] > avg)
                count++;
        }
        Console.WriteLine("Середня вага: " + avg);
        Console.WriteLine("Мінімум: " + min);
        Console.WriteLine("Максимум: " + max);
        Console.WriteLine("Вище середньої: " + count);
    }
}