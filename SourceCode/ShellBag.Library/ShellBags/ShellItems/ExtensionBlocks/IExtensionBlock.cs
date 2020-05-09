namespace ShellBag.Library.ShellBags.ShellItems.ExtensionBlocks
{
    public interface IExtensionBlock
    {
        ushort Size { get; }
        byte Version { get; }
        Signature Signature { get; }
    }
}
