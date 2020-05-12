using System;
using System.Collections.Generic;
using System.Linq;
using ShellBag.Library.ShellBags.Logging;

namespace ShellBag.Library.ShellBags.ShellItems
{
    /// <summary>
    /// Specific shell item type containing data about drives.
    /// </summary>
    public class VolumeShellItem : ShellItem
    {
        /// <summary>
        /// If not empty the property contains the drive letter found in <see cref="VolumeShellItem"/>.
        /// </summary>
        public string DriveLetter { get; private set; } = null!;
        /// <summary>
        /// Constructor
        /// </summary>
        public VolumeShellItem(ushort size, byte type, IEnumerable<byte> data): base(size, type, data)
        {
            AnalyzeData();
        }
        /// <summary>
        /// Sealed AnalyzeData method
        /// </summary>
        protected sealed override void AnalyzeData()
        {
            // offset from size (2 Bytes), class type (1 Byte)
            const int skip = 3;
            // size of the the drive letter string in bytes
            const int take = 3;

            switch (ClassType)
            {
                case 0x2f:
                    var name = Data.Skip(skip).Take(take);
                    DriveLetter = System.Text.Encoding.UTF8.GetString(name.ToArray());
                    break;
                default:
                    Logging.ConsoleLogger.Log(LogLevels.Debug, $"DriveLetter: {BitConverter.ToString(Data.ToArray())}");
                    break;
            }
        }
    }
}
