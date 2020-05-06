namespace ShellBag.Library.ShellBags.ShellItems
{
    public class FileEntryShellItem : ShellItem
    {
        public FileEntryShellItem(ushort size, byte type, byte[] data) : base(size, type, data)
        {

        }

        public override void AnalyzeData()
        {
            throw new System.NotImplementedException();
        }
    }
}
