using System;

namespace ShellBag.Library.ShellBags.Logging
{
    /// <summary>
    /// Enumeration for well-known Log levels.
    /// </summary>
    [Flags]
    public enum LogLevels
    {
        None = 0,
        Debug = 1,
        Info = 2,
        Warning = 4,
        Error = 8
    }
}
