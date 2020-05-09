using System;
using System.Linq;
using ShellBag.Library.ShellBags.ShellItems.Others;

namespace ShellBag.Library.ShellBags.ShellItems
{
    public class RootFolderShellItem : ShellItem
    {
        /// <summary>
        /// 
        /// </summary>
        public SortIndex SortIndex { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid GlobalId { get; private set; }

        public RootFolderShellItem(ushort size, byte type, byte[] data) : base(size, type, data)
        {
            AnalyzeData();
        }

        protected sealed override void AnalyzeData()
        {
            ParseSortIndex();
            ParseGuid();
        }

        private void ParseSortIndex()
        {
            // Skip Size + ClassType
            var type = Data.Skip(3).First();
            SortIndex = (SortIndex)type;
        }

        private void ParseGuid()
        {
            // Skip Size + ClassType + SortIndex / Take 16 Bytes for GUID
            var data = Data.Skip(4).Take(16);
            // platform independent (little or big endian)?
            // Source: https://stackoverflow.com/a/37711583 / https://referencesource.microsoft.com/#mscorlib/system/guid.cs,2f5155129905e1a3
            GlobalId = new Guid(data.ToArray());
        }
    }
}
