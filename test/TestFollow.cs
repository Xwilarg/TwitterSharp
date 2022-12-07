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
    public class TestFollow
    {
        private async Task<bool> ContainsFollowAsync(string username, RUsers rUsers)
        {
            if (rUsers.Users.Any(x => x.Username == username))
            {
                return true;
            }
            if (rUsers.NextAsync == null)
            {
                return false;
            }
            return await ContainsFollowAsync(username, await rUsers.NextAsync());
        }

        [TestMethod]
        public async Task GetUserFollowers()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetFollowersAsync("1022468464513089536", new UserSearchOptions
            {
                Limit = 1000
            });
            Assert.IsTrue(await ContainsFollowAsync("CoreDesign_com", answer));
        }

        [TestMethod]
        public async Task GetUserFollowing()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetFollowingAsync("1433657158067896325", new UserSearchOptions
            {
                Limit = 1000
            });
            Assert.IsTrue(await ContainsFollowAsync("cover_corp", answer));
        }
    }
}
