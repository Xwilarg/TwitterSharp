using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using TwitterSharp.Client;
using TwitterSharp.Request.AdvancedSearch;

namespace TwitterSharp.UnitTests
{
    [TestClass]
    public class TestTweet
    {
        [TestMethod]
        public async Task GetTweetByIdsAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsByIdsAsync("1389189291582967809");
            Assert.IsTrue(answer.Length == 1);
            Assert.AreEqual("1389189291582967809", answer[0].Id);
            Assert.AreEqual("たのしみ！！\uD83D\uDC93 https://t.co/DgBYVYr9lN", answer[0].Text);
            Assert.IsNull(answer[0].Author);
            Assert.IsNull(answer[0].PossiblySensitive);
        }

        [TestMethod]
        public async Task GetTweetByIdsWithAuthorAndSensitivityAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsByIdsAsync(new[] { "1389189291582967809" }, new[] { TweetOption.Possibly_Sensitive }, Array.Empty<UserOption>());
            Assert.IsTrue(answer.Length == 1);
            Assert.AreEqual("1389189291582967809", answer[0].Id);
            Assert.AreEqual("たのしみ！！\uD83D\uDC93 https://t.co/DgBYVYr9lN", answer[0].Text);
            Assert.IsNotNull(answer[0].Author);
            Assert.IsNotNull(answer[0].PossiblySensitive);
            Assert.AreEqual("kiryucoco", answer[0].Author.Username);
            Assert.IsFalse(answer[0].PossiblySensitive.Value);
        }

        [TestMethod]
        public async Task GetTweetsByIdsAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsByIdsAsync("1389330151779930113", "1389331863102128130");
            Assert.IsTrue(answer.Length == 2);
            Assert.AreEqual("1389330151779930113", answer[0].Id);
            Assert.AreEqual("ねむくなーい！ねむくないねむくない！ドタドタドタドタ", answer[0].Text);
            Assert.IsNull(answer[0].Author);
            Assert.AreEqual("1389331863102128130", answer[1].Id);
            Assert.AreEqual("( - ω・ )", answer[1].Text);
            Assert.IsNull(answer[1].Author);
        }

        [TestMethod]
        public async Task GetTweetsByIdsWithAuthorAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsByIdsAsync(new[] { "1389330151779930113", "1389331863102128130" }, null, Array.Empty<UserOption>());
            Assert.IsTrue(answer.Length == 2);
            Assert.AreEqual("1389330151779930113", answer[0].Id);
            Assert.AreEqual("ねむくなーい！ねむくないねむくない！ドタドタドタドタ", answer[0].Text);
            Assert.IsNotNull(answer[0].Author);
            Assert.AreEqual("tsunomakiwatame", answer[0].Author.Username);
            Assert.AreEqual("1389331863102128130", answer[1].Id);
            Assert.AreEqual("( - ω・ )", answer[1].Text);
            Assert.IsNotNull(answer[1].Author);
            Assert.AreEqual("tsunomakiwatame", answer[1].Author.Username);
        }

        [TestMethod]
        public async Task GetTweetsFromUserIdAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsFromUserIdAsync("1109748792721432577");
            Assert.IsTrue(answer.Length == 10);
            Assert.IsNull(answer[0].Author);
        }

        [TestMethod]
        public async Task GetTweetsFromUserIdWithAuthorAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsFromUserIdAsync("1109748792721432577", null, Array.Empty<UserOption>());
            Assert.IsTrue(answer.Length == 10);
            foreach (var t in answer)
            {
                Assert.IsNotNull(t.Author);
                Assert.AreEqual("inugamikorone", t.Author.Username);
            }
        }
    }
}
