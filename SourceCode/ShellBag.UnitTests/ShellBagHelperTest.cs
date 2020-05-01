using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShellBag.UnitTests
{
    [TestClass]
    public class ShellBagHelperTest
    {
        [TestMethod]
        public void LoadSiDsParallel()
        {
            var test = ShellBag.Library.ShellBags.ShellBagHelper.LoadSiDsParallel();
            Assert.IsNotNull(test);
        }
        [TestMethod]
        public void GetAccountFromSid()
        {
            var sids = ShellBag.Library.ShellBags.ShellBagHelper.LoadSiDsParallel();

            var test = ShellBag.Library.ShellBags.ShellBagHelper.GetAccountFromSid(sids.First().Key);
            Assert.IsNotNull(test);
        }
    }
}
