using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ShellBag.Library.ShellBags.ShellItems;

namespace ShellBag.Library.ShellBags.Logging
{
    public class FileExport
    {
        private const string Cross = "+- ";
        private const string Vertical = "|  ";
        private const string Space = "   ";

        //private string _textfile = "";

        public void ExportToTextFile(ShellBagNode rootNode, string rootName)
        {
            if (rootNode != null)
            {
                var filename = $"Export_{rootName}_{string.Format(CultureInfo.CurrentCulture, "{0:yyyy-MM-dd-HH_mm_ss}", DateTime.Now)}" + ".txt";
                using (StreamWriter sw = File.CreateText(filename))
                {
                    var temp = sw;
                    RecursiveTreeGenerator(ref rootNode, "", false, rootName, ref temp);
                }
            }
        }

        private void RecursiveTreeGenerator(ref ShellBagNode rootNode, string indent, bool last, string rootName, ref StreamWriter sw)
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
                var rawdata = BitConverter.ToString(rootNode.ShellItem.Data);

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
                            datetime = f.ModificationDateTime.ToString() + " (UTC)";
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
                        throw new Exception("Undefined ShellItem Type");
                }
            }

            //_textfile += indent + Cross + line + "\n";

            var _text = indent + Cross + line;
            sw.WriteLine(_text);

            indent += last ? Space : Vertical;

            for (int i = 0; i < rootNode.Count; i++)
            {
                var temp = rootNode[rootNode.ElementAt(i).Key];
                RecursiveTreeGenerator(ref temp, indent, i == rootNode.Count - 1, rootName, ref sw);
            }
        }
    }
}
