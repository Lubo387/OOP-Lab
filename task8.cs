using System;
class task8
{
    static void Main()
    {
        Console.Write("Відділення: ");
        int n = int.Parse(Console.ReadLine());
        Console.Write("Тижнів: ");
        int w = int.Parse(Console.ReadLine());
        int[,,] a = new int[n, w, 2];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < w; j++)
            {
                Console.Write("Відділення " + (i + 1) + ", тиждень " + (j + 1) + ", ранок: ");
                a[i, j, 0] = int.Parse(Console.ReadLine());
                Console.Write("Відділення " + (i + 1) + ", тиждень " + (j + 1) + ", вечір: ");
                a[i, j, 1] = int.Parse(Console.ReadLine());
            }
        }
        int max = 0;
        int dep = 0;

        for (int i = 0; i < n; i++)
        {
            int sum = 0;
            for (int j = 0; j < w; j++)
            {
                sum += a[i, j, 0];
                sum += a[i, j, 1];
            }
            Console.WriteLine("Відділення " + (i + 1) + ": " + sum);

            if (sum > max)
            {
                max = sum;
                dep = i;
            }
        }
        Console.WriteLine("Найзавантаженіше відділення: " + (dep + 1));
        Console.WriteLine("Кількість прийомів: " + max);
    }
}