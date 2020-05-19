using System;
using System.Collections.Generic;
using System.Linq;

namespace ShellBag.Library.ShellBags.ShellItems.ExtensionBlocks
{
    public abstract class ExtensionBlock : IExtensionBlock
    {
        public IEnumerable<byte> BeefData { get; }
        public ushort Size { get; }
        public ushort Version { get; }
        public Signature Signature { get; }
        /// <summary>
        /// Constructor.
        /// </summary>
        protected ExtensionBlock(IEnumerable<byte> rawBeefData)
        {
            BeefData = rawBeefData;

            Size = BitConverter.ToUInt16(BeefData.Take(2).ToArray(), 0);
            Version = BitConverter.ToUInt16(BeefData.Skip(2).Take(2).ToArray(), 0);
            Signature = (Signature)BitConverter.ToUInt16(BeefData.Skip(4).Take(4).ToArray(), 0);
        }

        /// <summary>
        /// Method which is analyzing the passed raw data
        /// </summary>
        protected abstract void AnalyzeData();
    }
}
