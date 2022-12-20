using static utils.DateTimeStrings;

namespace builder;

class Program
{
    static void Main(string[] args)
    {
        var now = DateTime.Now;
        Console.WriteLine($"Builder started {getTimeString(now)}");
    }
    
}
