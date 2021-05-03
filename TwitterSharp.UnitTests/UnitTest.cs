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
            var answer = await client.GetTweetsByIdAsync("1389189291582967809");
            Assert.IsTrue(answer.Length == 1);
            Assert.AreEqual("1389189291582967809", answer[0].Id);
            Assert.AreEqual("たのしみ！！\uD83D\uDC93 https://t.co/DgBYVYr9lN", answer[0].Text);
        }

        [TestMethod]
        public async Task GetTweetsByIdAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsByIdAsync("1389330151779930113", "1389331863102128130");
            Assert.IsTrue(answer.Length == 2);
            Assert.AreEqual("1389330151779930113", answer[0].Id);
            Assert.AreEqual("ねむくなーい！ねむくないねむくない！ドタドタドタドタ", answer[0].Text);
            Assert.AreEqual("1389331863102128130", answer[1].Id);
            Assert.AreEqual("( - ω・ )", answer[1].Text);
        }
    }
}
