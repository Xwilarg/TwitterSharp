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
            var answer = await client.GetTweetsAsync("1237543996861251586");
            Assert.IsTrue(answer.Length == 1);
            var a = answer[0];
            Assert.IsNull(a.Attachments);
        }

        [TestMethod]
        public async Task GetTweetWithMediaId()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsAsync(new[] { "1237543996861251586" }, new[] { TweetOption.AttachmentsIds }, null, null);
            Assert.IsTrue(answer.Length == 1);
            var a = answer[0];
            Assert.IsNotNull(a.Attachments);
            Assert.IsNotNull(a.Attachments.Media);
            Assert.AreEqual(1, a.Attachments.Media.Length);
            Assert.AreEqual("7_1237543944570847233", a.Attachments.Media[0].Key);
            Assert.IsNull(a.Attachments.Media[0].Type);
            Assert.IsNull(a.Attachments.Media[0].PreviewImageUrl);
        }

        [TestMethod]
        public async Task GetTweetWithMedia()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsAsync(new[] { "1237543996861251586" }, new[] { TweetOption.Attachments }, null, null);
            Assert.IsTrue(answer.Length == 1);
            var a = answer[0];
            Assert.IsNotNull(a.Attachments);
            Assert.IsNotNull(a.Attachments.Media);
            Assert.AreEqual(1, a.Attachments.Media.Length);
            Assert.AreEqual("7_1237543944570847233", a.Attachments.Media[0].Key);
            Assert.IsNotNull(a.Attachments.Media[0].Type);
            Assert.AreEqual(MediaType.Video, a.Attachments.Media[0].Type);
            Assert.IsNull(a.Attachments.Media[0].PreviewImageUrl);
        }

        [TestMethod]
        public async Task GetTweetWithMediaPreview()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsAsync(new[] { "1237543996861251586" }, new[] { TweetOption.Attachments }, null, new[] { MediaOption.Preview_Image_Url });
            Assert.IsTrue(answer.Length == 1);
            var a = answer[0];
            Assert.IsNotNull(a.Attachments);
            Assert.IsNotNull(a.Attachments.Media);
            Assert.AreEqual(1, a.Attachments.Media.Length);
            Assert.AreEqual("7_1237543944570847233", a.Attachments.Media[0].Key);
            Assert.IsNotNull(a.Attachments.Media[0].Type);
            Assert.AreEqual(MediaType.Video, a.Attachments.Media[0].Type);
            Assert.IsNotNull(a.Attachments.Media[0].PreviewImageUrl);
            Assert.AreEqual("https://pbs.twimg.com/ext_tw_video_thumb/1237543944570847233/pu/img/kRBUlSd7M7ju_QK1.jpg", a.Attachments.Media[0].PreviewImageUrl);
        }
    }
}
