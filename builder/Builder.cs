using System.Diagnostics;

namespace builder;

partial class Program
{
    public static ErrorCodes MainBuild()
    {
        if (!checkVersionOfBuildProgramm())
        {
            return ExecuteFullBuild();
        }

        return ErrorCodes.Success;
    }

    public static bool checkVersionOfBuildProgramm()
    {
        return false;
    }

    public delegate void Builder_Lock_Event();
    public event Builder_Lock_Event builder_lock_event = null;

    public static ErrorCodes ExecuteFullBuild()
    {
        if (File.Exists("builder.lock"))
        {
            
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
