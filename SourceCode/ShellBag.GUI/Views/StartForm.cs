//using ShellBag.Languages;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

// TODO: Elternknoten-Selektierung hakt alle Kindknoten ab

namespace ShellBag.GUI.Views
{
    ///
    ///
    ///
    /// <summary>
    /// 
    /// </summary>
    public partial class StartForm : Form
    {
        #region global variables
        /// <summary>
        /// 
        /// </summary>
        private readonly string _combotxtDefault;
        /// <summary>
        /// 
        /// </summary>
        private readonly List<string> _comboList = new List<string>();
        /// <summary>
        /// 
        /// </summary>
        private readonly BindingSource _bindingSource = new BindingSource();
        /// <summary>
        /// 
        /// </summary>
        private IDictionary<string, string> _sidDictionary = new Dictionary<string, string>();

        #endregion
        ///
        ///
        ///
        /// <summary>
        /// 
        /// </summary>
        public StartForm()
        {
            InitializeComponent();

            this.Shown += StartForm_Shown;
            treeView1.CheckBoxes = true;
            this.treeView1.AfterSelect += TreeView1_AfterSelect;
            this.treeView1.AfterCheck += TreeView1_AfterCheck;

            //_combotxtDefault = Resources.StartForm_ctr_combotxtDefault;
            //InitComboBox();
            this.comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
        }
        ///
        ///
        ///
        /// <summary>
        /// 
        /// </summary>
        //private void InitComboBox()
        //{
        //    _sidDictionary = ShellBagsHelper.LoadSiDsParallel();

        //    if (_sidDictionary.Count == 0)
        //    {
        //        toolStripStatusLabel.Text = Resources.StartForm_InitComboBox_Zero;
        //        return;
        //    }

        //    _comboList.Add(_combotxtDefault);
        //    _bindingSource.DataSource = _comboList;

        //    foreach (var e in _sidDictionary)
        //    {
        //        _comboList.Add(e.Key);
        //    }
        //    this.comboBox1.DataSource = _bindingSource;
        //}

        #region StartForm Events
        ///
        ///
        ///
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartForm_Shown(object sender, EventArgs e)
        {
            //var admin = Helper.CheckAdminRights();
            //if (!admin)
            //{
            //    this.Text += Resources.StartForm_Shown_NoAdminRights;
            //    return;
            //}
            //this.Text += Resources.StartForm_Shown_AdminRights;
        }
        ///
        ///
        ///
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //
        }
        ///
        ///
        ///
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //e.Node.Checked = !e.Node.Checked;
            //ChangeToolStripLabelPath(e.Node.FullPath);
        }
        ///
        ///
        ///
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var index = comboBox1.SelectedIndex;

            //if (index == 0)
            //{
            //    return;
            //}

            //var username = _sidDictionary[_comboList[index]];
            //ChangeUsername(username);
            //LoadShellBags(index);
        }
        ///
        ///
        ///
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UebertoolStripMenuItem_Click(object sender, EventArgs e)
        {
            //using (var about = new AboutForm())
            //{
            //    about.ShowDialog();
            //}
        }
        ///
        /// 
        /// 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var message = Resources.StartForm_Close_Message;
            //var caption = Resources.StartForm_Close_Caption;
            //var icon = MessageBoxIcon.Question;
            //var buttons = MessageBoxButtons.YesNo;
            //var defaultBtn = MessageBoxDefaultButton.Button2;
            //var result = MessageBox.Show(message, caption, buttons, icon, defaultBtn);

            //if (result != DialogResult.Yes) return;
            //this.Close();
        }
        #endregion
        ///
        ///
        ///
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        //private void ChangeToolStripLabelPath(string path)
        //{
        //    toolStripStatusLabel.Text = Resources.StartForm_ChangeToolStripLabelPath_Text + path;
        //}
        ///
        ///
        ///
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        //private void ChangeUsername(string text)
        //{
        //    accountLabel.Text = Resources.StartForm_ComboBoxChanged_Name + text;
        //    accountLabel.Visible = true;
        //}
        ///
        ///
        ///
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        //private void LoadShellBags(int index)
        //{
        //    treeView1.Nodes.Clear();
        //    this.UseWaitCursor = true;
        //    toolStripStatusLabel.Text = "Lade ShellBags...";

        //    var stopwatch = new System.Diagnostics.Stopwatch();
        //    stopwatch.Start();
        //    var parser = new ShellBagParser(_comboList[index]);

        //    // irgendwie optimieren! kein Boxing verwenden!
        //    var path_enums = (ShellBagParser.PathEnum[])Enum.GetValues(typeof(ShellBagParser.PathEnum));

        //    int global_count = 0;

        //    foreach (var path in path_enums)
        //    {
        //        var result = parser.LoadWithData(path);
        //        global_count += parser.NodesCount;
        //        TreeNode root = null;
        //        var rootName = Enum.GetName(typeof(ShellBagParser.PathEnum), path);
        //        GenerateRecursiveTreeView(ref root, ref result, ref rootName);
        //        treeView1.Nodes.Add(root);
        //    }
        //    stopwatch.Stop();

        //    TreeViewHintLabel.Hide();
        //    this.UseWaitCursor = false;
        //    toolStripStatusLabel.Text = "Bereit!";

        //    toolStripStatusCountLabel.Text = "Knotenanzahl: " + global_count.ToString();
        //    toolStripStatusLoadTimeLabel.Text = "Ladezeit: " + stopwatch.ElapsedMilliseconds.ToString() + "ms";
        //    toolStripStatusCountLabel.Visible = true;
        //    toolStripStatusLoadTimeLabel.Visible = true;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="nodes"></param>
        /// <param name="rootName"></param>
        //private void GenerateRecursiveTreeView(ref TreeNode root, ref ShellBagNode nodes, ref string rootName)
        //{
        //    if (root == null)
        //    {
        //        root = new TreeNode { Text = rootName + @".dat\BagMRU" };
        //    }

        //    foreach (var node in nodes)
        //    {
        //        var child = new TreeNode();
        //        // testweise den binären Text aus dem Elternknoten, welches dem zugehörigen Kind gehört

        //        var temp = node.Value.RawBinaryData != null ? BitConverter.ToString(node.Value.RawBinaryData) : node.Key.ToString();

        //        child.Text = temp;
        //        var nextNode = node.Value;
        //        GenerateRecursiveTreeView(ref child, ref nextNode, ref rootName);
        //        root.Nodes.Add(child);
        //    }
        //}
    }
}
