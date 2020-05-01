namespace ShellBag.Library.ShellBags.ShellItems
{
    internal interface IShellItem
    {
        byte Size { get; }
        byte ClassType { get; }
    }
}
