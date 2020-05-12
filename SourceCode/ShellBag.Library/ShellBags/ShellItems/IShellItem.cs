using System.Collections.Generic;

namespace ShellBag.Library.ShellBags.ShellItems
{
    /// <summary>
    /// <see cref="IShellItem"/> interface
    /// </summary>
    public interface IShellItem
    {
        /// <summary>
        /// Returns the Size of the raw data read from the registry. Basically offset 0x00 (2 bytes).
        /// </summary>
        ushort Size { get; }
        /// <summary>
        /// Returns the specific type of the shell item. Basically offset 0x02 (1 byte).
        /// </summary>
        byte ClassType { get; }
        // CA1819: Properties should not return arrays: byte[] Data { get; }
        /// <summary>
        /// Represents the raw data read from the registry.
        /// </summary>
        IEnumerable<byte> Data { get; }
    }
}
