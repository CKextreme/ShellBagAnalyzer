#nullable enable
namespace ShellBag.Library.ShellBags.ShellItems.Others
{
    public class UncNames
    {
        public string UncPath { get; }
        public string MicrosoftNetwork { get; }
        public string? Description { get; }

        public UncNames(string path, string mn, string? description)
        {
            UncPath = path;
            MicrosoftNetwork = mn;
            Description = description;
        }
    }
}
