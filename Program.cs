using System;
using System.Diagnostics;
using System.Threading;

class Vector
{
    private int[] elements;
    private Random random;

    public Vector(int size)
    {
        elements = new int[size];
        random = new Random();
        GenerateRandomElements();
    }

    private void GenerateRandomElements()
    {
        for (int i = 0; i < elements.Length; i++)
        {
            elements[i] = random.Next(1, 101); // Генеруємо випадкове число від 1 до 100 (можна змінити за потребою)
        }
    }

    public int this[int index]
    {
        get { return elements[index]; }
        set { elements[index] = value; }
    }

    public int Length
    {
        get { return elements.Length; }
    }

    public int CalculateSum()
    {
        int sum = 0;
        foreach (int element in elements)
        {
            sum += element;
        }
        return sum;
    }

    public int CalculateParallelSum()
    {
        int sum = 0;
        int numThreads = 1;
        int chunkSize = elements.Length / numThreads; ;

        Thread[] threads = new Thread[numThreads];

        for (int i = 0; i < numThreads; i++)
        {
            int start = i * chunkSize;
            int end = (i == numThreads - 1) ? elements.Length : start + chunkSize;
            threads[i] = new Thread(() => { sum += CalculateChunkSum(start, end); });
            threads[i].Start();
        }

        foreach (Thread thread in threads)
        {
            thread.Join();
        }

        return sum;
    }

    private int CalculateChunkSum(int start, int end)
    {
        int chunkSum = 0;
        for (int i = start; i < end; i++)
        {
            chunkSum += elements[i];
        }
        return chunkSum;
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.Write("Введіть розмір вектору: ");
        int size = int.Parse(Console.ReadLine());

        Vector vector = new Vector(size);
        /*
        Console.WriteLine("Елементи вектору:");

        for (int i = 0; i < size; i++)
        {
            Console.WriteLine($"Елемент {i + 1}: {vector[i]}");
        }
       */
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        int sum = vector.CalculateParallelSum();

        stopwatch.Stop();

        Console.WriteLine($"Сума елементів вектору: {sum}");
        Console.WriteLine($"Час виконання програми: {stopwatch.ElapsedMilliseconds} мілісекунд");
    }
}
