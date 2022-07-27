using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using TwitterSharp.Client;
using TwitterSharp.Request.AdvancedSearch;
using TwitterSharp.Request.Option;

namespace TwitterSharp.UnitTests
{
    [TestClass]
    public class TestUser
    {
        [TestMethod]
        public async Task GetUserAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetUserAsync("theindra5");
            Assert.AreEqual("1022468464513089536", answer.Id);
            Assert.AreEqual("TheIndra5", answer.Username);
            Assert.AreEqual("Indra", answer.Name);
        }

        [TestMethod]
        public async Task GetUserById()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetUserByIdAsync("1022468464513089536");
            Assert.AreEqual("1022468464513089536", answer.Id);
            Assert.AreEqual("TheIndra5", answer.Username);
            Assert.AreEqual("Indra", answer.Name);
        }

        [TestMethod]
        public async Task GetUsersAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetUsersAsync(new[] { "theindra5" });
            Assert.IsTrue(answer.Length == 1);
            Assert.AreEqual("1022468464513089536", answer[0].Id);
            Assert.AreEqual("TheIndra5", answer[0].Username);
            Assert.AreEqual("Indra", answer[0].Name);
        }

        [TestMethod]
        public async Task GetUsersByIdsAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetUsersByIdsAsync(new[] { "1022468464513089536" });
            Assert.IsTrue(answer.Length == 1);
            Assert.AreEqual("1022468464513089536", answer[0].Id);
            Assert.AreEqual("TheIndra5", answer[0].Username);
            Assert.AreEqual("Indra", answer[0].Name);
        }

        [TestMethod]
        public async Task GetUserWithOptionsAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetUsersAsync(new[] { "theindra5" }, new UserSearchOptions
            {
                UserOptions = new[] { UserOption.Description, UserOption.Public_Metrics }
            });
            Assert.IsTrue(answer.Length == 1);
            Assert.AreEqual("1022468464513089536", answer[0].Id);
            Assert.AreEqual("TheIndra5", answer[0].Username);
            Assert.AreEqual("Indra", answer[0].Name);
            Assert.IsNotNull(answer[0].Description);
            Assert.IsNotNull(answer[0].PublicMetrics);
            Assert.IsNull(answer[0].Verified);
        }
    }
}
