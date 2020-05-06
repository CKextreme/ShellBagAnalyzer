using System;

namespace ShellBag.Library.ShellBags.ShellItems
{
    internal interface IShellItem
    {
        ushort Size { get; }
        byte ClassType { get; }
        void AnalyzeData();
    }
}
