using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ShellBag.UnitTests
{
    [TestClass]
    public class ShellBagHelperTest
    {
        [TestMethod]
        public void LoadSiDsParallel()
        {
            var test = Library.ShellBags.ShellBagHelper.LoadSiDsParallel();
            Assert.IsNotNull(test);
        }
        [TestMethod]
        public void GetAccountFromSid()
        {
            var sids = Library.ShellBags.ShellBagHelper.LoadSiDsParallel();

            var test = Library.ShellBags.ShellBagHelper.GetAccountFromSid(sids.First().Key);
            Assert.IsNotNull(test);
        }
    }
}
