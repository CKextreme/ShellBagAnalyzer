namespace ShellBag.Library.ShellBags.ShellItems
{
    public class UnknownShellItem : ShellItem
    {
        public UnknownShellItem(ushort size, byte type, byte[] data) : base(size, type, data)
        {

        }

        protected override void AnalyzeData()
        {
            
        }
    }
}
