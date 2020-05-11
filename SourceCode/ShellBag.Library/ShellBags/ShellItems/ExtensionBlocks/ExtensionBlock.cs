namespace ShellBag.Library.ShellBags.ShellItems.ExtensionBlocks
{
    public abstract class ExtensionBlock : IExtensionBlock
    { 
        public ushort Size { get; }
        public ushort Version { get; }
        public Signature Signature { get; }

        public ExtensionBlock(ushort size, ushort version, Signature signature)
        {
            Size = size;
            Version = version;
            Signature = signature;
        }
    }
}
