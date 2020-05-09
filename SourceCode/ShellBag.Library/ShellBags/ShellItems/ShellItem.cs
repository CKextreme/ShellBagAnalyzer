namespace ShellBag.Library.ShellBags.ShellItems
{
    public abstract class ShellItem : IShellItem
    {
        public ushort Size { get; }
        public byte ClassType { get; }
        public byte[] Data { get; }

        public ShellItem(ushort size, byte type, byte[] data)
        {
            Size = size;
            ClassType = type;
            Data = data;
        }

        protected abstract void AnalyzeData();
    }
}
