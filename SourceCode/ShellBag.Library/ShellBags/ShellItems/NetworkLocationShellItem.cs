using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShellBag.Library.ShellBags.ShellItems.Others;

namespace ShellBag.Library.ShellBags.ShellItems
{
    public class NetworkLocationShellItem : ShellItem
    {
        /// <summary>
        /// UNC Path
        /// </summary>
        public UncNames Names { get; set; }

        public NetworkLocationShellItem(ushort size, byte type, byte[] data) : base(size, type, data)
        {
            AnalyzeData();
        }

        public sealed override void AnalyzeData()
        {
            // Source: https://en.wikipedia.org/wiki/Control_character#Transmission_control
            // "The start of text character (STX) marked the end of the header, and the start of the textual part of a stream"
            // 0x0002 (ASCII Table)
            // 0x00 - mark es End of String (e.g. C - lang)

            const int skip = 5; // offset from size, class type, 2x unknown bytes
            var take = Size - skip - 2; // 2 = end of string mark (2 bytes)
            var name = Data.Skip(skip).Take(take);

            string temp = "";
            var list = new List<string>();
            foreach (var e in name)
            {
                if (e == 0x00 && !string.IsNullOrEmpty(temp))
                {
                    list.Add(temp);
                    temp = "";
                    continue;
                }
                temp += (char)e;
            }

            var path = list.First();
            var mn = list.ElementAt(1);
            var extra = list.Count > 2 ? list.ElementAt(2) : null;

            Names = new UncNames(path, mn, extra);
        }
    }
}
