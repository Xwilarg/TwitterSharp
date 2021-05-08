using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using TwitterSharp.Client;
using TwitterSharp.Request.AdvancedSearch;
using TwitterSharp.Response.RMedia;

namespace TwitterSharp.UnitTests
{
    [TestClass]
    public class TestMedia
    {
        [TestMethod]
        public async Task GetTweetWithoutMedia()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsAsync("1390905509167853575");
            Assert.IsTrue(answer.Length == 1);
            var a = answer[0];
            Assert.IsNull(a.Attachments);
        }

        [TestMethod]
        public async Task GetTweetWithMediaId()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsAsync(new[] { "1390905509167853575" }, new[] { TweetOption.AttachmentsIds }, null);
            Assert.IsTrue(answer.Length == 1);
            var a = answer[0];
            Assert.IsNotNull(a.Attachments);
            Assert.IsNotNull(a.Attachments.Media);
            Assert.AreEqual(1, a.Attachments.Media.Length);
            Assert.AreEqual("3_1390905504537268224", a.Attachments.Media[0].Key);
            Assert.IsNull(a.Attachments.Media[0].Type);
        }

        [TestMethod]
        public async Task GetTweetWithMedia()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsAsync(new[] { "1390905509167853575" }, new[] { TweetOption.Attachments }, null);
            Assert.IsTrue(answer.Length == 1);
            var a = answer[0];
            Assert.IsNotNull(a.Attachments);
            Assert.IsNotNull(a.Attachments.Media);
            Assert.AreEqual(1, a.Attachments.Media.Length);
            Assert.AreEqual("3_1390905504537268224", a.Attachments.Media[0].Key);
            Assert.IsNotNull(a.Attachments.Media[0].Type);
            Assert.AreEqual(MediaType.Photo, a.Attachments.Media[0].Type);
        }
    }
}
