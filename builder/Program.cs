using static utils.DateTimeStrings;

namespace builder;

partial class Program
{
    static int Main(string[] args)
    {
        Console.WriteLine($"Builder started at {getTimeString(DateTime.Now)}");

        // ---------------- Компиляция ----------------

        var ec = MainBuild();
        if (ec != ErrorCodes.Success)
        {
            Console.WriteLine($"{getTimeString(DateTime.Now)}. Error during build");
            return (int) ec;
        }

        Console.WriteLine($"Tests started at {getTimeString(DateTime.Now)}");

        // ---------------- Тесты ----------------
        ec = MainTests();
        if (ec != ErrorCodes.Success)
        {
            Console.WriteLine($"{getTimeString(DateTime.Now)}. Error during tests");
            return (int) ec;
        }

        Console.WriteLine($"Builder successfully ended at {getTimeString(DateTime.Now)}");

        return (int) ErrorCodes.Success;
    }

    public enum ErrorCodes
    {
        // Успех
        Success = 0,
        
        // Ошибка, которая неверно преобразуется в ErrorCodes
        Unknown = -1,

        // Наличие файла builder.lock - это означает, что билд был неуспешным
        Builder_Lock = 1
    };

}
