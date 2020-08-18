using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


public static class Logger
{
    private static bool log_dateTime = false;
    public static void DateTimeLogging(bool p) { log_dateTime = p; }

    public static void Error(object log)
    {
        Log("Error", log, ConsoleColor.Red);
    }
    public static void Warning(object log)
    {
        Log("Warning", log, ConsoleColor.Yellow);
    }
    public static void Exception(object log)
    {
        Log("Exception", log, ConsoleColor.Magenta);
    }
    public static void Log(object log)
    {
        Log("Log", log);
    }
    public static void Log(string tag, object log)
    {
        Log(tag, log, ConsoleColor.Green);
    }
    public static void Log(string tag, object log, ConsoleColor tagColor = ConsoleColor.Green)
    { 
        var orgForegroundColor = Console.ForegroundColor;


        Console.ForegroundColor = ConsoleColor.White;
        if (log_dateTime)
        {
            Console.Write($"[{System.DateTime.Now.ToString("hh:mm:ss")}] ");
        }



        Console.ForegroundColor = tagColor;
        Console.Write($"[{tag}] ");



        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"{log}");



        Console.ForegroundColor = orgForegroundColor;
    } 
}
