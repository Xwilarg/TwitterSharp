using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitterSharp.Rule;

namespace TwitterSharp.UnitTests
{
    [TestClass]
    public class TestExpression
    {
        [TestMethod]
        public void TestOr()
        {
            var exp = Expression.Author("achan_UGA").Or(Expression.Author("tanigox"), Expression.Author("daidoushinove"));
            Assert.AreEqual("from:achan_UGA OR from:tanigox OR from:daidoushinove", exp.ToString());
        }
    }
}
