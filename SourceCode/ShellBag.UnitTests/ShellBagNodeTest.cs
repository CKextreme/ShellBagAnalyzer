using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShellBag.UnitTests
{
    [TestClass]
    public class ShellBagNodeTest
    {
        [TestMethod]
        public void CreateInstance()
        {
            var node1 = new Library.ShellBags.ShellBagNode();
            Assert.IsNull(node1.RawBinaryData);
        }
    }
}
