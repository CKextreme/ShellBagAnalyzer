using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ShellBag.Library.ShellBags.ShellItems
{
    /// <summary>
    /// Abstract class with predefined method
    /// </summary>
    public abstract class ShellItem : IShellItem
    {
        public ushort Size { get; }
        public byte ClassType { get; }
        public IEnumerable<byte> RawData { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        protected ShellItem(IEnumerable<byte> rawData)
        {
            RawData = rawData;
            Size = BitConverter.ToUInt16(RawData.Take(2).ToArray(), 0);
            ClassType = RawData.Skip(2).Take(1).First();
        }
        /// <summary>
        /// Method which is analyzing the passed raw data
        /// </summary>
        protected abstract void AnalyzeData();

        /// <summary>
        /// Custom ToString method.
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Size: {0} , ClassType: {1} , RawData: {BitConverter.ToString(RawData.ToArray())}");
            return sb.ToString();
        }
    }
}
