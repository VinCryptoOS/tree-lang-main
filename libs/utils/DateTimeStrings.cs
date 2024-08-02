namespace utils;
public static class DateTimeStrings
{
    [Flags]
    /// <summary>Флаги для функции getTimeString_options)</summary>
    public enum GetTimeString_options: byte { None = 0, Date = 1, Time = 2, Seconds = 4 }
    public static string GetTimeString
                                    (
                                        DateTime time,
                                        GetTimeString_options opts = GetTimeString_options.Date | GetTimeString_options.Time | GetTimeString_options.Seconds
                                    )
    {
        String format = "";

        if (opts.HasFlag(GetTimeString_options.Time))
        {
            if (opts.HasFlag(GetTimeString_options.Seconds))
                format += "HH:mm:ss";
            else
                format += "HH:mm";
        }

        if (opts.HasFlag(GetTimeString_options.Date))     // opts.HasFlag()
        {
            if (format.Length > 0)
                format += " ";

            format += "dd MMM yyyy";
        }


        return time.ToString(format);
    }

    public static string GetDateVersionString(DateTime time)
    {
        return time.ToString("yyyy.MM.dd.HHmm");
    }
}
