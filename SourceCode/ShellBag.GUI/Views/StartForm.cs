using ShellBag.Library;
using ShellBag.Library.ShellBags;
using ShellBag.Library.ShellBags.ShellItems;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using ShellBag.GUI.Localization;
using ShellBag.Library.ShellBags.Logging;
using ShellBag.Library.ShellBags.ShellItems.ExtensionBlocks;
using ShellBag.Library.ShellBags.ShellItems.Others;

namespace ShellBag.GUI.Views
{
    /// <summary>
    /// 
    /// </summary>
    public partial class StartForm : Form
    {
        #region Global Variables
        /// <summary>
        /// 
        /// </summary>
        private readonly List<string> _comboList = new List<string>();
        /// <summary>
        /// 
        /// </summary>
        private readonly BindingSource _comboBoxBindingSource = new BindingSource();
        /// <summary>
        /// 
        /// </summary>
        private IDictionary<string, string> _sidDictionary = new Dictionary<string, string>();
        /// <summary>
        /// 
        /// </summary>
        private readonly Dictionary<string, ShellBagNode> _export = new Dictionary<string, ShellBagNode>();
        #endregion

        /// <summary>
        /// Public <see cref="StartForm"/> Constructor
        /// </summary>
        public StartForm()
        {
            InitializeComponent();

            // Eventlisteners
            Shown += StartForm_Shown;
            FormClosing += StartForm_FormClosing;
            beendenToolStripMenuItem.Click += BeendenToolStripMenuItem_Click;
            uebertoolStripMenuItem.Click += UebertoolStripMenuItem_Click;
            treeView1.AfterSelect += TreeView1_AfterSelect;
            treeView1.AfterCheck += TreeView1_AfterCheck;
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            textDateitxtToolStripMenuItem.Click += TextDateitxtToolStripMenuItem_Click;
        }

        #region StartForm Events
        /// <summary>
        /// 
        /// </summary>
        private void StartForm_Shown(object sender, EventArgs e)
        {
            InitComboBox();

            var admin = Helper.CheckAdminRights();
            if (!admin)
            {
                this.Text += $@" {Localization.Language.StartForm_Shown_NoAdminRights}";
                return;
            }
            this.Text += $@" {Localization.Language.StartForm_Shown_AdminRights}";
        }
        /// <summary>
        /// 
        /// </summary>
        private void StartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ShutdownQuestion())
            {
                e.Cancel = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void BeendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// 
        /// </summary>
        private void TextDateitxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Save to TextFile

            if (_export.Count == 0)
            {
                MessageBox.Show($@"{Localization.Language.StartForm_TextDateitxtToolStripMenuItem_SearchPlease}");
                return;
            }

            foreach (var element in _export)
            {
                //var exporter = new FileExport();
                FileExport.ExportToTextFile(element.Value, element.Key + ".dat");
            }

            MessageBox.Show($@"{Localization.Language.StartForm_TextDateitxtToolStripMenuItem_Success}");
        }
        /// <summary>
        /// 
        /// </summary>
        private void UebertoolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var about = new AboutForm();
            about.ShowDialog();
        }
        /// <summary>
        /// 
        /// </summary>
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = comboBox1.SelectedIndex;

            if (index == 0)
            {
                return;
            }

            var username = _sidDictionary[_comboList[index]];
            accountLabel.Text = $@"{Localization.Language.StartForm_ComboBoxChanged_Name} {username}";
            accountLabel.Visible = true;
            LoadShellBags(index);
        }
        /// <summary>
        /// 
        /// </summary>
        private void TreeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //
        }
        /// <summary>
        /// 
        /// </summary>
        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //e.Node.Checked = !e.Node.Checked;
            toolStripStatusLabel.Text = $@"{Localization.Language.StartForm_ChangeToolStripLabelPath_Text} {e.Node.FullPath}";

            var temp = (ShellBagNode)e.Node.Tag;
            RenderDataGridView(temp);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void InitComboBox()
        {
            _sidDictionary = ShellBag.Library.ShellBags.ShellBagHelper.LoadSiDsParallel();

            if (_sidDictionary.Count == 0)
            {

                toolStripStatusLabel.Text = $@"{Localization.Language.StartForm_InitComboBox_Zero}";
                return;
            }

            _comboList.Add($@"{Localization.Language.StartForm_ctr_combotxtDefault}");
            _comboBoxBindingSource.DataSource = _comboList;
            _comboList.AddRange(_sidDictionary.Keys);
            comboBox1.DataSource = _comboBoxBindingSource;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static bool ShutdownQuestion()
        {
            string message = $"{Localization.Language.StartForm_Close_Message}";
            string caption = $"{Localization.Language.StartForm_Close_Caption}";
            const MessageBoxIcon icon = MessageBoxIcon.Question;
            const MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            const MessageBoxDefaultButton defaultBtn = MessageBoxDefaultButton.Button2;
            var result = MessageBox.Show(message, caption, buttons, icon, defaultBtn);
            return result == DialogResult.Yes;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        private void LoadShellBags(int index)
        {
            treeView1.Nodes.Clear();
            _export.Clear();

            this.UseWaitCursor = true;
            toolStripStatusLabel.Text = $@"{Localization.Language.StartForm_LoadShellBags_Status}";

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            var parser = new ShellBagParser(_comboList[index]);

            // irgendwie optimieren! kein Boxing verwenden!
            var path_enums = (ShellBagParser.PathEnum[])Enum.GetValues(typeof(ShellBagParser.PathEnum));

            int global_count = 0;

            foreach (var path in path_enums)
            {
                var result = parser.LoadWithData(path);
                _export.Add(path.ToString(), result);
                global_count += parser.NodesCount;
                TreeNode root = null;
                var rootName = Enum.GetName(typeof(ShellBagParser.PathEnum), path);
                GenerateRecursiveTreeView(ref root, ref result, ref rootName);
                treeView1.Nodes.Add(root);
            }
            stopwatch.Stop();

            TreeViewHintLabel.Hide();
            this.UseWaitCursor = false;
            toolStripStatusLabel.Text = $@"{Localization.Language.StartForm_Ready}";

            toolStripStatusCountLabel.Text = "Knotenanzahl: " + global_count.ToString(CultureInfo.CurrentCulture);
            toolStripStatusLoadTimeLabel.Text = "Ladezeit: " + stopwatch.ElapsedMilliseconds.ToString(CultureInfo.CurrentCulture) + "ms";
            toolStripStatusCountLabel.Visible = true;
            toolStripStatusLoadTimeLabel.Visible = true;

            dataGridViewHintLabel.Text = Language.StartForm_LoadShellBags_datagridViewHintLabel;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="nodes"></param>
        /// <param name="rootName"></param>
        private void GenerateRecursiveTreeView(ref TreeNode root, ref ShellBagNode nodes, ref string rootName)
        {
            // if null then
            root ??= new TreeNode { Text = rootName + @".dat\BagMRU" };

            foreach (var node in nodes)
            {
                var child = new TreeNode {Tag = node.Value};

                // demo output for the nodes
                var name = $"[{string.Format(CultureInfo.CurrentCulture, "{0:D3}", node.Key)}] - ";
                name += node.Value.ShellItem switch
                {
                    NetworkLocationShellItem n => $"{n.Names.UncPath}",
                    VolumeShellItem v => $"{(string.IsNullOrEmpty(v.DriveLetter) ? "VolumeShellItem (unknown)" : $"Laufwerk: {v.DriveLetter}")}",
                    RootFolderShellItem r => Enum.IsDefined(typeof(SortIndex), r.SortIndex) ? $"{r.SortIndex}" : $"SortIndex (unknown): {string.Format(CultureInfo.CurrentCulture, "0x{0:X2}", (int)r.SortIndex)}",
                    FileEntryShellItem f => $"{f.PrimaryName}",
                    UnknownShellItem u => $"ShellItem (unknown): {string.Format(CultureInfo.CurrentCulture, "0x{0:X2}", u.ClassType)}",
#pragma warning disable CA1303
                    _ => throw new ArgumentOutOfRangeException(nameof(nodes), "node.Value.ShellItem has undefinded shell item")
#pragma warning restore CA1303
                };

                if (node.Value != null)
                {
                    var count = node.Value.Count;
                    name += $" (Knoten: {count})";
                }

                child.Text = name;
                var nextNode = node.Value;
                GenerateRecursiveTreeView(ref child, ref nextNode, ref rootName); // ref rootname could be null because its only used for root treenode
                root.Nodes.Add(child);
            }
        }

        private void RenderDataGridView(ShellBagNode node)
        {
            if (node == null) return;

            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            // Empty Column Header
            dataGridView1.Columns.Add("", "");
            dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            // Make First Column Items Bold, etc.
            using (FontFamily fontFamily = new FontFamily("Arial"))
            {
                Font font = new Font(fontFamily, 12, FontStyle.Bold, GraphicsUnit.Pixel);
                dataGridView1.Columns[0].DefaultCellStyle.Font = font;
            }

            switch (node.ShellItem)
            {
                case RootFolderShellItem r:
                    RenderRootItem(ref r);
                    break;
                case VolumeShellItem v:
                    RenderVolumeItem(ref v);
                    break;
                case NetworkLocationShellItem n:
                    RenderNetworkItem(ref n);
                    break;
                case FileEntryShellItem f:
                    RenderFileEntryItem(ref f);
                    break;
                case UnknownShellItem u:
                    RenderUnknownItem(ref u);
                    break;
                default:
#pragma warning disable CA1303
                    throw new Exception("Unknown ShellItem Type");
#pragma warning restore CA1303
            }
            dataGridViewHintLabel.Visible = false;
        }

        private void RenderRootItem(ref RootFolderShellItem root)
        {
            // Columns
            dataGridView1.Columns.Add(nameof(RootFolderShellItem), $"ShellItem: {nameof(RootFolderShellItem)}");
            // Rows
            dataGridView1.Rows.Add("ClassType:", string.Format(CultureInfo.CurrentCulture, "0x{0:X4}", root.ClassType));
            dataGridView1.Rows.Add("Size:", root.Size + " Bytes");
            dataGridView1.Rows.Add("GUID:", root.GlobalId.ToString());
            dataGridView1.Rows.Add("SortIndex:", root.SortIndex + $" ({string.Format(CultureInfo.CurrentCulture, "0x{0:X2}", (int)root.SortIndex)})");
            dataGridView1.Rows.Add("RawData:", BitConverter.ToString(root.RawData.ToArray()));
        }

        private void RenderVolumeItem(ref VolumeShellItem volume)
        {
            dataGridView1.Columns.Add(nameof(VolumeShellItem), $"ShellItem: {nameof(VolumeShellItem)}");
            dataGridView1.Rows.Add("ClassType:", string.Format(CultureInfo.CurrentCulture, "0x{0:X4}", volume.ClassType));
            dataGridView1.Rows.Add("Size:", volume.Size + " Bytes");

            if (!string.IsNullOrEmpty(volume.DriveLetter))
            {
                dataGridView1.Rows.Add("DriveLetter:", volume.DriveLetter);
            }
            dataGridView1.Rows.Add("RawData:",BitConverter.ToString(volume.RawData.ToArray()));
        }

        private void RenderNetworkItem(ref NetworkLocationShellItem network)
        {
            dataGridView1.Columns.Add(nameof(NetworkLocationShellItem), $"ShellItem: {nameof(NetworkLocationShellItem)}");
            dataGridView1.Rows.Add("ClassType:", string.Format(CultureInfo.CurrentCulture, "0x{0:X4}", network.ClassType));
            dataGridView1.Rows.Add("Size:", network.Size + " Bytes");
            dataGridView1.Rows.Add("UncPath:", network.Names.UncPath + $" ({network.Names.MicrosoftNetwork})");
            dataGridView1.Rows.Add("Description:",network.Names.Description);
            dataGridView1.Rows.Add("RawData:",BitConverter.ToString(network.RawData.ToArray()));
        }

        private void RenderFileEntryItem(ref FileEntryShellItem file)
        {
            dataGridView1.Columns.Add(nameof(FileEntryShellItem), $"ShellItem: {nameof(FileEntryShellItem)}");
            dataGridView1.Rows.Add("ClassType:", string.Format(CultureInfo.CurrentCulture, "0x{0:X4}", (int)file.ClassType) + $" ({file.ClassType})");
            dataGridView1.Rows.Add("Size:", file.Size + " Bytes");
            dataGridView1.Rows.Add("PrimaryName:", file.PrimaryName);

            if (file.ModificationDateTime != null)
            {
                dataGridView1.Rows.Add("Modification DateTime:", file.ModificationDateTime + " (UTC)");
            }
            else
            {
                dataGridView1.Rows.Add("Modification DateTime:", "null");
            }

            if (file.BeefType != null)
            {
                dataGridView1.Rows.Add("Beef-Size:",string.Format(CultureInfo.CurrentCulture, "0x{0:X4}", file.BeefType.Size));
                dataGridView1.Rows.Add("Beef-Version:", string.Format(CultureInfo.CurrentCulture, "0x{0:X4}", file.BeefType.Version));
                dataGridView1.Rows.Add("Beef-Signature:",file.BeefType.Signature.ToString());


                switch (file.BeefType)
                {
                    case Beef0004 beef0004:
                        dataGridView1.Rows.Add("Creation DateTime:", beef0004.CreationDateTime + " (UTC)");
                        dataGridView1.Rows.Add("LastAccess DateTime:", beef0004.LastAccessDateTime + " (UTC)");
                        dataGridView1.Rows.Add("SecondaryName:", beef0004.SecondaryName);
                        break;
                }

            }
            else
            {
                dataGridView1.Rows.Add("BeefType:", "null");
            }

            dataGridView1.Rows.Add("RawData:",BitConverter.ToString(file.RawData.ToArray()));
        }

        private void RenderUnknownItem(ref UnknownShellItem unknown)
        {
            dataGridView1.Columns.Add(nameof(UnknownShellItem), $"ShellItem: {nameof(UnknownShellItem)}");
            dataGridView1.Rows.Add("ClassType:", string.Format(CultureInfo.CurrentCulture, "0x{0:X4}", unknown.ClassType));
            dataGridView1.Rows.Add("Size:", unknown.Size + " Bytes");
            dataGridView1.Rows.Add("RawData",BitConverter.ToString(unknown.RawData.ToArray()));
        }
    }
}
