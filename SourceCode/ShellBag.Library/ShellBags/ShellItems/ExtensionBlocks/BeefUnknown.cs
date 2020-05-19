using System.Collections.Generic;
using System.Globalization;
using ShellBag.Library.ShellBags.Logging;

namespace ShellBag.Library.ShellBags.ShellItems.ExtensionBlocks
{
    /// <summary>
    /// Represents an unknown beef type (unknown signature).
    /// <para>
    /// <seealso cref="Signature"/>
    /// </para>
    /// </summary>
    public class BeefUnknown : ExtensionBlock
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BeefUnknown(IEnumerable<byte> rawBeefData) : base(rawBeefData)
        {
            ConsoleLogger.Log(LogLevels.Debug, $"{nameof(BeefUnknown)} found - ClassType: {string.Format(CultureInfo.CurrentCulture, "0x{0:X4}", Signature)}");
        }

        protected override void AnalyzeData()
        {
            throw new System.NotSupportedException();
        }
    }
}
