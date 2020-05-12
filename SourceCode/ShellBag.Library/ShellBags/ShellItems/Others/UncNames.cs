namespace ShellBag.Library.ShellBags.ShellItems.Others
{
    /// <summary>
    /// <see cref="UncPath"/> class.
    /// <para>
    /// Used by: <seealso cref="NetworkLocationShellItem"/>
    /// </para>
    /// </summary>
    public class UncNames
    {
        /// <summary>
        /// The UNC Path (network path)
        /// </summary>
        public string UncPath { get; }
        /// <summary>
        /// Mostly contains the string 'Microsoft Network' to describe the network type.
        /// </summary>
        public string MicrosoftNetwork { get; }
        /// <summary>
        /// Optionally Description property with extra Information for <seealso cref="UncPath"/>
        /// </summary>
        public string Description { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        public UncNames(string path, string mn, string description)
        {
            UncPath = path;
            MicrosoftNetwork = mn;
            Description = description;
        }
    }
}
