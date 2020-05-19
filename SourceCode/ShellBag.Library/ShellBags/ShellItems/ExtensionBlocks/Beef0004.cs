using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ShellBag.Library.ShellBags.ShellItems.ExtensionBlocks
{
    /// <summary>
    /// Special Beef-Type with the signature 0x0400efbe (beef0004)
    /// </summary>
    public class Beef0004 : ExtensionBlock
    {
        /// <summary>
        /// A secondary name (full long name) for the <see cref="FileEntryShellItem"/>.PrimaryName property.
        /// <para>
        /// <seealso cref="FileEntryShellItem"/>
        /// </para>
        /// </summary>
        public string SecondaryName { get; private set; } = null!;
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreationDateTime { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastAccessDateTime { get; private set; }

        private readonly string _primaryName;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rawBeefData"></param>
        /// <param name="primaryname"></param>
        public Beef0004(IEnumerable<byte> rawBeefData, string primaryname) : base(rawBeefData)
        {
            _primaryName = primaryname;
            AnalyzeData();
        }

        protected sealed override void AnalyzeData()
        {
            ParseCreationDateTime();
            ParseLastAccessDateTime();
            ParseSecondaryName();
        }

        private void ParseCreationDateTime()
        {
            const int skip = 8;

            try
            {
                CreationDateTime = Utils.ParseDateTime(BeefData.Skip(skip).Take(4).ToArray());
            }
#pragma warning disable CA1031
            catch
#pragma warning restore CA1031
            {
                CreationDateTime = null;
            }
        }

        private void ParseLastAccessDateTime()
        {
            if (CreationDateTime == null) return;

            const int skip = 12;
            try
            {
                LastAccessDateTime = Utils.ParseDateTime(BeefData.Skip(skip).Take(4).ToArray());
            }
#pragma warning disable CA1031
            catch
#pragma warning restore CA1031
            {
                LastAccessDateTime = null;
            }
        }

        private void ParseSecondaryName()
        {
            if (LastAccessDateTime == null) return;
            if (string.IsNullOrEmpty(_primaryName)) return;

            const int skip = 16;
            var data = BeefData.Skip(skip).ToList();

            // Dirty Code!
            
            // 1. Reverse Data and remove all end characters
            data.Reverse();
            var counter = 0;
            var cut = 0;
            foreach (var b in data)
            {
                if (b != 0x00)
                {
                    counter++;
                }

                if (counter >= 2)
                {
                    break;
                }
                cut++;
            }

            data.RemoveRange(0, cut);
            data.Reverse();

            // 2. Remove all null bytes (0x00)
            for (int i = 0; i < data.Count-1; i++)
            {
                if (data[i]==0x00)
                {
                    data.RemoveAt(i);
                }
            }

            // 2. Search for the first two equal char if possible
            var count = 0;
            for (var i = 0; i < data.Count-1; i++)
            {
                count = i;

                if (_primaryName.Length == 1)
                {
                    var compare = char.ToLower((char) data[i]) == char.ToLower(_primaryName[0]);
                    if (compare)
                    {
                        break;
                    }
                }
                else
                {
                    var compare = char.ToLower((char) data[i]) == char.ToLower(_primaryName[0]) && char.ToLower((char) data[i+1]) == char.ToLower(_primaryName[1]);
                    if (compare)
                    {
                        break;
                    } 
                }
            }

            data.RemoveRange(0, count);

            SecondaryName = System.Text.Encoding.UTF8.GetString(data.ToArray());
        }
    }
}
