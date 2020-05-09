using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShellBag.UnitTests
{
    [TestClass]
    public class ShellBagParserTest
    {
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void EmptySIDLoadOnDemand()
        {
            var parser = new Library.ShellBagParser("");
            parser.LoadOnDemand(Library.ShellBagParser.PathEnum.NtUser);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void EmptySidLoadWithData()
        {
            var parser = new Library.ShellBagParser("");
            parser.LoadWithData(Library.ShellBagParser.PathEnum.NtUser);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void NullSid()
        {
            string emptyStr = null;

            var parser = new Library.ShellBagParser(emptyStr);
            parser.LoadOnDemand(Library.ShellBagParser.PathEnum.NtUser);
        }
    }
}
