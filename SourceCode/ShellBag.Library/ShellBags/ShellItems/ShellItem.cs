namespace ShellBag.Library.ShellBags.ShellItems
{
    internal abstract class ShellItem : IShellItem
    {
        public byte Size { get; }
        public byte ClassType { get; }

        internal ShellItem(byte size, byte type)
        {
            Size = size;
            ClassType = type;
        }
    }
}
