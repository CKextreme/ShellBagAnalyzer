using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellBag.Library.ShellBags.ShellItems.ExtensionBlocks
{
    public class Beef0004 : ExtensionBlock
    {
        public string SecondaryName { get; private set; } = null!;
        public Beef0004(ushort size, ushort version, Signature signature) : base(size, version, signature)
        {

        }
    }
}
