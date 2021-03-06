﻿using System;
using Microsoft.Win32;
using ShellBag.Library.ShellBags;

namespace ShellBag.Library
{
    /// <summary>
    /// Die <see cref="ShellBagParser"/>-Klasse dient zum Auslesen der spezifischen ShellBag-Registry-Pfade.
    /// </summary>
    public class ShellBagParser
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly string _sid;
        /// <summary>
        /// Constant Ntuser path
        /// </summary>
        private const string NtUserPath = @"\Software\Microsoft\Windows\Shell\BagMRU";
        /// <summary>
        /// Constant UsrClass path
        /// </summary>
        private const string UsrClassPath = @"\Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\BagMRU";

        /// <summary>
        /// Zählt die Gesamtanzahl aller Knoten je <see cref="ShellBagParser"/>-Klasse
        /// </summary>
        public int NodesCount { get; private set; }

        /// <summary>
        /// Enumeration für die möglichen Suchpfade
        /// </summary>
        public enum PathEnum
        {
            NtUser,
            UsrClass
        }


        /// <summary>
        /// Klasse zum Auslesen der Ordneraktivität unter Angabe eines bestimmten Benutzers, basierend seiner SID.
        /// </summary>
        /// <param name="sid">SID des zu traversierenden Benutzers aus der Registry</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Literale nicht als lokalisierte Parameter übergeben", Justification = "<Ausstehend>")]
        public ShellBagParser(string sid)
        {
            if (sid == null)
            {
                throw new ArgumentNullException(nameof(sid));
            }
            else if (string.IsNullOrEmpty(sid))
            {
                throw new ArgumentException($"The parameter {nameof(sid)} must not be zero!");
            }
            _sid = sid;
        }

        /// <summary>
        /// Schnelles laden alle Eltern- und Kindknoten (ohne Daten).
        /// </summary>
        /// <param name="searchEnum">Enumeration zur Angabe des zu traversierenden Pfades.</param>
        /// <returns><see cref="ShellBagNode"/></returns>
        public ShellBagNode LoadOnDemand(PathEnum searchEnum)
        {
            var rootKey = searchEnum == PathEnum.NtUser ? Registry.Users.OpenSubKey(_sid + NtUserPath) : Registry.Users.OpenSubKey(_sid + UsrClassPath);
            return RecursiveTraverse(rootKey, null, false);
        }

        /// <summary>
        /// Lade alle Eltern- und Kindknoten mitsamt ihrer zugehörigen Daten.
        /// </summary>
        /// <param name="searchEnum">Enumeration zur Angabe des zu traversierenden Pfades.</param>
        /// <returns><see cref="ShellBagNode"/></returns>
        public ShellBagNode LoadWithData(PathEnum searchEnum)
        {
            var rootKey = searchEnum == PathEnum.NtUser ? Registry.Users.OpenSubKey(_sid + NtUserPath) : Registry.Users.OpenSubKey(_sid + UsrClassPath);
            return RecursiveTraverse(rootKey, null, true);
        }

        /// <summary>
        /// Recursive Hilfsfunktion für das Laden der Baumstruktur.
        /// </summary>
        /// <param name="parentKey"><see cref="RegistryKey"/> des Elternknotens</param>
        /// <param name="data">Ein Byte-Array, welches die Daten für den Kindknoten vom Elternknoten übergibt.</param>
        /// <param name="withData">Angabe ob mit oder ohne Daten geladen werden sollen.</param>
        /// <returns><see cref="ShellBagNode"/></returns>
        private ShellBagNode RecursiveTraverse(RegistryKey parentKey, byte[] data, bool withData)
        {
            var newnode = new ShellBagNode(data);
            var subKeyNames = parentKey.GetSubKeyNames();

            foreach (var key in subKeyNames)
            {
                byte[] value = null;
                if (withData)
                {
                    // hole den Inhalt des Elternknotens basierend des Kindknoten-Namens (0, 1, etc.)
                    value = (byte[])parentKey.GetValue(key);
                }

#pragma warning disable CA1305 // IFormatProvider angeben
                var i = int.Parse(key);
#pragma warning restore CA1305 // IFormatProvider angeben
                var subkeys = RecursiveTraverse(parentKey.OpenSubKey(key), value, withData);
                newnode.Add(i, subkeys);
                NodesCount++;
            }
            return newnode;
        }
    }
}
