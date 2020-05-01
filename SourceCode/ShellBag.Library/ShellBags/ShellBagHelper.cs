using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ShellBag.Library.ShellBags
{
    /// <summary>
    /// Hilfklasse für ShellBag-spezifische Funktionen.
    /// </summary>
    internal static class ShellBagHelper
    {
        /// <summary>
        /// Lade die verfügbaren User-SIDs aus der Registry.
        /// </summary>
        /// <returns>Gebe bei Erfolg ein <see cref="IDictionary{TKey, TValue}"/> zurück. Ansonsten <see langword="null"/>.</returns>
        internal static IDictionary<string, string> LoadSiDsParallel()
        {
            var allUsers = Microsoft.Win32.Registry.Users.GetSubKeyNames();
            var localUsers = new Dictionary<string, string>();
            
            // lade alle SIDs parallel
            var result = Parallel.For(0, allUsers.Length, (i, state) =>
            {
                // laut Beleg können die ShellBags in folgenden Knoten beginnen
                var reg1 = Microsoft.Win32.Registry.Users.OpenSubKey(allUsers[i] + @"\Software\Microsoft\Windows\Shell\BagMRU");
                var reg2 = Microsoft.Win32.Registry.Users.OpenSubKey(allUsers[i] + @"\Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\BagMRU");
                // wenn beide Pfade keine ShellBag besitzen, so schließe die SID aus (wahrscheinlich eine System-SID)
                if (reg1 == null && reg2 == null) return;
                // hole mir die SID des Users, welche eine Shell\BagMRU besitzt (schließe System-SIDs aus)
                var sid = reg1.Name.Split(new char[] { '\\' })[1];
                // lade den zugehörigen Benutzernamen, falls verfügbar)
                var account = GetAccountFromSid(sid);
                if (string.IsNullOrEmpty(account))
                {
                    account = "No Username"; //Resources.ShellBagsHelper_LoadSidsParallel_NoUsername;
                }
                localUsers.Add(sid, account);
            });

            // sollte die parallele Ausführung schief laufen, so gebe eine leere Liste zurück!
            if (result.IsCompleted) return new ReadOnlyDictionary<string, string>(localUsers);
            localUsers.Clear();
            return new ReadOnlyDictionary<string, string>(localUsers);
        }

        /// <summary>
        /// Suche den Benutzernamen basierend der übergebenen SID.
        /// <para>
        /// Quelle: <seealso href="https://stackoverflow.com/a/636786"/>
        /// </para>
        /// </summary>
        /// <param name="sid">Die SID des Benutzers</param>
        /// <returns>Sollte ein Benutzer gefunden worden sein, so gebe den Namen zurück, andernfalls einen leeren String.</returns>
        private static string GetAccountFromSid(string sid)
        {
            string account;
            try
            {
                account = new SecurityIdentifier(sid).Translate(typeof(NTAccount)).ToString();
            }
#pragma warning disable CA1031 // Keine allgemeinen Ausnahmetypen abfangen
            catch
#pragma warning restore CA1031 // Keine allgemeinen Ausnahmetypen abfangen
            {
                account = string.Empty;
            }
            return account;
        }
    }
}
