using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using TwitterSharp.Client;
using TwitterSharp.Request.Option;
using TwitterSharp.Response.RUser;

namespace TwitterSharp.UnitTests
{
    [TestClass]
    public class TestLikesByUser
    {
        [TestMethod]
        public async Task GetLikesByUser()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var likedTweets = await client.GetLikedTweets("1611314460399902720", new TweetSearchOptions
            {
                Limit = 5
            });
            Assert.IsTrue(likedTweets.Any(x => x.Id == "1636441616071180312"));
        }
    }
}
