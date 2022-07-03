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
            Assert.AreEqual("(from:achan_UGA OR from:tanigox OR from:daidoushinove)", exp.ToString());
        }        
        
        [TestMethod]
        public void TestAnd()
        {
            var exp = Expression.Keyword("Test Keyword").And(Expression.IsReply().Negate(), Expression.IsRetweet().Negate());
            Assert.AreEqual("(\"Test Keyword\" -is:reply -is:retweet)", exp.ToString());
        }
    }
}
