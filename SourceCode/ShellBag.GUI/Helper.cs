using System.Security.Principal;

namespace ShellBag.GUI
{
    /// <summary>
    /// Helpful class with some basic independent functions.
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// Checks if the program ist is started with admin permissions.
        /// <para>
        /// Source: <seealso href="https://stackoverflow.com/a/5953294"/>
        /// </para>
        /// </summary>
        /// <returns>True if program started as admin. Otherwise false.</returns>
        internal static bool CheckAdminRights()
        {
            using var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
