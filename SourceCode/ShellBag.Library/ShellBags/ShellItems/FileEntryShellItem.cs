using ShellBag.Library.ShellBags.ShellItems.ExtensionBlocks;
using ShellBag.Library.ShellBags.ShellItems.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShellBag.Library.ShellBags.Logging;

namespace ShellBag.Library.ShellBags.ShellItems
{
    /// <summary>
    /// 
    /// </summary>
    public class FileEntryShellItem : ShellItem
    {
        /// <summary>
        /// 
        /// </summary>
        public new FileEntryClassType ClassType { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModificationDateTime { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string PrimaryName { get; private set; } = null!;
        /// <summary>
        /// 
        /// </summary>
        public ExtensionBlock BeefType { get; private set; } = null!;
        /// <summary>
        /// Constructor
        /// </summary>
        public FileEntryShellItem(IEnumerable<byte> rawData) : base(rawData)
        {
            AnalyzeData();
        }
        /// <summary>
        /// Sealed AnalyzeData method.
        /// </summary>
        protected sealed override void AnalyzeData()
        {
            ParseClassType();
            // skip Size (2 bytes) + ClassType (1 byte) + 5 unknown bytes = 8 bytes to skip
            ParseModificationDateTime(RawData.Skip(8).Take(4).ToArray());
            // get the variable offset from the primary name
            var variableOffset = ParsePrimaryName();
            ParseExtensionBlock(variableOffset);
        }
        /// <summary>
        /// Cast the class type to the specific <see cref="FileEntryClassType"/>.
        /// </summary>
        private void ParseClassType()
        {
            // Skip Size (2 Bytes)
            var classType = RawData.Skip(2).First();
            ClassType = (FileEntryClassType)classType;
        }

        /// <summary>
        /// Parse the Modification DateTime from the <see cref="FileEntryShellItem"/>.
        /// <para>
        /// Source: <see href="http://www.ntfs.com/exfat-time-stamp.htm"/>
        /// </para>
        /// </summary>
        private void ParseModificationDateTime(byte[] datetimeBytes)
        {
            try
            {
                ModificationDateTime = Utils.ParseDateTime(datetimeBytes);
            }
#pragma warning disable CA1031 // Keine allgemeinen Ausnahmetypen abfangen
            catch (Exception e)
#pragma warning restore CA1031 // Keine allgemeinen Ausnahmetypen abfangen
            {
                ConsoleLogger.Log(e);
                ModificationDateTime = null;
            }
        }
        /// <summary>
        /// Parse the primary name from the <see cref="FileEntryShellItem"/>.
        /// </summary>
        /// <returns></returns>
        private int ParsePrimaryName()
        {
            // skip 2 bytes after modification datetime for unknown bytes
            var data = RawData.Skip(14);
            List<byte> name = new List<byte>();

            foreach (var b in data)
            {
                if (b != 0x00)
                {
                    name.Add(b);
                    continue;
                }
                break;
            }

            // PrimaryName could be Empty
            PrimaryName = System.Text.Encoding.UTF8.GetString(name.ToArray());
            return name.Count;
        }
        /// <summary>
        /// Parse the remaining data and analyze the specific beef type.
        /// </summary>
        private void ParseExtensionBlock(int nextskip)
        {
            // let BeefType be null if primaryname empty.
            // In development found some FileEntryShellItems without PrimaryName + no ExtensionBlock, but with SecondaryName (crazy)
            if (nextskip == 0) return;

            // offset from previous data and primaryname length
            var preOffset = 14 + nextskip;
            var data = RawData.Skip(preOffset).ToList();

            // skip null bytes after primary name (1 or 2 bytes possible)
            var skip = 0;
            foreach (var b in data)
            {
                if (b == 0x00)
                {
                    skip++;
                    continue;
                }
                break;
            }

            // get signature and create specific extension block
            var signature = (Signature)BitConverter.ToUInt16(data.Skip(skip + 4).Take(4).ToArray(), 0);
            BeefType = signature switch
            {
                Signature.Beef0004 => new Beef0004(data.Skip(skip), PrimaryName),
                _ => new BeefUnknown(data.Skip(skip))
            };
        }

        /// <summary>
        /// Custom ToString method.
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            // ExtensionBlock is missing here
            sb.Append($" , ModificationDateTime: {ModificationDateTime} , PrimaryName: {PrimaryName}");
            return sb.ToString();
        }
    }
}
