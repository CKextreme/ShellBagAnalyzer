using System;
using System.Linq;

namespace ShellBag.Library.ShellBags.ShellItems
{
    public class VolumeShellItem : ShellItem
    {
        public VolumeShellItem(ushort size, byte type, byte[] data): base(size, type, data)
        {
            AnalyzeData();
        }

        public override void AnalyzeData()
        {
            const int skip = 3; // offset from size, class type
            var take = Size - skip;
            var name = Data.Skip(skip).Take(take);

            //Console.WriteLine(System.Text.Encoding.UTF8.GetString(name.ToArray()));
        }
    }
}
