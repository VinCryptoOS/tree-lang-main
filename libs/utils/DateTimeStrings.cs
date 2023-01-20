namespace utils;
public static class DateTimeStrings
{
    [Flags]
    /// <summary>Флаги для функции getTimeString_options)</summary>
    public enum getTimeString_options: byte { None = 0, Date = 1, Time = 2, Seconds = 4 }
    public static string getTimeString
                                    (
                                        DateTime time,
                                        getTimeString_options opts = getTimeString_options.Date | getTimeString_options.Time | getTimeString_options.Seconds
                                    )
    {
        String format = "";

        if (opts.HasFlag(getTimeString_options.Time))
        {
            if (opts.HasFlag(getTimeString_options.Seconds))
                format += "HH:mm:ss";
            else
                format += "HH:mm";
        }

        if (opts.HasFlag(getTimeString_options.Date))     // opts.HasFlag()
        {
            if (format.Length > 0)
                format += " ";

            format += "dd MMM yyyy";
        }


        return time.ToString(format);
    }

    public static string getDateVersionString(DateTime time)
    {
        return time.ToString("yyyy.MM.dd.HHmm");
    }
}
