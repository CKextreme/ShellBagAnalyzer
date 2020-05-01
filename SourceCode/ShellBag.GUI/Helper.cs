using System.Security.Principal;

namespace ShellBag
{
    /// <summary>
    /// Hilfsklasse für hilfreiche Methoden.
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// Prüfe ob das Programm mit Administratorrechten gestartet wurde.
        /// <para>
        /// Quelle: <seealso href="https://stackoverflow.com/a/5953294"/>
        /// </para>
        /// </summary>
        /// <returns>True wenn Programm mit Adminrechten gestartet. Andernfalls false.</returns>
        internal static bool CheckAdminRights()
        {
            using (var identity = WindowsIdentity.GetCurrent())
            {
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
    }
}
