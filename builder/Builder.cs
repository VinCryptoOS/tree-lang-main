using System.Diagnostics;

namespace builder;

partial class Program
{
    public static (ErrorCodes resultCode, bool WillTests) MainBuild()
    {
        if (!checkVersionOfBuildProgramm())
        {
            return (ExecuteFullBuild(), false);
        }

        return (ErrorCodes.Success, true);
    }

    /// <summary>Проверяем, актуальна ли на нынешний момент программа билда</summary>
    /// <returns>true - если билдер актуален; иначе false</returns>
    public static bool checkVersionOfBuildProgramm()
    {
        var pathToFile = typeof(Program).Assembly.Location;
        // Console.WriteLine(pathToFile);

        var di    = new DirectoryInfo(Directory.GetCurrentDirectory());
        var fi    = new FileInfo(pathToFile);
        var last  = fi.LastWriteTimeUtc;
        var files = di.GetFiles("*.cs", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            if (file.LastWriteTimeUtc >= last)
            {
                // Console.WriteLine($"Update file found: {file.FullName}");
                updated_file_found_event?.Invoke(file);
                return false;
            }
        }

        return true;
    }

    public delegate void  Builder_Lock_Event      (FileInfo builder_lock_file);
    public delegate void  Updated_File_Found_Event(FileInfo updatedFile);

    public static   event Builder_Lock_Event?       builder_lock_event;
    public static   event Updated_File_Found_Event? updated_file_found_event;

    public static ErrorCodes ExecuteFullBuild()
    {
        var fi = new FileInfo("builder.lock");
        if (fi.Exists)
        {
            var b = builder_lock_event;
            if (b != null)
                b(fi);

            return ErrorCodes.Builder_Lock;
        }

        try
        {
            using var fh = File.Open("builder.lock", FileMode.CreateNew, FileAccess.Write, FileShare.None);

            var psi = Process.Start("bash", "build.sh");
            psi.WaitForExit();

            return (ErrorCodes) psi.ExitCode;
        }
        finally
        {
            File.Delete("builder.lock");
        }
    }
}
