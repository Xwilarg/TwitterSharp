using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using TwitterSharp.Client;

namespace TwitterSharp.UnitTests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public async Task GetTweetByIdAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetByIdAsync("1389189291582967809");
            Assert.IsTrue(answer.Length == 1);
            Assert.AreEqual("1389189291582967809", answer[0].Id);
            Assert.AreEqual("たのしみ！！\uD83D\uDC93 https://t.co/DgBYVYr9lN", answer[0].Text);
        }
    }
}
