using System;
using System.Linq;

namespace ShellBag.Library
{
    public static class Utils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Literale nicht als lokalisierte Parameter übergeben", Justification = "Library only english")]
        public static DateTime ParseDateTime(byte[] byteArray)
        {
            if (byteArray == null)
            {
                throw new ArgumentNullException(nameof(byteArray), "Cannot be null!");
            }
            if (byteArray.Length != 4)
            {
                throw new ArgumentException("The value must have a size of 4 bytes (32 bit)!", nameof(byteArray));
            }
            if (byteArray[0] == 0x00 && byteArray[1] == 0x00 && byteArray[2] == 0x00 && byteArray[3] == 0x00)
            {
                throw new ArgumentException("The array cannot contain only null bytes!", nameof(byteArray));
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

            // Demo Little Endian: 0xE5_4E_C4_63 (big endian: 0x63_C4_4E_E5)

            var date = BitConverter.ToUInt16(byteArray, 0);
            var time = BitConverter.ToUInt16(byteArray, 2);

            // if Little Endian - order is reversed by BitConverter-class
            // year, month, day
            var year = (date >> 9) + 1980; // since 1980
            var month = (date & 0b_0000000_1111_00000) >> 5;
            var day = (date & 0b_0000000_0000_11111);

            // hour, minutes, seconds
            var hour = time >> 11;
            var minutes = (time & 0b_00000_111111_00000) >> 5;
            var seconds = (time & 0b_00000_000000_11111) * 2; // *2 = << 1;

            // throws possible exception
            return new DateTime(year, month, day, hour, minutes, seconds, DateTimeKind.Utc);
        }
    }
}
