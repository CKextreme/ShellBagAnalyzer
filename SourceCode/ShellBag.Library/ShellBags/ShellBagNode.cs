using System;
using System.Collections.Generic;
using System.Linq;

namespace ShellBag.Library.ShellBags
{
    /// <summary>
    /// Abgeleitete Klasse von <see cref="Dictionary{TKey,TValue}"/>, welche die Knoten und deren Kindknoten speichert.
    /// </summary>
    public class ShellBagNode : Dictionary<int, ShellBagNode>
    {
        private static int counter = 0;

        /// <summary>
        /// Ein nur lesbares Byte-Array, welches die Daten für den Kindknoten vom Elternknoten speichert.
        /// <para>Basierend der Daten aus der Registry vom Typ REG_BINARY.</para>
        /// </summary>
        public byte[] RawBinaryData { get; }

        /// <summary>
        /// Parameterloser Konstruktor für die Klasse <see cref="ShellBagNode"/>.
        /// <para>
        /// <see cref="RawBinaryData"/> wird auf <see langword="null"/> gesetzt.
        /// </para>
        /// </summary>
        public ShellBagNode() => RawBinaryData = null;

        /// <summary>
        /// Der Konstruktor für die Klasse <see cref="ShellBagNode"/>.
        /// </summary>
        /// <param name="data">Die zu speichernden binäre Daten</param>
        public ShellBagNode(byte[] data)
        {
            RawBinaryData = data;
            if (data != null)
            {
                AnalyzeData();
            }
        }

        public void AnalyzeData()
        {
            if (counter == 2)
            {
                var subset = RawBinaryData.Skip(5).Take(29);
                var lengthArray = RawBinaryData.Take(2);
                var length = BitConverter.ToInt16(lengthArray.ToArray(), 0);
                var text = System.Text.Encoding.UTF8.GetString(subset.ToArray());
                Console.WriteLine("Größe: " + length + " / Text: " + text);
            }
            counter++;


            // .Net Framework - no Span<T>, only in .Net Core
            //var length = BitConverter.ToInt16(new[] {RawBinaryData[0], RawBinaryData[1]}, 0);
            //Console.WriteLine(length);
        }
    }
}
