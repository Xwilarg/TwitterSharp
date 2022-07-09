using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using TwitterSharp.Client;
using TwitterSharp.Request.AdvancedSearch;
using TwitterSharp.Request.Option;
using TwitterSharp.Response.RMedia;
using TwitterSharp.Response.RTweet;
using TwitterSharp.Rule;

namespace TwitterSharp.UnitTests
{
    [TestClass]
    public class TestTweet
    {
        [TestMethod]
        public async Task GetTweetByIdsAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsAsync(new[] { "1389189291582967809" });
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
            var answer = await client.GetTweetAsync("1389189291582967809");
            Assert.AreEqual("1389189291582967809", answer.Id);
            Assert.AreEqual("たのしみ！！\uD83D\uDC93 https://t.co/DgBYVYr9lN", answer.Text);
            Assert.IsNull(answer.Author);
            Assert.IsNull(answer.PossiblySensitive);
        }

        [TestMethod]
        public async Task GetTweetByIdsWithAuthorAndSensitivityAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsAsync(new[] { "1389189291582967809" }, new TweetSearchOptions
            {
                TweetOptions = new[] { TweetOption.Possibly_Sensitive },
                UserOptions = Array.Empty<UserOption>()
            });
            Assert.IsTrue(answer.Length == 1);
            Assert.AreEqual("1389189291582967809", answer[0].Id);
            Assert.AreEqual("たのしみ！！\uD83D\uDC93 https://t.co/DgBYVYr9lN", answer[0].Text);
            Assert.IsNotNull(answer[0].Author);
            Assert.IsNotNull(answer[0].PossiblySensitive);
            Assert.AreEqual("kiryucoco", answer[0].Author.Username);
            Assert.IsFalse(answer[0].PossiblySensitive.Value);
        }

        [TestMethod]
        public async Task GetTweetsAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsAsync(new[] { "1389330151779930113", "1389331863102128130" });
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
            var answer = await client.GetTweetsAsync(new[] { "1389330151779930113", "1389331863102128130" }, new()
            {
                UserOptions = Array.Empty<UserOption>()
            });
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
            Assert.AreEqual(10, answer.Length);
            Assert.IsNull(answer[0].Author);
        }

        [TestMethod]
        public async Task GetTweetsFromUserIdWithSinceIdAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsFromUserIdAsync("1200397238788247552", new TweetSearchOptions
            {
                SinceId = "1410551383795781634"
            });
            Assert.AreEqual(2, answer.Length);
        }

        [TestMethod]
        public async Task GetTweetsFromUserIdWithStartTimeAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsFromUserIdAsync("1200397238788247552", new TweetSearchOptions
            {
                StartTime = new DateTime(2021, 7, 1, 12, 50, 0)
            });
            Assert.AreEqual(1, answer.Length);
        }

        [TestMethod]
        public async Task GetTweetsFromUserIdWithArgumentsAsync() // Issue #2
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsFromUserIdAsync("1109748792721432577", new TweetSearchOptions
            {
                TweetOptions = new[] { TweetOption.Attachments },
                UserOptions = Array.Empty<UserOption>()
            });
            Assert.AreEqual(10, answer.Length);
            Assert.IsNotNull(answer[0].Author);
        }

        [TestMethod]
        public async Task GetTweetsFromUserIdWithAuthorAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsFromUserIdAsync("1109748792721432577", new TweetSearchOptions
            {
                UserOptions = Array.Empty<UserOption>()
            });
            Assert.IsTrue(answer.Length == 10);
            foreach (var t in answer)
            {
                Assert.IsNotNull(t.Author);
                Assert.AreEqual("inugamikorone", t.Author.Username);
            }
        }
            
        [TestMethod]
        public async Task GetTweetsFromUserIdWithModifiedLimitAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var answer = await client.GetTweetsFromUserIdAsync("1109748792721432577", new()
            {
                Limit = 100
            });
            Assert.IsTrue(answer.Length == 100);
        }

        [TestMethod]
        public async Task GetTweetWithNothing()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var a = await client.GetTweetAsync("1390702559086596101");

            Assert.IsNull(a.Attachments);
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
        public async Task GetTweetWithAttachment()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var a = await client.GetTweetAsync("1481598340051914753", new TweetSearchOptions
            {
                TweetOptions = new[] { TweetOption.Attachments },
                MediaOptions = new[] { MediaOption.Url }
            });

            Assert.IsNotNull(a.Attachments);
            Assert.IsNotNull(a.Attachments.Media);
            Assert.IsTrue(a.Attachments.Media.Length == 1);
            Assert.IsTrue(a.Attachments.Media[0].Type == MediaType.Photo);
            Assert.IsTrue(a.Attachments.Media[0].Url == "https://pbs.twimg.com/media/FI-xd52aUAEHCJB.jpg");
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
        public async Task GetTweetWithConversationId()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            var a = await client.GetTweetAsync("1390738061797953536", new TweetSearchOptions
            {
                TweetOptions = new[] { TweetOption.Conversation_Id }
            });

            Assert.IsNull(a.Attachments);
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
            var a = await client.GetTweetAsync("1390699421726253063", new TweetSearchOptions
            {
                TweetOptions = new[] { TweetOption.Created_At }
            });

            Assert.IsNull(a.Attachments);
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
            var a = await client.GetTweetAsync("1390699421726253063", new TweetSearchOptions
            {
                TweetOptions = new[] { TweetOption.Entities }
            });

            Assert.IsNull(a.Attachments);
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
            var a = await client.GetTweetAsync("1390700491294724099", new TweetSearchOptions
            {
                TweetOptions = new[] { TweetOption.In_Reply_To_User_Id }
            });

            Assert.IsNull(a.Attachments);
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
            var a = await client.GetTweetAsync("1390766509610340354", new TweetSearchOptions
            {
                TweetOptions = new[] { TweetOption.Lang }
            });

            Assert.IsNull(a.Attachments);
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
            var a = await client.GetTweetAsync("1373191601154007040", new TweetSearchOptions
            {
                TweetOptions = new[] { TweetOption.Possibly_Sensitive }
            });

            Assert.IsNull(a.Attachments);
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
            var a = await client.GetTweetAsync("1390728256148500480", new TweetSearchOptions
            {
                TweetOptions = new[] { TweetOption.Public_Metrics }
            });

            Assert.IsNull(a.Attachments);
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
            var a = await client.GetTweetAsync("1387457640414859267", new TweetSearchOptions
            {
                TweetOptions = new[] { TweetOption.Referenced_Tweets }
            });

            Assert.IsNull(a.Attachments);
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
            var a = await client.GetTweetAsync("1390650205440147457", new TweetSearchOptions
            {
                TweetOptions = new[] { TweetOption.Reply_Settings }
            });

            Assert.IsNull(a.Attachments);
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
            var a = await client.GetTweetAsync("1390650205440147457", new TweetSearchOptions
            {
                TweetOptions = new[] { TweetOption.Source }
            });

            Assert.IsNull(a.Attachments);
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

        [TestMethod]
        public async Task GetRecentTweets()
        {
            var hashtag = "Test";
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));

            // note: retweets are truncated at 140 characters, so i had to exclude them for making the check reliable
            var a = await client.GetRecentTweets(Expression.Hashtag(hashtag).And(Expression.IsRetweet().Negate()));
            
            Assert.IsTrue(a.All(x => x.Text.Contains("#"+hashtag, StringComparison.InvariantCultureIgnoreCase)));
        }

        [TestMethod]
        public async Task GetTweetByIdErrorAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            try
            {
                await client.GetTweetAsync("FALSE_TWEET_ID");
            }
            catch (TwitterException e)
            {
                Assert.IsTrue(e.Errors != null);
                Assert.IsTrue(e.Errors.Length == 1);
                Assert.AreEqual("Invalid Request", e.Title);
            }
        }
        
        [TestMethod]
        public async Task GetTweetsByIdErrorAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            try
            {
                await client.GetTweetsAsync(new [] {"FALSE_TWEET_ID", "FALSE_TWEET_ID2"});
            }
            catch (TwitterException e)
            {
                Assert.IsTrue(e.Errors != null);
                Assert.IsTrue(e.Errors.Length == 2);
                Assert.AreEqual("Invalid Request", e.Title);
            }
        }
        
        [TestMethod]
        public async Task GetTweetsFromUserIdErrorAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            try
            {
                await client.GetTweetsFromUserIdAsync("FALSE_USER_ID");
            }
            catch (TwitterException e)
            {
                Assert.IsTrue(e.Errors != null);
                Assert.IsTrue(e.Errors.Length == 1);
                Assert.AreEqual("Invalid Request", e.Title);
            }
        }
        
        [TestMethod]
        public async Task GetTweetsFromNotFoundUserIdErrorAsync()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            try
            {
                await client.GetTweetsFromUserIdAsync("1474083406862782466"); // Not found
            }
            catch (TwitterException e)
            {
                Assert.IsTrue(e.Errors != null);
                Assert.IsTrue(e.Errors.Length == 1);
                Assert.AreEqual("id", e.Errors.First().Parameter);
                Assert.AreEqual("Not Found Error", e.Errors.First().Title);
            }
        }
    }
}
