using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShellBag.Library;

namespace ShellBag.UnitTests
{
    [TestClass]
    public class ShellBagParserTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptySIDLoadOnDemand()
        {
            var parser = new ShellBagParser("");
            parser.LoadOnDemand(ShellBagParser.PathEnum.NtUser);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptySIDLoadWithData()
        {
            var parser = new ShellBagParser("");
            parser.LoadWithData(ShellBagParser.PathEnum.NtUser);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullSID()
        {
            string emptyStr;
            emptyStr = null;

            var parser = new ShellBagParser(emptyStr);
            parser.LoadOnDemand(ShellBagParser.PathEnum.NtUser);
        }
    }
}
