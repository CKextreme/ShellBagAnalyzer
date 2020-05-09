using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ShellBag.Library.ShellBags.ShellItems.Others;

namespace ShellBag.Library.ShellBags.ShellItems
{
    public class FileEntryShellItem : ShellItem
    {
        public new FileEntryClassType ClassType { get; private set; }
        public DateTime ModificationDateTime { get; private set; }
        public string PrimaryName { get; private set; } = null!;
        // ExtensionBlock Getter


        public FileEntryShellItem(ushort size, byte type, byte[] data) : base(size, type, data)
        {
            AnalyzeData();
        }

        protected sealed override void AnalyzeData()
        {
            ParseClassType();
            ParseModificationDateTime(); 
            ParsePrimaryName();

            //ParseExtensionBlock();
            //ParseSecondaryName();
        }


        private void ParseClassType()
        {
            // Skip Size (2 Bytes)
            var classType = Data.Skip(2).First();
            ClassType = (FileEntryClassType) classType;
        }

        /// <summary>
        /// Bla Blub
        /// <para>
        /// Fat32 DateTime - 16 Bit for Time, followed by 16 Bit for Date
        /// </para>
        /// </summary>
        /// <param name="fat32_datetime_raw">4 Byte (32 Bit) larges byte-array as timestamp</param>
        /// <returns><see cref="DateTime"/></returns>
        //private DateTime ParseModificationDateTime(byte[] fat32_datetime_raw)
        private void ParseModificationDateTime()
        {
            ModificationDateTime = new DateTime();

            // skip Size (2 bytes), ClassType (1 byte) + 5 unknown bytes = 8 Bytes to skip
            // Reverse for Little Endian (lowest bit 0 on the right side)
            //var modificationData = Data.Skip(8).Take(4);
            //var time_raw = modificationData.Take(2).Reverse();
            //var date_raw = modificationData.Skip(2).Take(2).Reverse();

            // FAT32 Specification (16 bits for Time/ 16 bits for Date):
            // 0-4 Bits (5 Bits) = Seconds (2x multiply)
            // 5-10 (10 Bits) = minutes
            // 11-15 (5 Bits) = hours
            // ---
            // 0-4 (5 Bits) = day
            // 5-8 (4 Bits) = month
            // 9-15 (7 Bits) = year (since 1980)

            //ushort time = BitConverter.ToUInt16(modificationData.Take(2).ToArray(),0);
            //ushort date = BitConverter.ToUInt16(modificationData.Skip(2).Take(2).ToArray(),0);
            //var date = BitConverter.ToUInt16(modificationData.Skip(2).Take(2).ToArray(),0);

            var rawbytes = Data.Skip(8).Take(4).ToArray();

            // take the first 2 Bytes ( 16 Bit)
            var date = BitConverter.ToUInt16(rawbytes, 0);
            // take the remaining 2 Bytes
            var time = BitConverter.ToUInt16(rawbytes, 2);




            //int hour = (time >> 11);
            //int minute = (time & 0b_00000_111111_00000) >> 5;
            //int second = (time & 0b_00000_000000_11111) * 2; // *2 = << 1

            //int year = (date >> 9) + 1980; // since 1980
            //int month = (date & 0b_0000000_1111_00000) >> 5;
            //int day = (date & 0b_0000000_0000_11111);

            //var datetime = new DateTime(year,month,day,hour,minute,second);

            //Console.WriteLine(datetime);

            //return datetime;


            // ---------------------

            //DateTimeOffset? thedate = null;

            //var somedate = BitConverter.ToUInt16(rawBytes, 0);

            //var day = (somedate & 0x1f);
            //var month = (somedate & 0x1e0) >> 5;
            //var year = ((somedate & 0xfe00) >> 9) + 1980;

            //var sometime = BitConverter.ToUInt16(rawBytes, 2);

            //var someTimeBinary = Convert.ToString(sometime, 2).PadLeft(16, '0');

            //var chunkt1 = someTimeBinary.Substring(0, 5);
            //var chunkt2 = someTimeBinary.Substring(5, 6);
            //var chunkt3 = someTimeBinary.Substring(11, 5);

            //var hour = (int) Convert.ToUInt32(chunkt1, 2);
            //var minute = (int) Convert.ToUInt32(chunkt2, 2);
            //var seconds = (int) Convert.ToUInt32(chunkt3, 2)*2;

            //var dt = new DateTime(year, month, day, hour, minute, seconds, DateTimeKind.Utc);
            //var dtoffset = new DateTimeOffset(dt, TimeZone.CurrentTimeZone.GetUtcOffset(dt));

            // ---------------------



            // Example BA:
            // hours, minutes, seconds
            //const ushort time = 0b_01100_011110_00011;
            // year, month, day
            //const ushort date = 0b_0100111_0111_00101;

            //var intDatetime = BitConverter.ToInt32(modificationData.ToArray(),0);

            //Console.WriteLine(intDatetime);

            //var testByte = new byte[] {0x5b, 0x49, 0x45, 0x2f};
            //Console.WriteLine(BitConverter.ToInt32(modificationData.ToArray(),0));
            //var time =  DateTime.FromFileTime(BitConverter.ToInt32(testByte,0));
            //Console.WriteLine(time);
            //var time = DateTime.FromFileTime(BitConverter.ToInt32(modificationData.ToArray(),0));

            //Console.WriteLine(intDatetime);

            //var date = new DateTime(intDatetime);

            //Console.WriteLine(date);
        }

        

        private void ParsePrimaryName()
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

            PrimaryName = System.Text.Encoding.UTF8.GetString(name.ToArray());
        }

        private void ParseExtensionBlock()
        {
            //ParseCreationDateTime();
            //ParseLastAccessDateTime();
        }

        private void ParseSecondaryName()
        {

        }
    }
}
