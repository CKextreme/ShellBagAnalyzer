using System;
using System.Linq;

namespace ShellBag.Library.ShellBags.ShellItems
{
    public class VolumeShellItem : ShellItem
    {
        public string DriveLetter { get; private set; } = null!;

        public VolumeShellItem(ushort size, byte type, byte[] data): base(size, type, data)
        {
            AnalyzeData();
        }

        protected sealed override void AnalyzeData()
        {
            // offset from size (2 Bytes), class type (1 Byte)
            const int skip = 3;
            // size of the the drive letter string in bytes
            const int take = 3;
            var name = Data.Skip(skip).Take(take);

            switch (ClassType)
            {
                case 0x2f:
                    DriveLetter = System.Text.Encoding.UTF8.GetString(name.ToArray());
                    break;

                #if !DEBUG
                default:
                    DriveLetter = BitConverter.ToString(Data);
                    break;
                #endif
            }
        }
    }
}
