using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TwitterSharp.Client;
using TwitterSharp.Request;
using TwitterSharp.Response;
using TwitterSharp.Rule;

namespace TwitterSharp.UnitTests
{
    [TestClass]
    public class TestStream
    {
        [TestMethod]
        public async Task TestStreamProcess()
        {
            List<RateLimit> rateLimitEvents = new();

            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));
            
            client.RateLimitChanged += (_, rateLimit) =>
            {
                rateLimitEvents.Add(rateLimit);
            };

            var res = await client.GetInfoTweetStreamAsync();
            var elem = res.FirstOrDefault(x => x.Tag == "TwitterSharp UnitTest");

            var objectiveCount = res.Length + 1;

            if (elem != null)
            {
                await client.DeleteTweetStreamAsync(elem.Id);
                objectiveCount--;
            }

            var exp = Expression.Author("arurandeisu");
            res = await client.AddTweetStreamAsync(new StreamRequest(exp, "TwitterSharp UnitTest"));

            Assert.IsTrue(res.Length == 1);
            Assert.IsTrue(res[0].Tag == "TwitterSharp UnitTest");
            Assert.IsTrue(res[0].Value.ToString() == exp.ToString());

            res = await client.GetInfoTweetStreamAsync();
            
            Assert.IsTrue(CheckGetInfoTweetStreamAsyncRateLimit(rateLimitEvents));

            elem = res.FirstOrDefault(x => x.Tag == "TwitterSharp UnitTest");
            Assert.IsTrue(res.Length == objectiveCount);
            Assert.IsNotNull(elem.Id);
            Assert.IsTrue(elem.Tag == "TwitterSharp UnitTest");
            Assert.IsTrue(elem.Value.ToString() == exp.ToString());

            objectiveCount--;

            Assert.IsTrue(await client.DeleteTweetStreamAsync(elem.Id) == 1);

            res = await client.GetInfoTweetStreamAsync();

            Assert.IsTrue(CheckGetInfoTweetStreamAsyncRateLimit(rateLimitEvents));

            Assert.IsTrue(res.Length == objectiveCount);
            elem = res.FirstOrDefault(x => x.Tag == "TwitterSharp UnitTest");
            Assert.IsNull(elem);
        }

        private bool CheckGetInfoTweetStreamAsyncRateLimit(List<RateLimit> rateLimitEvents)
        {
            var rateLimits = rateLimitEvents.Where(x => x.Endpoint == nameof(TwitterClient.GetInfoTweetStreamAsync)).ToList();

            return rateLimits[^1].Remaining == rateLimits[^2].Remaining - 1;
        }
    }
}
