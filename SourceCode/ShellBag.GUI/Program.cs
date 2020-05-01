using System;
using System.Windows.Forms;
using ShellBag.GUI.Views;

namespace ShellBag.GUI
{
    /// <summary>
    /// Programm-Einstiegspunkt-Klasse.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var startForm = new StartForm())
            {
                Application.Run(startForm);
            }
        }
    }
}
