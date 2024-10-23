using System;

namespace Lab2;

class Program
{
    public static void Main(string[] _)
    {
        while (true)
        {
            if (SelectTask(RequestTask()) is Action task) task();
            else break;
        }
    }

    private static int RequestTask()
    {
        const string requestText =
            "Select task:\n" +
            "0 - exit\n" +
            "1 - task 1 (variant 15)\n" +
            "2 - task 2 (variant 34)\n" +
            "3 - task 3 (variant 38)\n" +
            "4 - additional 1\n" +
            "5 - additional 2\n" +
            "6 - additional 3\n" +
            "7 - additional 4\n" +
            "> ";

        Console.Write(requestText);
        return IO.ReadInt();
    }

    private static Action? SelectTask(int choice)
    {
        return choice switch
        {
            0 => null,
            1 => Tasks.Task1,
            2 => Tasks.Task2,
            3 => Tasks.Task3,
            4 => Tasks.DoAdditional1,
            5 => Tasks.DoAdditional2,
            6 => Tasks.DoAdditional3,
            7 => Tasks.DoAdditional4,
            _ => () => { Console.WriteLine($"Invalid task: {choice}"); }
        };
    }
}

static class IO
{
    public static int ReadInt(string message = "")
    {
        if (message != "") Console.Write($"{message}\n> ");

        return TryReadInt() switch
        {
            int value => value,
            _ => ReadInt(message),
        };
    }

    public static int? TryReadInt()
    {
        var input = Console.ReadLine() ?? "";

        try
        {
            return int.Parse(input);
        }
        catch
        {
            Console.WriteLine($"Invalid input: '{input}'");
            return null;
        }
    }

    public static double ReadDouble(string message = "")
    {
        if (message != "") Console.Write($"{message}\n> ");

        return TryReadDouble() switch
        {
            double value => value,
            _ => ReadDouble(message),
        };
    }

    public static double? TryReadDouble()
    {
        var input = Console.ReadLine() ?? "";

        try
        {
            return double.Parse(input);
        }
        catch
        {
            Console.WriteLine($"Invalid input: '{input}'");
            return null;
        }
    }
}

enum Loop
{
    For,
    While,
    DoWhile,
    GoTo,
}

static class LoopHelper
{
    public static Loop RequestFromUser()
    {
        const string requestText =
            "Select loop:\n" +
            "1 - for\n" +
            "2 - while\n" +
            "3 - do while\n" +
            "> ";

        Console.Write(requestText);
        return IO.TryReadInt() switch
        {
            1 => Loop.For,
            2 => Loop.While,
            3 => Loop.DoWhile,
            _ => RequestFromUser(),
        };
    }
}

class Tasks
{
    // Варіант 15
    //
    // Дана послідовність з n цілих чисел. Знайти кількість елементів цієї
    // послідовності, кратних її першому елементу.
    public static void Task1()
    {
        static void TaskHelper(int first, ref int count)
        {
            int num = IO.ReadInt();
            if (num % first == 0) count++;
        }

        const bool isFirstNumCounts = true;

        var loop = LoopHelper.RequestFromUser();
        int n = IO.ReadInt("Input number of the elements");

        if (n <= 0)
        {
            Console.WriteLine(0);
            return;
        }

        int first = IO.ReadInt("Input elements...");
        int count = isFirstNumCounts ? 1 : 0;

        switch (loop)
        {
            case Loop.For:
                for (int i = 1; i < n; ++i) TaskHelper(first, ref count);
                break;

            case Loop.While:
                {
                    int i = 1;
                    while (i < n)
                    {
                        TaskHelper(first, ref count);
                        ++i;
                    }
                }
                break;

            case Loop.DoWhile:
                if (n != 1)
                {
                    int i = 1;
                    do
                    {
                        TaskHelper(first, ref count);
                        ++i;
                    } while (i < n);
                }
                break;

            case Loop.GoTo:
                {
                    int i = 1;
                begin:
                    if (i < n)
                    {
                        TaskHelper(first, ref count);
                        ++i;
                        goto begin;
                    }
                }
                break;
        }

        Console.WriteLine($"Count is {count}");
    }

    // Варіант 34
    //
    // Дана послідовність цілих чисел, за якою слідує 0. Визначити, яких чисел
    // в цій послідовності більше: додатних чи від’ємних.
    public static void Task2()
    {
        static bool TaskHelper(ref int positives, ref int negatives)
        {
            var num = IO.ReadInt();
            if (num == 0) return false;
            if (num > 0) ++positives;
            else ++negatives;
            return true;
        }

        var negativeCount = 0;
        var positiveCount = 0;
        var loop = LoopHelper.RequestFromUser();

        Console.WriteLine("Input elements...");

        switch (loop)
        {
            case Loop.For:
                for (; ; )
                {
                    if (!TaskHelper(ref positiveCount, ref negativeCount))
                    {
                        break;
                    }
                }
                break;

            case Loop.While:
                while (true)
                {
                    if (!TaskHelper(ref positiveCount, ref negativeCount))
                    {
                        break;
                    }
                }
                break;

            case Loop.DoWhile:
                bool isNotZero;
                do
                {
                    isNotZero = TaskHelper(ref positiveCount, ref negativeCount);
                }
                while (isNotZero);
                break;

            case Loop.GoTo:
            begin:
                if (TaskHelper(ref positiveCount, ref negativeCount))
                {
                    goto begin;
                }
                break;
        }

        Console.WriteLine(
            negativeCount == positiveCount
            ? $"Positive & Negative numbers count is equal ({positiveCount} = {negativeCount})"
            : positiveCount > negativeCount
            ? $"Positive numbers count is higher ({positiveCount} > {negativeCount})"
            : $"Negative numbers count is higher ({positiveCount} < {negativeCount})"
        );
    }

    // Варіант 38
    //
    // S = 15 + 17 – 19 + 21 + 23 – 25 + ..., де всього n доданків.
    public static void Task3()
    {
        const int x0 = 15;

        static void TaskHelper(int i, ref int result)
        {
            var xi = x0 + (i - 1) * 2;
            result += (i % 3 == 0) ? -xi : xi;
        }

        var loop = LoopHelper.RequestFromUser();
        var n = IO.ReadInt("Input N");
        var result = 0;

        switch (loop)
        {
            case Loop.For:
                for (int i = 1; i <= n; ++i)
                {
                    TaskHelper(i, ref result);
                }
                break;

            case Loop.While:
                {
                    int i = 1;
                    while (i <= n)
                    {
                        TaskHelper(i, ref result);
                        ++i;
                    }
                }
                break;

            case Loop.DoWhile:
                if (n != 1)
                {
                    int i = 1;
                    do
                    {
                        TaskHelper(i, ref result);
                        ++i;
                    } while (i <= n);
                }
                break;

            case Loop.GoTo:
                {
                    int i = 1;
                begin:
                    if (i <= n)
                    {
                        TaskHelper(i, ref result);
                        ++i;
                        goto begin;
                    }
                }
                break;
        }

        Console.WriteLine(result);
    }

    // Додаткове 1
    //
    // S = √(3 + √(6 + √(9 + ... √(3n))))
    public static void DoAdditional1()
    {
        static double PrintExpr(System.IO.TextWriter? w, double num, double max)
        {
            if (num >= max)
            {
                w?.Write($"√{num}");
                return Math.Sqrt(num);
            }

            w?.Write($"√({num} + ");
            var next = PrintExpr(w, num + 3.0, max);
            w?.Write(")");

            return Math.Sqrt(num + next);
        }

        static void TaskHelper(int num, ref double result)
        {
            result = num + Math.Sqrt(result);
        }

        var loop = LoopHelper.RequestFromUser();
        var n = IO.ReadInt("Input N");
        var result = 0.0;
        var num = 3 * n;

        switch (loop)
        {
            case Loop.For:
                for (; num >= 3; num -= 3)
                {
                    TaskHelper(num, ref result);
                }
                break;

            case Loop.While:
                while (num >= 3)
                {
                    TaskHelper(num, ref result);
                    num -= 3;
                }
                break;

            case Loop.DoWhile:
                if (n >= 3)
                {
                    do
                    {
                        TaskHelper(num, ref result);
                        num -= 3;
                    }
                    while (num >= 3);
                }
                break;

            case Loop.GoTo:
            begin:
                if (num >= 3)
                {
                    TaskHelper(num, ref result);
                    num -= 3;
                    goto begin;
                }
                break;
        }

        // var buf = new StringWriter();
        // var recResult = TaskHelper(buf, 3.0, 3.0 * n);
        // Console.WriteLine(buf);

        result = Math.Sqrt(result);
        Console.WriteLine(result);
    }

    // Додаткове 2
    //
    // S = sin(x + sin(2x − sin(3x + sin(4x + sin(5x − sin(6x + ...) ...)
    //
    //  * до sin(nx) включно
    //  * на кожні три рази двічі відбувається додавання, один раз віднімання
    public static void DoAdditional2()
    {
        static void TaskHelper(int i, double x, ref double result)
        {
            result = (i + 1) % 3 == 0
                   ? i * x - Math.Sin(result)
                   : i * x + Math.Sin(result);
        }

        var loop = LoopHelper.RequestFromUser();
        var n = IO.ReadInt("Input N");
        var x = IO.ReadDouble("Input X");
        var result = 0.0;
        var i = n;

        switch (loop)
        {
            case Loop.For:
                for (; i >= 1; --i)
                {
                    TaskHelper(i, x, ref result);
                }
                break;

            case Loop.While:
                while (i >= 1)
                {
                    TaskHelper(i, x, ref result);
                    --i;
                }
                break;

            case Loop.DoWhile:
                if (i >= 1)
                {
                    do
                    {
                        TaskHelper(i, x, ref result);
                        --i;
                    }
                    while (i >= 1);
                }
                break;

            case Loop.GoTo:
            begin:
                if (i >= 1)
                {
                    TaskHelper(i, x, ref result);
                    --i;
                    goto begin;
                }
                break;
        }

        result = Math.Sin(result);
        Console.WriteLine(result);
    }

    // Додаткове 3
    //
    // S = sin(x + cos(2x + sin(3x + cos(4x + sin(5x + cos(6x + ...) ...)
    //
    //  * до sin(nx) чи cos(nx) включно, sin(nx) чи cos(nx) залежить від парності n
    public static void DoAdditional3()
    {
        static void TaskHelper(int i, double x, ref double result)
        {
            result = i % 2 == 0
                   ? i * x + Math.Sin(result)
                   : i * x + Math.Cos(result);
        }

        var loop = LoopHelper.RequestFromUser();
        var n = IO.ReadInt("Input N");
        var x = IO.ReadDouble("Input X");
        var result = 0.0;
        var i = n;

        switch (loop)
        {
            case Loop.For:
                for (; i >= 1; --i)
                {
                    TaskHelper(i, x, ref result);
                }
                break;

            case Loop.While:
                while (i >= 1)
                {
                    TaskHelper(i, x, ref result);
                    --i;
                }
                break;

            case Loop.DoWhile:
                if (i >= 1)
                {
                    do
                    {
                        TaskHelper(i, x, ref result);
                        --i;
                    }
                    while (i >= 1);
                }
                break;

            case Loop.GoTo:
            begin:
                if (i >= 1)
                {
                    TaskHelper(i, x, ref result);
                    --i;
                    goto begin;
                }
                break;
        }

        result = Math.Sin(result);
        Console.WriteLine(result);
    }

    // Додаткове 4
    //
    // S = sin(x + cos(2x − sin(3x + cos(4x + sin(5x − cos(6x + ...) ...)
    //
    //  * до sin(nx) чи cos(nx) включно, sin(nx) чи cos(nx) залежить від парності n
    //  * на кожні три рази двічі відбувається додавання, один раз віднімання
    public static void DoAdditional4()
    {
        static void TaskHelper(int i, double x, ref double result)
        {
            var tmp = i % 2 == 0
                   ? Math.Sin(result)
                   : Math.Cos(result);

            // for debug
            var prev = result;
            var f = i % 2 == 0 ? "sin" : "cos";
            var op = (i + 1) % 3 == 0 ? "-" : "+";

            result = (i + 1) % 3 == 0
                   ? i * x - tmp
                   : i * x + tmp;

            Console.WriteLine($"{i}x {op} {f}({prev}) = {result}");
        }

        var loop = LoopHelper.RequestFromUser();
        var n = IO.ReadInt("Input N");
        var x = IO.ReadDouble("Input X");
        var result = 0.0;
        var i = n;

        switch (loop)
        {
            case Loop.For:
                for (; i >= 1; --i)
                {
                    TaskHelper(i, x, ref result);
                }
                break;

            case Loop.While:
                while (i >= 1)
                {
                    TaskHelper(i, x, ref result);
                    --i;
                }
                break;

            case Loop.DoWhile:
                if (n >= 1)
                {
                    do
                    {
                        TaskHelper(i, x, ref result);
                        --i;
                    }
                    while (i >= 1);
                }
                break;

            case Loop.GoTo:
            begin:
                if (i >= 1)
                {
                    TaskHelper(i, x, ref result);
                    --i;
                    goto begin;
                }
                break;
        }

        result = Math.Sin(result);
        Console.WriteLine(result);
    }
}
