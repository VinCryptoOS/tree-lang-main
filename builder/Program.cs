using static utils.DateTimeStrings;

namespace builder;

partial class Program
{
    static int Main(string[] args)
    {
        using (var opt = new NotImportantConsoleOptions())
        {
            Console.Write($"Builder started at {getTimeString(DateTime.Now)}");
        }

        // ---------------- Устанавливаем обработчики ошибок ----------------

        SetErrorHandlers();

        // ---------------- Компиляция ----------------

        var ec = MainBuild();
        if (ec.resultCode != ErrorCodes.Success)
        {
            Console.Error.WriteLine($"{getTimeString(DateTime.Now)}. Error during build");
            return (int) ec.resultCode;
        }

        if (!ec.WillTests)
            return (int) ec.resultCode;


        Console.WriteLine($"Tests started at {getTimeString(DateTime.Now)}");

        // ---------------- Тесты ----------------
        ec.resultCode = MainTests();
        if (ec.resultCode != ErrorCodes.Success)
        {
            Console.Error.WriteLine($"{getTimeString(DateTime.Now)}. Error during tests");
            return (int) ec.resultCode;
        }

        using (var opt = new NotErrorConsoleOptions())
        {
            Console.Write($"Builder successfully ended at {getTimeString(DateTime.Now)}");
        }

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

    /// <summary>Класс для временного переопределения параметров консоли</summary>
    /// <remarks>Использование:
    /// <para>Отнаследовать класс. В конструкторе задать нужные условия. Если нужно, переопределить Disposing()</para>
    /// Применять с ключевым словом using
    /// <para>using var console_opts = new ConsoleOptionsChild();</para>
    /// </remarks>
    public abstract class ConsoleOptions: IDisposable
    {
                                                        /// <summary>Первоначальный цвет фона консоли</summary>
        public ConsoleColor InitialBackgroundColor;     /// <summary>Первоначальный цвет текста консоли</summary>
        public ConsoleColor InitialForegroundColor;

        public ConsoleOptions()
        {
            InitialBackgroundColor = Console.BackgroundColor;
            InitialForegroundColor = Console.ForegroundColor;
        }

        public void Dispose()
        {
            Disposing();
        }

        public virtual void Disposing()
        {
            Console.BackgroundColor = InitialBackgroundColor;
            Console.ForegroundColor = InitialForegroundColor;

            // Делаем для того, чтобы цвет фона не распространился на следующую строку
            Console.WriteLine();
        }
    }

    public class ErrorConsoleOptions : ConsoleOptions
    {
        public ErrorConsoleOptions() => Console.BackgroundColor = ConsoleColor.DarkRed;
    }

    public class NotErrorConsoleOptions : ConsoleOptions
    {
        public NotErrorConsoleOptions()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }
    }

    public class NotImportantConsoleOptions : ConsoleOptions
    {
        public NotImportantConsoleOptions()
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }

    public static void SetErrorHandlers()
    {
        builder_lock_event       += Builder_Lock_ErrorHandler;
        updated_file_found_event += Updated_File_Found_Handler;
    }

    public static void Builder_Lock_ErrorHandler(FileInfo builder_lock_file)
    {
        using (var bg = new ErrorConsoleOptions())
        {
            Console.Error.Write("builder.lock file exists. ");
        }

        Console.Error.Write("The build was run twice or got a code error during building");
    }

    public static void Updated_File_Found_Handler(FileInfo updatedFile)
    {
        using (var opt = new NotErrorConsoleOptions())
        {
            Console.Error.Write($"Update file found: {updatedFile.FullName}");
        }
    }

}
