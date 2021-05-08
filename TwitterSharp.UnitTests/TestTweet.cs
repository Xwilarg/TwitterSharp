using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using TwitterSharp.Client;
using TwitterSharp.Request.AdvancedSearch;
using TwitterSharp.Response.RTweet;

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
        public async Task GetTweetByIdAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetByIdAsync("1389189291582967809");
            Assert.AreEqual("1389189291582967809", answer.Id);
            Assert.AreEqual("たのしみ！！\uD83D\uDC93 https://t.co/DgBYVYr9lN", answer.Text);
            Assert.IsNull(answer.Author);
            Assert.IsNull(answer.PossiblySensitive);
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

        [TestMethod]
        public async Task GetTweetWithNothing()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsByIdsAsync("1390702559086596101");

            Assert.IsTrue(answer.Length == 1);
            var a = answer[0];
            Assert.IsNull(a.ConversationId);
            Assert.IsNull(a.CreatedAt);
            Assert.IsNull(a.Entities);
            Assert.IsNull(a.InReplyToUserId);
            Assert.IsNull(a.Lang);
            Assert.IsNull(a.PossiblySensitive);
            Assert.IsNull(a.PublicMetrics);
            Assert.IsNull(a.ReferencedTweets);
            Assert.IsNull(a.ReplySettings);
            Assert.IsNull(a.Source);
        }

        [TestMethod]
        public async Task GetTweetWithAuthorId()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsByIdsAsync(new[] { "1390738061797953536" }, new TweetOption[] { TweetOption.Conversation_Id }, null);

            Assert.IsTrue(answer.Length == 1);
            var a = answer[0];
            Assert.IsNotNull(a.ConversationId);
            Assert.AreEqual("1390736182720430082", a.ConversationId);
            Assert.IsNull(a.CreatedAt);
            Assert.IsNull(a.Entities);
            Assert.IsNull(a.InReplyToUserId);
            Assert.IsNull(a.Lang);
            Assert.IsNull(a.PossiblySensitive);
            Assert.IsNull(a.PublicMetrics);
            Assert.IsNull(a.ReferencedTweets);
            Assert.IsNull(a.ReplySettings);
            Assert.IsNull(a.Source);
        }

        [TestMethod]
        public async Task GetTweetWithCreatedAt()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsByIdsAsync(new[] { "1390699421726253063" }, new TweetOption[] { TweetOption.Created_At }, null);

            Assert.IsTrue(answer.Length == 1);
            var a = answer[0];
            Assert.IsNull(a.ConversationId);
            Assert.IsNotNull(a.CreatedAt);
            Assert.AreEqual(new DateTime(2021, 5, 7, 16, 5, 54), a.CreatedAt);
            Assert.IsNull(a.Entities);
            Assert.IsNull(a.InReplyToUserId);
            Assert.IsNull(a.Lang);
            Assert.IsNull(a.PossiblySensitive);
            Assert.IsNull(a.PublicMetrics);
            Assert.IsNull(a.ReferencedTweets);
            Assert.IsNull(a.ReplySettings);
            Assert.IsNull(a.Source);
        }

        [TestMethod]
        public async Task GetTweetWithEntities()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsByIdsAsync(new[] { "1390699421726253063" }, new TweetOption[] { TweetOption.Entities }, null);

            Assert.IsTrue(answer.Length == 1);
            var a = answer[0];
            Assert.IsNull(a.ConversationId);
            Assert.IsNull(a.CreatedAt);
            Assert.IsNotNull(a.Entities);
            Assert.AreEqual(1, a.Entities.Hashtags.Length);
            Assert.AreEqual(0, a.Entities.Cashtags.Length);
            Assert.AreEqual(1, a.Entities.Urls.Length);
            Assert.AreEqual(0, a.Entities.Mentions.Length);
            Assert.IsNull(a.InReplyToUserId);
            Assert.IsNull(a.Lang);
            Assert.IsNull(a.PossiblySensitive);
            Assert.IsNull(a.PublicMetrics);
            Assert.IsNull(a.ReferencedTweets);
            Assert.IsNull(a.ReplySettings);
            Assert.IsNull(a.Source);
        }

        [TestMethod]
        public async Task GetTweetWithInReplyToUserId()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsByIdsAsync(new[] { "1390700491294724099" }, new TweetOption[] { TweetOption.In_Reply_To_User_Id }, null);

            Assert.IsTrue(answer.Length == 1);
            var a = answer[0];
            Assert.IsNull(a.ConversationId);
            Assert.IsNull(a.CreatedAt);
            Assert.IsNull(a.Entities);
            Assert.IsNotNull(a.InReplyToUserId);
            Assert.AreEqual("960340787782299648", a.InReplyToUserId);
            Assert.IsNull(a.Lang);
            Assert.IsNull(a.PossiblySensitive);
            Assert.IsNull(a.PublicMetrics);
            Assert.IsNull(a.ReferencedTweets);
            Assert.IsNull(a.ReplySettings);
            Assert.IsNull(a.Source);
        }

        [TestMethod]
        public async Task GetTweetWithLang()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsByIdsAsync(new[] { "1390766509610340354" }, new TweetOption[] { TweetOption.Lang }, null);

            Assert.IsTrue(answer.Length == 1);
            var a = answer[0];
            Assert.IsNull(a.ConversationId);
            Assert.IsNull(a.CreatedAt);
            Assert.IsNull(a.Entities);
            Assert.IsNull(a.InReplyToUserId);
            Assert.IsNotNull(a.Lang);
            Assert.AreEqual("ja", a.Lang);
            Assert.IsNull(a.PossiblySensitive);
            Assert.IsNull(a.PublicMetrics);
            Assert.IsNull(a.ReferencedTweets);
            Assert.IsNull(a.ReplySettings);
            Assert.IsNull(a.Source);
        }

        [TestMethod]
        public async Task GetTweetWithPossiblySensitive()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsByIdsAsync(new[] { "1390729374140289024" }, new TweetOption[] { TweetOption.Possibly_Sensitive }, null);

            Assert.IsTrue(answer.Length == 1);
            var a = answer[0];
            Assert.IsNull(a.ConversationId);
            Assert.IsNull(a.CreatedAt);
            Assert.IsNull(a.Entities);
            Assert.IsNull(a.InReplyToUserId);
            Assert.IsNull(a.Lang);
            Assert.IsNotNull(a.PossiblySensitive);
            Assert.IsTrue(a.PossiblySensitive.Value);
            Assert.IsNull(a.PublicMetrics);
            Assert.IsNull(a.ReferencedTweets);
            Assert.IsNull(a.ReplySettings);
            Assert.IsNull(a.Source);
        }

        [TestMethod]
        public async Task GetTweetWithPublicMetrics()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsByIdsAsync(new[] { "1390728256148500480" }, new TweetOption[] { TweetOption.Public_Metrics }, null);

            Assert.IsTrue(answer.Length == 1);
            var a = answer[0];
            Assert.IsNull(a.ConversationId);
            Assert.IsNull(a.CreatedAt);
            Assert.IsNull(a.Entities);
            Assert.IsNull(a.InReplyToUserId);
            Assert.IsNull(a.Lang);
            Assert.IsNull(a.PossiblySensitive);
            Assert.IsNotNull(a.PublicMetrics);
            Assert.IsTrue(a.PublicMetrics.LikeCount > 0);
            Assert.IsTrue(a.PublicMetrics.ReplyCount > 0);
            Assert.IsTrue(a.PublicMetrics.RetweetCount > 0);
            Assert.IsNull(a.ReferencedTweets);
            Assert.IsNull(a.ReplySettings);
            Assert.IsNull(a.Source);
        }

        [TestMethod]
        public async Task GetTweetWithReferencedTweets()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsByIdsAsync(new[] { "1387457640414859267" }, new TweetOption[] { TweetOption.Referenced_Tweets }, null);

            Assert.IsTrue(answer.Length == 1);
            var a = answer[0];
            Assert.IsNull(a.ConversationId);
            Assert.IsNull(a.CreatedAt);
            Assert.IsNull(a.Entities);
            Assert.IsNull(a.InReplyToUserId);
            Assert.IsNull(a.Lang);
            Assert.IsNull(a.PossiblySensitive);
            Assert.IsNull(a.PublicMetrics);
            Assert.IsNotNull(a.ReferencedTweets);
            Assert.AreEqual("1387454731212103680", a.ReferencedTweets[0].Id);
            Assert.IsNull(a.ReplySettings);
            Assert.IsNull(a.Source);
        }

        [TestMethod]
        public async Task GetTweetWithReplySettings()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsByIdsAsync(new[] { "1390650205440147457" }, new TweetOption[] { TweetOption.Reply_Settings }, null);

            Assert.IsTrue(answer.Length == 1);
            var a = answer[0];
            Assert.IsNull(a.ConversationId);
            Assert.IsNull(a.CreatedAt);
            Assert.IsNull(a.Entities);
            Assert.IsNull(a.InReplyToUserId);
            Assert.IsNull(a.Lang);
            Assert.IsNull(a.PossiblySensitive);
            Assert.IsNull(a.PublicMetrics);
            Assert.IsNull(a.ReferencedTweets);
            Assert.IsNotNull(a.ReplySettings);
            Assert.AreEqual(ReplySettings.Everyone, a.ReplySettings);
            Assert.IsNull(a.Source);
        }

        [TestMethod]
        public async Task GetTweetWithSource()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsByIdsAsync(new[] { "1390650205440147457" }, new TweetOption[] { TweetOption.Source }, null);

            Assert.IsTrue(answer.Length == 1);
            var a = answer[0];
            Assert.IsNull(a.ConversationId);
            Assert.IsNull(a.CreatedAt);
            Assert.IsNull(a.Entities);
            Assert.IsNull(a.InReplyToUserId);
            Assert.IsNull(a.Lang);
            Assert.IsNull(a.PossiblySensitive);
            Assert.IsNull(a.PublicMetrics);
            Assert.IsNull(a.ReferencedTweets);
            Assert.IsNull(a.ReplySettings);
            Assert.IsNotNull(a.Source);
            Assert.AreEqual("Twitter for iPhone", a.Source);
        }
    }
}
