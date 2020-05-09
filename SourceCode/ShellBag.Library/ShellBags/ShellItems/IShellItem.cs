namespace ShellBag.Library.ShellBags.ShellItems
{
    public interface IShellItem
    {
        ushort Size { get; }
        byte ClassType { get; }
        byte[] Data { get; }
    }
}
