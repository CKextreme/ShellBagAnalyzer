using System;
using System.Collections.Generic;
using System.Linq;
using ShellBag.Library.ShellBags.ShellItems;

namespace ShellBag.Library.ShellBags
{
    /// <summary>
    /// Abgeleitete Klasse von <see cref="Dictionary{TKey,TValue}"/>, welche die Knoten und deren Kindknoten speichert.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2237:ISerializable-Typen mit \"serializable\" markieren", Justification = "Don't want to add serialization.")]
    public class ShellBagNode : Dictionary<int, ShellBagNode>
    {
        //private static int counter = 0;

        /// <summary>
        /// Ein nur lesbares Byte-Array, welches die Daten für den Kindknoten vom Elternknoten speichert.
        /// <para>Basierend der Daten aus der Registry vom Typ REG_BINARY.</para>
        /// </summary>
        public IEnumerable<byte> RawBinaryData { get; }

        /// <summary>
        /// Abstrakte Klasse um einen der abgeleiteten Sonderfälle widerzuspiegeln.
        /// <para>
        /// Siehe hierfür:
        /// <seealso cref="RootFolderShellItem"/>, <seealso cref="VolumeShellItem"/>, <seealso cref="FileEntryShellItem"/>, <seealso cref="NetworkLocationShellItem"/>
        /// </para>
        /// </summary>
        public ShellItem ShellItem { get; } = null!;

        /// <summary>
        /// Der Konstruktor für die Klasse <see cref="ShellBagNode"/>.
        /// </summary>
        /// <param name="data">Die zu speichernden binäre Daten</param>
        public ShellBagNode(byte[] data)
        {
            RawBinaryData = data;
            if (data != null)
            {
                ShellItem = AnalyzeShellItemType();
            }
        }

        /// <summary>
        /// Indexer
        /// </summary>
        public new KeyValuePair<int, ShellBagNode> this[int i] => this.ElementAt(i);

        private ShellItem AnalyzeShellItemType()
        {
            // All class types as output found from earlier read from my computer:
            // 1F, 00, C3, 31, 35, 2E, 32, 26, 07, 3A, A2, A4, 9A, 8E, E7, BC, 52, C5, 2B, 70, 18, 9D, 90, 38, 08, 6A, 7C, 37, 3F, 2F, C8, 89, F5, BF, 09, 02, 5C, D8, 68, AA, 42, 36, 67
            var classType = RawBinaryData.Skip(2).Take(1).First();
            var itemSize = BitConverter.ToUInt16(RawBinaryData.Take(2).ToArray(),0);

            ShellItem shellitem;
            switch (classType)
            {
                case 0x1F:
                    // filter other type when the size isn't 0x14. Seems to be something new.
                    if (itemSize == 0x0014)
                    {
                        shellitem = new RootFolderShellItem(RawBinaryData);
                    }
                    else
                    {
                        shellitem = new UnknownShellItem(RawBinaryData);
                    }
                    break;
                case 0x31:
                case 0x32:
                    shellitem = new FileEntryShellItem(RawBinaryData);
                    break;
                case { } when (classType >= 0x20 && classType <= 0x2F ):
                    shellitem = new VolumeShellItem(RawBinaryData);
                    break;
                case 0xC3:
                    shellitem = new NetworkLocationShellItem(RawBinaryData);
                    break;
                default:
                    shellitem = new UnknownShellItem(RawBinaryData);
                    break;
            }

            return shellitem;
        }
    }
}
