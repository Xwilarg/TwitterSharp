using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using TwitterSharp.Client;
using TwitterSharp.Request.Option;
using TwitterSharp.Response;
using TwitterSharp.Response.RUser;

namespace TwitterSharp.UnitTests
{
    [TestClass]
    public class TestLike
    {
        private async Task<bool> ContainsLikeAsync(string username, RList<User> rUsers)
        {
            if (rUsers.Data.Any(x => x.Username == username))
            {
                return true;
            }
            if (rUsers.NextAsync == null)
            {
                return false;
            }
            return await ContainsLikeAsync(username, await rUsers.NextAsync());
        }

        [TestMethod]
        public async Task GetTweetLikes()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetLikesAsync("1390699421726253063", new UserSearchOptions
            {
                Limit = 100
            });
            Assert.IsTrue(await ContainsLikeAsync("daphne637", answer));
        }
    }
}
