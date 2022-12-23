using static utils.DateTimeStrings;

namespace builder;

partial class Program
{
    static int Main(string[] args)
    {
        var builder_config_file_name = args.Length > 0 ? args[0] : "builder.conf";
        var builder_config_fi        = new FileInfo(builder_config_file_name);

        if (!builder_config_fi.Exists)
        {
            File.WriteAllText
            (
                builder_config_fi.FullName,
                """
                configuration: Debug
                output: build

                [projects]
                """
            );
        }

        var configFileLines = File.ReadAllLines(builder_config_fi.FullName);
        ParseConfigFile(configFileLines);

        using (var opt = new NotImportantConsoleOptions())
        {            
            Console.WriteLine($"Builder started at {getTimeString(DateTime.Now)}");
            Console.WriteLine($"Config file: '{builder_config_fi.FullName}");
            Console.WriteLine($"Projects count to build: {configuration["projects"].Values.Count}");

            var output_for_configuration = new string[] {"configuration", "output"};
            foreach (var confOptName in output_for_configuration)
            {
                if (!configuration.ContainsKey(confOptName))
                    return (int) ErrorCode.InvalidConfigFile;

                Console.Write($"{confOptName}: '{configuration[confOptName]}'; ");
            }
        }

        // ---------------- Устанавливаем обработчики ошибок ----------------

        SetErrorHandlers();

        // ---------------- Компиляция ----------------

        var ec = MainBuild();
        if (ec.resultCode != ErrorCode.Success)
        {
            Console.Error.WriteLine($"{getTimeString(DateTime.Now)}. Error during build");
            return (int) ec.resultCode;
        }

        if (!ec.WillTests)
            return (int) ec.resultCode;


        Console.WriteLine($"Tests started at {getTimeString(DateTime.Now)}");

        // ---------------- Тесты ----------------
        ec.resultCode = MainTests();
        if (ec.resultCode != ErrorCode.Success)
        {
            Console.Error.WriteLine($"{getTimeString(DateTime.Now)}. Error during tests");
            return (int) ec.resultCode;
        }

        using (var opt = new NotErrorConsoleOptions())
        {
            Console.Write($"Builder successfully ended at {getTimeString(DateTime.Now)}");
        }

        return (int) ErrorCode.Success;
    }

    public enum ErrorCode
    {
        /// <summary>Успех</summary>
        Success = 0,
        
        /// <summary>Ошибка, которая неверно преобразуется в ErrorCode</summary>
        Unknown = -1,

        /// <summary>Наличие файла builder.lock - это означает, что билд был неуспешным</summary>
        Builder_Lock = 1,

        /// <summary>Не удалось собрать какой-то из проектов</summary>
        DotnetError = 2,

        /// <summary>Неверный файл конфигурации builder.conf</summary>
        InvalidConfigFile = 3,
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

        end_build_for_project_event += end_build_for_project_Handler;
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

    public static void end_build_for_project_Handler(ErrorCode code, DirectoryInfo updatedFile)
    {
        var ProjectName = updatedFile.FullName;

        var cd = Directory.GetCurrentDirectory();
        if (ProjectName.StartsWith(cd))
        {
            ProjectName = "." + Path.Combine("./", ProjectName.Substring(cd.Length));
        }

        using (var opt = new NotImportantConsoleOptions())
        {
            if (code == ErrorCode.Success)
                Console.Error.Write($"Project builded: '{ProjectName}'");
            else
                Console.Error.Write($"Error '{code}' occured during build for project: '{ProjectName}'");
        }
    }
}
