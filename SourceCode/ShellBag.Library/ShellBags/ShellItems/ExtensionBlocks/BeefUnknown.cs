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
        public BeefUnknown(ushort size, ushort version, Signature signature) : base(size, version, signature)
        {
            ConsoleLogger.Log(LogLevels.Debug, $"{nameof(BeefUnknown)} found - ClassType: {string.Format(CultureInfo.CurrentCulture, "0x{0:X4}", signature)}");
        }

        protected override void AnalyzeData()
        {
            throw new System.NotSupportedException();
        }
    }
}
