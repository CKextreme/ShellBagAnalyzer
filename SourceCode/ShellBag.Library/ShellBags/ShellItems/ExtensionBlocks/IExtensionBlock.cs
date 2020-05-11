namespace ShellBag.Library.ShellBags.ShellItems.ExtensionBlocks
{
    public interface IExtensionBlock
    {
        ushort Size { get; }
        ushort Version { get; }
        Signature Signature { get; }
    }
}
