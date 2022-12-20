namespace utils;
public static class DateTimeStrings
{
    [Flags]
    public enum getTimeString_options: byte { Date = 1, Time = 2 }
    public static string getTimeString
                                    (
                                        DateTime time,
                                        getTimeString_options opts = getTimeString_options.Date | getTimeString_options.Time
                                    )
    {
        String str = "";
        if (opts == getTimeString_options.Date)
            str = time.ToString("dd MMM yyyy");
        if (opts == getTimeString_options.Time)
            str = time.ToString("HH:mm");
        else
            str = time.ToString("HH:mm dd MMM yyyy");

        return str;
    }
}
