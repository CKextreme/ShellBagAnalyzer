using System.Collections.Generic;

namespace ShellBag.Library.ShellBags.ShellItems
{
    /// <summary>
    /// Abstract class with predefined method
    /// </summary>
    public abstract class ShellItem : IShellItem
    {
        public ushort Size { get; }
        public byte ClassType { get; }
        public IEnumerable<byte> Data { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        protected ShellItem(ushort size, byte type, IEnumerable<byte> data)
        {
            Size = size;
            ClassType = type;
            Data = data;
        }
        /// <summary>
        /// Method which is analyzing the passed raw data
        /// </summary>
        protected abstract void AnalyzeData();
    }
}
