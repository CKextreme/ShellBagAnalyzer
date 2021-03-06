﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShellBag.Library.ShellBags.ShellItems.Others;

namespace ShellBag.Library.ShellBags.ShellItems
{
    /// <summary>
    /// 
    /// </summary>
    public class RootFolderShellItem : ShellItem
    {
        /// <summary>
        /// Special Index for Windows objects.
        /// </summary>
        public SortIndex SortIndex { get; private set; }
        /// <summary>
        /// Global Unique ID
        /// </summary>
        public Guid GlobalId { get; private set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public RootFolderShellItem(IEnumerable<byte> rawData) : base(rawData)
        {
            AnalyzeData();
        }
        /// <summary>
        /// Sealed AnalyzeData method.
        /// </summary>
        protected sealed override void AnalyzeData()
        {
            ParseSortIndex();
            ParseGuid();
        }
        /// <summary>
        /// Parse the <see cref="SortIndex"/>.
        /// </summary>
        private void ParseSortIndex()
        {
            // Skip Size + ClassType
            var type = RawData.Skip(3).First();
            SortIndex = (SortIndex)type;
        }
        /// <summary>
        /// Parse the <see cref="Guid"/>.
        /// <para>Source 1: <seealso href="https://stackoverflow.com/a/37711583"/></para>
        /// <para>Source 2: <seealso href="https://referencesource.microsoft.com/#mscorlib/system/guid.cs,2f5155129905e1a3"/></para>
        /// </summary>
        private void ParseGuid()
        {
            // Skip Size + ClassType + SortIndex / Take 16 Bytes for GUID
            var data = RawData.Skip(4).Take(16);
            // platform independent (little or big endian)?

            try
            {
                GlobalId = new Guid(data.ToArray());
            }
#pragma warning disable CA1031
            catch
#pragma warning restore CA1031
            {

                // Guid = null;
            }
            
        }

        /// <summary>
        /// Custom ToString method.
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.Append($" , SortIndex: {SortIndex} , GUID: {GlobalId}");
            return sb.ToString();
        }
    }
}
