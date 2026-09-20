using System;
class task7
{
    static void Main()
    {
        Console.Write("N: ");
        int n = int.Parse(Console.ReadLine());
        string[] names = new string[n];
        double[] bmi = new double[n];

        for (int i = 0; i < n; i++)
        {
            Console.Write("Ім'я: ");
            names[i] = Console.ReadLine();
            Console.Write("ІМТ: ");
            bmi[i] = double.Parse(Console.ReadLine());
        }

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (bmi[j] < bmi[j + 1])
                {
                    double t = bmi[j];
                    bmi[j] = bmi[j + 1];
                    bmi[j + 1] = t;
                    string s = names[j];
                    names[j] = names[j + 1];
                    names[j + 1] = s;
                }
            }
        }
        Console.WriteLine();
        Console.WriteLine("Рейтинг:");

        for (int i = 0; i < n; i++)
        {
            Console.WriteLine((i + 1) + ". " + names[i] + " - " + bmi[i]);
        }
    }
}