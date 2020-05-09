using ShellBag.Library;
using ShellBag.Library.ShellBags;
using ShellBag.Library.ShellBags.ShellItems;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ShellBag.Library.ShellBags.Logging;
using ShellBag.Library.ShellBags.ShellItems.Others;

// TODO: Elternknoten-Selektierung hakt alle Kindknoten ab

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
        #endregion

        /// <summary>
        /// Public <see cref="StartForm"/> Constructor
        /// </summary>
        public StartForm()
        {
            InitializeComponent();

            // Eventlisteners
            this.Shown += StartForm_Shown;
            this.FormClosing += StartForm_FormClosing;
            this.beendenToolStripMenuItem.Click += BeendenToolStripMenuItem_Click;
            this.uebertoolStripMenuItem.Click += UebertoolStripMenuItem_Click;
            this.treeView1.AfterSelect += TreeView1_AfterSelect;
            this.treeView1.AfterCheck += TreeView1_AfterCheck;
            this.comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
        }

        #region StartForm Events

        private void StartForm_Shown(object sender, EventArgs e)
        {
            InitComboBox();

            var admin = Helper.CheckAdminRights();
            if (!admin)
            {
                this.Text += " (Keine Adminrechte!)";
                //this.Text += Resources.StartForm_Shown_NoAdminRights;
                return;
            }
            this.Text += " (Adminrechte!)";
            //this.Text += Resources.StartForm_Shown_AdminRights;
        }

        private void StartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ShutdownQuestion())
            {
                e.Cancel = true;
            }
        }

        private void BeendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UebertoolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var about = new AboutForm())
            {
                about.ShowDialog();
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = comboBox1.SelectedIndex;

            if (index == 0)
            {
                return;
            }

            var username = _sidDictionary[_comboList[index]];
            accountLabel.Text = username; //Resources.StartForm_ComboBoxChanged_Name + text;
            accountLabel.Visible = true;
            LoadShellBags(index);
        }

        private void TreeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {

        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            e.Node.Checked = !e.Node.Checked;
            toolStripStatusLabel.Text = "Pfad: " + e.Node.FullPath; //Resources.StartForm_ChangeToolStripLabelPath_Text + path;
        }
        #endregion


        private void InitComboBox()
        {
            _sidDictionary = ShellBag.Library.ShellBags.ShellBagHelper.LoadSiDsParallel();

            if (_sidDictionary.Count == 0)
            {

                toolStripStatusLabel.Text = "Leere Combobox"; //Resources.StartForm_InitComboBox_Zero;
                return;
            }

            _comboList.Add("leere combobox"); // Resources...
            _comboBoxBindingSource.DataSource = _comboList;
            _comboList.AddRange(_sidDictionary.Keys);
            comboBox1.DataSource = _comboBoxBindingSource;
        }

        private bool ShutdownQuestion()
        {
            const string message = "Beenden?"; //Resources.StartForm_Close_Message;
            const string caption = "Beenden?"; //Resources.StartForm_Close_Caption;
            const MessageBoxIcon icon = MessageBoxIcon.Question;
            const MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            const MessageBoxDefaultButton defaultBtn = MessageBoxDefaultButton.Button2;
            var result = MessageBox.Show(message, caption, buttons, icon, defaultBtn);
            return result == DialogResult.Yes;
        }






        private void LoadShellBags(int index)
        {
            treeView1.Nodes.Clear();
            this.UseWaitCursor = true;
            toolStripStatusLabel.Text = "Lade ShellBags...";

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            var parser = new ShellBagParser(_comboList[index]);

            // irgendwie optimieren! kein Boxing verwenden!
            var path_enums = (ShellBagParser.PathEnum[])Enum.GetValues(typeof(ShellBagParser.PathEnum));

            int global_count = 0;

            foreach (var path in path_enums)
            {
                var result = parser.LoadWithData(path);
                global_count += parser.NodesCount;
                TreeNode root = null;
                var rootName = Enum.GetName(typeof(ShellBagParser.PathEnum), path);
                GenerateRecursiveTreeView(ref root, ref result, ref rootName);
                treeView1.Nodes.Add(root);
            }
            stopwatch.Stop();

            TreeViewHintLabel.Hide();
            this.UseWaitCursor = false;
            toolStripStatusLabel.Text = "Bereit!";

            toolStripStatusCountLabel.Text = "Knotenanzahl: " + global_count.ToString();
            toolStripStatusLoadTimeLabel.Text = "Ladezeit: " + stopwatch.ElapsedMilliseconds.ToString() + "ms";
            toolStripStatusCountLabel.Visible = true;
            toolStripStatusLoadTimeLabel.Visible = true;
        }

        private void GenerateRecursiveTreeView(ref TreeNode root, ref ShellBagNode nodes, ref string rootName)
        {
            // if null then
            root ??= new TreeNode { Text = rootName + @".dat\BagMRU" };

            foreach (var node in nodes)
            {
                var child = new TreeNode();

                // demo output for the nodes
                var temp = $"{node.Key}: ";
                temp += node.Value.ShellItem switch
                {
                    NetworkLocationShellItem networkitem => "NetworkLocationShellItem: " + networkitem.Names.UncPath + $" ({networkitem.Names.MicrosoftNetwork} | Description: {(string.IsNullOrEmpty(networkitem.Names.Description) ? "-" : networkitem.Names.Description)})",
                    VolumeShellItem volumeitem => $"VolumeItem-Type - DriveLetter: {(string.IsNullOrEmpty(volumeitem.DriveLetter) ? "/" : volumeitem.DriveLetter)}",
                    RootFolderShellItem rootItem => Enum.IsDefined(typeof(SortIndex), rootItem.SortIndex) ? $"SortIndex: {rootItem.SortIndex} / Guid: {rootItem.GlobalId}" : $"SortIndex (unbekannt!): {string.Format("0x{0:X2}", rootItem.SortIndex)}",
                    FileEntryShellItem fileItem => Enum.IsDefined(typeof(FileEntryClassType), fileItem.ClassType) ? "FileEntryShellItem: " + string.Format("0x{0:X2}", (int)fileItem.ClassType) + $" / PrimaryName: {fileItem.PrimaryName}" + $" / DateTime (Demo): {fileItem.ModificationDateTime}" : "FileEntryShellItem (unbekannt!): " + string.Format("0x{0:X2}", (int)fileItem.ClassType),
                    UnknownShellItem unknowItem => "Unknown ShellItem!" + $" Classtype: {string.Format("0x{0:X2}", unknowItem.ClassType)}",
                    _ => "Default Text - dürfte nicht sein!"
                };
                child.Text = temp;
                var nextNode = node.Value;
                GenerateRecursiveTreeView(ref child, ref nextNode, ref rootName);
                root.Nodes.Add(child);
            }
        }

        private void TestLoop(ref ShellBagNode node)
        {
            //var temp = node.ShellItem;
            //string type;

            //switch (temp)
            //{
            //    case FileEntryShellItem item1:
            //    {
            //        var root = item1;
            //        Console.WriteLine("FileEntryShellItem - " + "Size: " + root.Size + " / ClassType:" + root.ClassType);
            //        break;
            //    }
            //    case VolumeShellItem item2:
            //    {
            //        var root = item2;
            //        Console.WriteLine("VolumeShellItem - " + "Size: " + root.Size + " / ClassType:" + root.ClassType);
            //        break;
            //    }
            //    case RootFolderShellItem item3:
            //    {
            //        var root = item3;
            //        Console.WriteLine("RootFolderShellItem - " + "Size: " + root.Size + " / ClassType:" + root.ClassType);
            //        break;
            //    }
            //}

            //foreach (var pair in node)
            //{
            //    var nextnode = pair.Value;
            //    TestLoop(ref nextnode);
            //}
        }
    }
}
