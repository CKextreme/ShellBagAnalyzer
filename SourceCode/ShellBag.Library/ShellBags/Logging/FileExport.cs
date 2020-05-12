using ShellBag.Library.ShellBags.ShellItems;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ShellBag.Library.ShellBags.Logging
{
    /// <summary>
    /// Static class to export the data.
    /// </summary>
    public static class FileExport
    {
        /// <summary>
        /// 
        /// </summary>
        private const string Cross = "+- ";
        /// <summary>
        /// 
        /// </summary>
        private const string Vertical = "|  ";
        /// <summary>
        /// 
        /// </summary>
        private const string Space = "   ";
        /// <summary>
        /// Static method to export the data to a file.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Literale nicht als lokalisierte Parameter übergeben", Justification = "Library only in english")]
        public static void ExportToTextFile(ShellBagNode rootNode, string rootName)
        {
            if (rootNode == null) throw new ArgumentNullException(nameof(rootNode), $"{nameof(rootNode)} cannot be null!");
            var filename = $"Export_{rootName}_{string.Format(CultureInfo.CurrentCulture, "{0:yyyy-MM-dd-HH_mm_ss}", DateTime.Now)}" + ".txt";
            using StreamWriter sw = File.CreateText(filename);
            var temp = sw;
            RecursiveTreeGenerator(ref rootNode, "", false, rootName, ref temp);
        }
        /// <summary>
        /// Method to generate recursive the ASCII tree.
        /// </summary>
        /// <param name="rootNode"><see cref="ShellBagNode"/> root node with childs.</param>
        /// <param name="indent">Indent text to write</param>
        /// <param name="last">if the element is the last one.</param>
        /// <param name="rootName">Root name</param>
        /// <param name="sw"><see cref="StreamWriter"/></param>
        private static void RecursiveTreeGenerator(ref ShellBagNode rootNode, string indent, bool last, string rootName, ref StreamWriter sw)
        {
            var line = "";
            if (rootNode.ShellItem == null)
            {
                line = rootName;
            }
            else
            {
                var classtype = string.Format(CultureInfo.CurrentCulture, "0x{0:X4}", rootNode.ShellItem.ClassType);
                var size = rootNode.ShellItem.Size + " Bytes";
                var rawdata = BitConverter.ToString(rootNode.ShellItem.Data.ToArray());

                switch (rootNode.ShellItem)
                {
                    case RootFolderShellItem r:
                        var sortindex = r.SortIndex + $" ({string.Format(CultureInfo.CurrentCulture, "0x{0:X2}", (int)r.SortIndex)})";
                        var guid = r.GlobalId.ToString();
                        line = $"{nameof(RootFolderShellItem)}: ClassType={classtype} ; Size={size} ; SortIndex={sortindex} ; Guid={guid} ; RawData={rawdata}";
                        break;
                    case NetworkLocationShellItem n:
                        var path = $"UNC-Path={n.Names.UncPath} ({n.Names.MicrosoftNetwork}) , Description={n.Names.Description}";
                        line = $"{nameof(NetworkLocationShellItem)}: ClassType={classtype} ; Size={size} ; " + path + $" ; RawData={rawdata}";
                        break;
                    case VolumeShellItem v:
                        line = $"{nameof(VolumeShellItem)}: ClassType={classtype} ; Size={size} ; {(string.IsNullOrEmpty(v.DriveLetter) ? "" : $"{v.DriveLetter} ; ")} RawData={rawdata}";
                        break;
                    case FileEntryShellItem f:
                        var primaryname = f.PrimaryName;
                        var datetime = "";
                        if (f.ModificationDateTime != null)
                        {
                            datetime = f.ModificationDateTime + " (UTC)";
                        }
                        else
                        {
                            datetime = "null";
                        }

                        var beeftext = "";
                        if (f.BeefType != null)
                        {
                            var beefsize = string.Format(CultureInfo.CurrentCulture, "0x{0:X4}", f.BeefType.Size);
                            var beefversion = string.Format(CultureInfo.CurrentCulture, "0x{0:X4}", f.BeefType.Version);
                            var beefsignature = f.BeefType.Signature.ToString();
                            beeftext = $"BeefSize={beefsize} ; BeefVersion={beefversion} ; BeefSignature={beefsignature}";
                        }
                        else
                        {
                            beeftext = "BeefType=null";
                        }
                        line = $"{nameof(FileEntryShellItem)}: ClassType={classtype} ; Size={size} ; PrimaryName={primaryname} ; ModificationDateTime={datetime} ; {beeftext} ; RawData={rawdata}";
                        break;
                    case UnknownShellItem u:
                        line = $"{nameof(UnknownShellItem)}: ClassType={classtype} ; Size={size} ; RawData={rawdata}";
                        break;
                    default:
                        throw new Exception("Undefined ShellItem Type! Type: " + string.Format(CultureInfo.CurrentCulture, "0x{0:X2}", rootNode.ShellItem.ClassType));
                }
            }

            var text = indent + Cross + line;
            sw.WriteLine(text);

            indent += last ? Space : Vertical;

            for (var i = 0; i < rootNode.Count; i++)
            {
                var temp = rootNode[i].Value;
                RecursiveTreeGenerator(ref temp, indent, i == rootNode.Count - 1, rootName, ref sw);
            }
        }
    }
}
