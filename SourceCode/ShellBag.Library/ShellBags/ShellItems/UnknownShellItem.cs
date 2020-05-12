using System;
using System.Collections.Generic;
using System.Globalization;
using ShellBag.Library.ShellBags.Logging;

namespace ShellBag.Library.ShellBags.ShellItems
{
    /// <summary>
    /// Special shell item for unknown types.
    /// </summary>
    public class UnknownShellItem : ShellItem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UnknownShellItem(ushort size, byte type, IEnumerable<byte> data) : base(size, type, data)
        {
            ConsoleLogger.Log(LogLevels.Debug, $"{nameof(UnknownShellItem)} found - ClassType: {string.Format(CultureInfo.CurrentCulture, "0x{0:X2}", ClassType)}");
        }
        /// <summary>
        /// Not used because no analyzing possible
        /// </summary>
        protected override void AnalyzeData()
        {
            throw new NotSupportedException();
        }
    }
}
