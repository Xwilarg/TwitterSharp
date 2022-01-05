using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using TwitterSharp.Client;

namespace TwitterSharp.UnitTests
{
    [TestClass]
    public class TestFollow
    {
        [TestMethod]
        public async Task GetUserFollowers()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetFollowersAsync("1433657158067896325");
            Assert.IsTrue(answer.Any(x => x.Username == "shirakamifubuki"));
        }

        [TestMethod]
        public async Task GetUserFollowing()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetFollowingAsync("1433657158067896325");
            Assert.IsTrue(answer.Any(x => x.Username == "cover_corp"));
        }
    }
}
