using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitterSharp.JsonOption;

namespace TwitterSharp.UnitTests
{
    [TestClass]
    public class TestSnakeCaseNamingPolicy
    {
        [TestMethod]
        public void TestCase()
        {
            var policy = new SnakeCaseNamingPolicy();
            Assert.AreEqual("this_is_a_test", policy.ConvertName("ThisIsATest"));
        }
    }
}
