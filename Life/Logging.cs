using System;
using System.Collections.Generic;
using System.Text;

namespace Life
{
    //This class is used to print useful console messages. Borrowed from part 1 solution
    static class Logging
    {
        private static string FormattedTime
        {
            get { return DateTime.Now.ToString("[hh:mm:ss:fff]"); }
        }

        public static void Success(string message)
        {
            Message(message, "Success", ConsoleColor.Green);
        }

        public static void Warning(string message)
        {
            Message(message, "Warning", ConsoleColor.Yellow);
        }

        public static void Error(string message)
        {
            Message(message, "Error", ConsoleColor.Red);
        }

        public static void Message(string message, string prefix = null, 
            ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"{FormattedTime}{(prefix != null ? $" {prefix}: " : " ")}{message}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
