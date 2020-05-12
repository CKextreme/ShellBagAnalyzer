using System;

namespace ShellBag.Library.ShellBags.Logging
{
    /// <summary>
    /// Helper class for logging tasks to the console.
    /// </summary>
    public static class ConsoleLogger
    {
        /// <summary>
        /// Log message to the console with specific color based on the Level.
        /// </summary>
        public static void Log(LogLevels level, string message)
        {
            switch (level)
            {
                case LogLevels.None:
                    break;
                case LogLevels.Debug:
                    #if DEBUG
                    Console.WriteLine("Debug: " + message);           
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
        /// <summary>
        /// Error logger as short version.
        /// </summary>
        /// <param name="ex"></param>
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
