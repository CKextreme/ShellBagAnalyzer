using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShellBag.Library.ShellBags;

namespace ShellBag.UnitTests
{
    [TestClass]
    public class ShellBagNodeTest
    {
        [TestMethod]
        public void CreateInstance()
        {
            var node1 = new ShellBagNode();
            var node2 = new ShellBagNode(null);
            Assert.IsNull(node1.RawBinaryData);
        }
    }
}
