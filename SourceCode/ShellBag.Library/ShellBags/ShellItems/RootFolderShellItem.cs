using System;
using System.Linq;
using ShellBag.Library.ShellBags.ShellItems.Others;

namespace ShellBag.Library.ShellBags.ShellItems
{
    public class RootFolderShellItem : ShellItem
    {
        public SortIndex SortIndex { get; set; }
        public Guid Guid { get; set; }

        public RootFolderShellItem(ushort size, byte type, byte[] data) : base(size, type, data)
        {
            AnalyzeData();
        }

        public override void AnalyzeData()
        {
            ParseSortIndex();
            ParseGuid();
        }

        private void ParseSortIndex()
        {
            // Skip Size + ClassType
            var type = Data.Skip(3).First();
            // check if Value is in SortIndex
            var test = Enum.IsDefined(typeof(SortIndex), type);
            if (test)
            {
                SortIndex = (SortIndex)type;
            }
        }

        private void ParseGuid()
        {
            // Skip Size + ClassType + SortIndex / Take 16 Bytes for GUID
            var data = Data.Skip(4).Take(16);
            Guid = new Guid(data.ToArray());
        }
    }
}
