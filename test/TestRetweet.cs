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
    public class TestRetweet
    {
        private async Task<bool> ContainsUserAsync(string username, RList<User> rUsers)
        {
            if (rUsers.Data.Any(x => x.Username == username))
            {
                return true;
            }
            if (rUsers.NextAsync == null)
            {
                return false;
            }
            return await ContainsUserAsync(username, await rUsers.NextAsync());
        }

        [TestMethod]
        public async Task GetTweetRetweets()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetRetweetsAsync("1390699421726253063", new UserSearchOptions
            {
                Limit = 100
            });
            Assert.IsTrue(await ContainsUserAsync("iamachibi", answer));
        }
    }
}
