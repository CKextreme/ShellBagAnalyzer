using System;
using System.Windows.Forms;
using ShellBag.GUI.Views;

namespace ShellBag.GUI
{
    /// <summary>
    /// Program entrypoint class.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entrypoint method for the program.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using var startForm = new StartForm();
            Application.Run(startForm);
        }
    }
}
