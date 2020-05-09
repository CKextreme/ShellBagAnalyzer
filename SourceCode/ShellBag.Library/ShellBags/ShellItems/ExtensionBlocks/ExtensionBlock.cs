namespace ShellBag.Library.ShellBags.ShellItems.ExtensionBlocks
{
    public abstract class ExtensionBlock : IExtensionBlock
    {
        public ushort Size { get; }
        public byte Version { get; }
        public Signature Signature { get; }
    }
}
