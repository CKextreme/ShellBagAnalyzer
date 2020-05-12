namespace ShellBag.Library.ShellBags.ShellItems.ExtensionBlocks
{
    /// <summary>
    /// <see cref="IExtensionBlock"/>
    /// </summary>
    public interface IExtensionBlock
    {
        /// <summary>
        /// Contains the size for the extension block.
        /// </summary>
        ushort Size { get; }
        /// <summary>
        /// Specific version number for the extension block.
        /// </summary>
        ushort Version { get; }
        /// <summary>
        /// Specific signature which represents the extension block (beef-type).
        /// </summary>
        Signature Signature { get; }
    }
}
