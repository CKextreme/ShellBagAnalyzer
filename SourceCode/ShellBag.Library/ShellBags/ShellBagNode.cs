using System;
using System.Collections.Generic;
using System.Linq;
using ShellBag.Library.ShellBags.ShellItems;

namespace ShellBag.Library.ShellBags
{
    /// <summary>
    /// Abgeleitete Klasse von <see cref="Dictionary{TKey,TValue}"/>, welche die Knoten und deren Kindknoten speichert.
    /// </summary>
    public class ShellBagNode : Dictionary<int, ShellBagNode>
    {
        //private static int counter = 0;

        /// <summary>
        /// Ein nur lesbares Byte-Array, welches die Daten für den Kindknoten vom Elternknoten speichert.
        /// <para>Basierend der Daten aus der Registry vom Typ REG_BINARY.</para>
        /// </summary>
        public byte[] RawBinaryData { get; }

        /// <summary>
        /// Parameterloser Konstruktor für die Klasse <see cref="ShellBagNode"/>.
        /// <para>
        /// <see cref="RawBinaryData"/> wird auf <see langword="null"/> gesetzt.
        /// </para>
        /// </summary>
        public ShellBagNode() => RawBinaryData = null;

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

        // Test Fields
        public static Dictionary<byte,string> dict = new Dictionary<byte,string>();
        public ShellItem ShellItem { get; }

        private ShellItem AnalyzeShellItemType()
        {
            // All class types as output from earlier read from my computer:
            // 1F, 00, C3, 31, 35, 2E, 32, 26, 07, 3A, A2, A4, 9A, 8E, E7, BC, 52, C5, 2B, 70, 18, 9D, 90, 38, 08, 6A, 7C, 37, 3F, 2F, C8, 89, F5, BF, 09, 02, 5C, D8, 68, AA, 42, 36, 67
            var classType = RawBinaryData.Skip(2).Take(1).First();
            var itemSize = BitConverter.ToUInt16(RawBinaryData.Take(2).ToArray(),0);
            var data = RawBinaryData.Take(itemSize).ToArray();

            ShellItem shellitem = null;
            switch (classType)
            {
                // Root Folder Shell Item
                case 0x1F:
                    shellitem = new RootFolderShellItem(itemSize, classType, data);
                    break;
                // File Entry Shell Item
                // 0x31 - Folder
                // 0x32 Zip-File
                case 0x31:
                case 0x32:
                    shellitem = new FileEntryShellItem(itemSize, classType, data);
                    break;
                // Volume Shell Item
                case byte _ when (classType >= 0x20 && classType <= 0x2F ):
                    shellitem = new VolumeShellItem(itemSize, classType, data);
                    break;
                // Network Location Shell Item
                // more types seen, but not by us
                case 0xC3:
                    shellitem = new NetworkLocationShellItem(itemSize, classType, data);
                    break;
                default:
                    //Console.WriteLine("Unbekannter ClassType");
                    break;
            }

            return shellitem;

            //ShellItem test = new RootFolderShellItem(classType,classType);
            //if (test is RootFolderShellItem)
            //{
            //    ((RootFolderShellItem)test).Test();
            //}

            //switch (classType)
            //{
            //    case 0x31:
            //        Console.WriteLine("0x31 Hex");
            //        break;
            //    default:
            //        var arr = new[] {classType};

            //        if (!dict.ContainsKey(classType))
            //        {
            //            dict.Add(classType,BitConverter.ToString(arr));
            //        }
            //        //Console.WriteLine("unbekannter ClassType: " + BitConverter.ToString(arr));
            //        break;
            //}

            //if (counter == 2)
            //{
            //    var subset = RawBinaryData.Skip(5).Take(29);
            //    var lengthArray = RawBinaryData.Take(2);
            //    var length = BitConverter.ToInt16(lengthArray.ToArray(), 0);
            //    var text = System.Text.Encoding.UTF8.GetString(subset.ToArray());
            //    Console.WriteLine("Größe: " + length + " / Text: " + text);
            //}
            //counter++;


            // .Net Framework - no Span<T>, only in .Net Core
            //var length = BitConverter.ToInt16(new[] {RawBinaryData[0], RawBinaryData[1]}, 0);
            //Console.WriteLine(length);
        }
    }
}
