using ShellBag.Library.ShellBags.ShellItems.Others;
using System.Collections.Generic;
using System.Linq;

namespace ShellBag.Library.ShellBags.ShellItems
{
    public class NetworkLocationShellItem : ShellItem
    {
        /// <summary>
        /// Field that holds a <see cref="UncNames"/>-class.
        /// </summary>
        public UncNames Names { get; private set; } = null!;

        public NetworkLocationShellItem(ushort size, byte type, byte[] data) : base(size, type, data)
        {
            AnalyzeData();
        }

        protected sealed override void AnalyzeData()
        {
            // offset from size, class type, 2x unknown bytes
            const int skip = 5; 
            // end of string mark (2 bytes)
            var take = Size - skip - 2;
            var name = Data.Skip(skip).Take(take);

            string temp = "";
            var list = new List<string>();
            // read string from byte-array
            foreach (var e in name)
            {
                // if e == '\0' (null byte) => end of string
                if (e == 0x00)
                {
                    // add string to list, reset temporary string and continue loop
                    list.Add(temp);
                    temp = "";
                    continue;
                }
                // convert byte to character
                temp += (char)e;
            }

            // extract the strings from the list and pass to the class
            var path = list.First();
            var network = list.ElementAt(1);
            var extra = list.Count > 2 ? list.ElementAt(2) : null;

            Names = new UncNames(path, network, extra);
        }
    }
}
