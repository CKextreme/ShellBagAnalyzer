using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Principal;
using System.Threading.Tasks;
using ShellBag.Library.ShellBags.Logging;

namespace ShellBag.Library.ShellBags
{
    /// <summary>
    /// Helper class for shell bag specific functions.
    /// </summary>
    public static class ShellBagHelper
    {
        /// <summary>
        /// Load the available user-SIDs from the registry.
        /// </summary>
        /// <returns>If success return <see cref="IDictionary{TKey, TValue}"/>. Otherwise <see langword="null"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Literale nicht als lokalisierte Parameter übergeben", Justification = "Library only english")]
        public static IDictionary<string, string> LoadSiDsParallel()
        {
            var allUsers = Microsoft.Win32.Registry.Users.GetSubKeyNames();
            var localUsers = new Dictionary<string, string>();
            
            // load all Sids parallel
            var result = Parallel.For(0, allUsers.Length, (i, state) =>
            {
                // based of the research paper the shell bags could exists in the following nodes.
                var reg1 = Microsoft.Win32.Registry.Users.OpenSubKey(allUsers[i] + @"\Software\Microsoft\Windows\Shell\BagMRU");
                var reg2 = Microsoft.Win32.Registry.Users.OpenSubKey(allUsers[i] + @"\Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\BagMRU");
                // if both paths have no ShellBag, exclude the SID (probably a system SID)
                if (reg1 == null && reg2 == null) return;
                // get me the SID of the user that has a Shell\BagMRU (exclude system SIDs)
                var sid = reg1!.Name.Split(new char[] { '\\' })[1];
                // load the corresponding user name, if available
                var account = GetAccountFromSid(sid);
                if (string.IsNullOrEmpty(account))
                {
                    account = "No Username found!";
                }
                localUsers.Add(sid, account);
            });

            // if the parallel execution fails, return an empty list!
            if (result.IsCompleted)
            {
                ConsoleLogger.Log(LogLevels.Debug, $"Found SIDs: {localUsers.Count}");
                return new ReadOnlyDictionary<string, string>(localUsers);
            }
            ConsoleLogger.Log(LogLevels.Warning, $"{nameof(LoadSiDsParallel)} isn't completed!");
            localUsers.Clear();
            return new ReadOnlyDictionary<string, string>(localUsers);
        }

        /// <summary>
        /// Search for the user name based on the SID passed.
        /// <para>
        /// Source: <seealso href="https://stackoverflow.com/a/636786"/>
        /// </para>
        /// </summary>
        /// <param name="sid">The SID of the user</param>
        /// <returns>If a user has been found, return the name, otherwise an empty string.</returns>
        public static string GetAccountFromSid(string sid)
        {
            string account;
            try
            {
                account = new SecurityIdentifier(sid).Translate(typeof(NTAccount)).ToString();
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                account = string.Empty;
                ConsoleLogger.Log(e);
            }
            return account;
        }
    }
}
