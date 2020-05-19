using System;
using ShellBag.Library.ShellBags.ShellItems.Others;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShellBag.Library.ShellBags.ShellItems
{
    /// <summary>
    /// Specific shell item contains network informations.
    /// </summary>
    public class NetworkLocationShellItem : ShellItem
    {
        /// <summary>
        /// Field that holds a <see cref="UncNames"/>-class.
        /// </summary>
        public UncNames Names { get; private set; } = null!;
        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkLocationShellItem(IEnumerable<byte> rawData) : base(rawData)
        {
            AnalyzeData();
        }
        /// <summary>
        /// Sealed AnalyzeData method
        /// </summary>
        protected sealed override void AnalyzeData()
        {
            // offset from size, class type, 2x unknown bytes
            const int skip = 5; 
            // end of string mark (2 bytes)
            var take = Size - skip - 2;
            var name = RawData.Skip(skip).Take(take);

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
            var extra = list.Count > 2 ? list.ElementAt(2) : string.Empty;

            Names = new UncNames(path, network, extra);
        }

        /// <summary>
        /// Custom ToString method.
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.Append($" , UncPath: {Names.UncPath} ({Names.MicrosoftNetwork}) , Description: {Names.Description}");
            return sb.ToString();
        }
    }
}
