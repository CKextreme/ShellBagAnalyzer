namespace ShellBag.Library.ShellBags.ShellItems.ExtensionBlocks
{
    /// <summary>
    /// Special Beef-Type with the signature 0x0400efbe (beef0004)
    /// </summary>
    public class Beef0004 : ExtensionBlock
    {
        /// <summary>
        /// A secondary name (full long name) for the <see cref="FileEntryShellItem"/>.PrimaryName property.
        /// <para>
        /// <seealso cref="FileEntryShellItem"/>
        /// </para>
        /// </summary>
        public string SecondaryName { get; private set; } = null!;
        /// <summary>
        /// Constructor
        /// </summary>
        public Beef0004(ushort size, ushort version, Signature signature) : base(size, version, signature)
        {
            // need to be implemented
            SecondaryName = "";
            // AnalyzeData();
        }

        protected override void AnalyzeData()
        {
            throw new System.NotImplementedException();
        }
    }
}
