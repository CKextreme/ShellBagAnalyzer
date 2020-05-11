using ShellBag.Library.ShellBags.ShellItems.ExtensionBlocks;
using ShellBag.Library.ShellBags.ShellItems.Others;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShellBag.Library.ShellBags.ShellItems
{
    public class FileEntryShellItem : ShellItem
    {
        public new FileEntryClassType ClassType { get; private set; }
        public DateTime? ModificationDateTime { get; private set; }
        public string PrimaryName { get; private set; } = null!;
        public ExtensionBlock BeefType { get; private set; } = null!;

        public FileEntryShellItem(ushort size, byte type, byte[] data) : base(size, type, data)
        {
            AnalyzeData();
        }

        protected sealed override void AnalyzeData()
        {
            ParseClassType();
            // skip Size (2 bytes) + ClassType (1 byte) + 5 unknown bytes = 8 bytes to skip
            ParseModificationDateTime(Data.Skip(8).Take(4).ToArray());
            // get the variable offset from the primary name
            var variableOffset = ParsePrimaryName();
            ParseExtensionBlock(variableOffset);
        }

        private void ParseClassType()
        {
            // Skip Size (2 Bytes)
            var classType = Data.Skip(2).First();
            ClassType = (FileEntryClassType)classType;
        }

        /// <summary>
        /// Parse the Modification DateTime from the <see cref="FileEntryShellItem"/>.
        /// <para>
        /// Source: <see href="http://www.ntfs.com/exfat-time-stamp.htm"/>
        /// </para>
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Literale nicht als lokalisierte Parameter übergeben", Justification = "<Ausstehend>")]
        private void ParseModificationDateTime(byte[] datetimeBytes, bool isLittleEndian = true)
        {
            if (!isLittleEndian)
            {
                throw new NotImplementedException("BigEndian byte-array is not supported yet!");
            }
            if (datetimeBytes.Length != 4)
            {
                throw new ArgumentException("The value must have a size of 4 bytes (32 bit)!", nameof(datetimeBytes));
            }

            if (datetimeBytes[0] == 0x00 && datetimeBytes[1] == 0x00 && datetimeBytes[2] == 0x00 && datetimeBytes[3] == 0x00)
            {
                ModificationDateTime = null;
                return;
            }

            // NTFS Timestamp Format (2 Byte Time, 2 Byte Date):
            // Time (UTC):
            // 0-4 Bits (Size 5) = Seconds (2 second interval, resolution) - multiply with 2!
            // 5-10 (Size 6) = Minutes
            // 11-15 (Size 5) = Hour
            // Date:
            // 16-20 (Size 5) = Day
            // 21-24 (Size 4) = Month
            // 25-31 (Size 7) = Year (as offset from 1980, 0 represents 1980)
            // = seconds, minutes, hour, day, month, year

            // Demo Little Endian: 0xE5_4E_C4_63
            // Big Endian: 0x63_C4_4E_E5

            //datetimeBytes = new byte[] { 0xE5, 0x4E, 0xC4, 0x63 };

            var date = BitConverter.ToUInt16(datetimeBytes, 0);
            var time = BitConverter.ToUInt16(datetimeBytes, 2);

            // if Little Endian - order is reversed by BitConverter-class
            // year, month, day
            var year = (date >> 9) + 1980; // since 1980
            var month = (date & 0b_0000000_1111_00000) >> 5;
            var day = (date & 0b_0000000_0000_11111);

            // hour, minutes, seconds
            var hour = time >> 11;
            var minutes = (time & 0b_00000_111111_00000) >> 5;
            var seconds = (time & 0b_00000_000000_11111) * 2; // *2 = << 1;

            try
            {
                ModificationDateTime = new DateTime(year, month, day, hour, minutes, seconds, DateTimeKind.Utc);
            }
            catch (Exception e)
            {
                ModificationDateTime = null;
            }
            
        }

        private int ParsePrimaryName()
        {
            // skip 2 bytes after modification datetime for unknown bytes
            var data = Data.Skip(14);
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

        private void ParseExtensionBlock(int nextskip)
        {
            // let beeftype be null if primaryname empty.
            // In development found some FileEntryShellItems without PrimaryName + no ExtensionBlock, but with SecondaryName (crazy)
            if (nextskip == 0) return;

            // offset from previous data and primaryname length
            var preOffset = 14 + nextskip;
            var data = Data.Skip(preOffset).ToList();

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

            // Read the ExtensionBlock part
            var size = BitConverter.ToUInt16(data.Skip(skip).Take(2).ToArray(), 0);
            var version = BitConverter.ToUInt16(data.Skip(skip + 2).Take(2).ToArray(), 0);
            var signature = (Signature)BitConverter.ToUInt16(data.Skip(skip + 4).Take(4).ToArray(), 0);

            // ExtensionBlock contains the size of the remaining array and data & not only its own size
            BeefType = signature switch
            {
                Signature.Beef0004 => new Beef0004(size, version, signature),
                _ => new BeefUnknown(size, version, signature)
            };
        }
    }
}
