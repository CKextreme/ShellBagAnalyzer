using System;

namespace ShellBag.Library.ShellBags.Logging
{
    public static class ConsoleLogger
    {
        public static void Log(LogLevels level, string message)
        {
            switch (level)
            {
                case LogLevels.None:
                    break;
                case LogLevels.Debug:
                    #if DEBUG
                    Console.WriteLine("Debug:" + message);           
                    #endif
                    break;
                case LogLevels.Info:
                    Console.WriteLine("Info: " + message);
                    break;
                case LogLevels.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Warning: " + message);
                    break;
                case LogLevels.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: " + message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level,null);
            }
        }

        public static void Log(Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex));
            }
            Log(LogLevels.Error, ex.ToString());
        }
    }
}
