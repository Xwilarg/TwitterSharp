using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using TwitterSharp.Client;
using TwitterSharp.Request;
using TwitterSharp.Rule;

namespace TwitterSharp.UnitTests
{
    [TestClass]
    public class TestStream
    {
        [TestMethod]
        public async Task TestStreamProcess()
        {
            var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));

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

            elem = res.FirstOrDefault(x => x.Tag == "TwitterSharp UnitTest");
            Assert.IsTrue(res.Length == objectiveCount);
            Assert.IsNotNull(elem.Id);
            Assert.IsTrue(elem.Tag == "TwitterSharp UnitTest");
            Assert.IsTrue(elem.Value.ToString() == exp.ToString());

            objectiveCount--;

            Assert.IsTrue(await client.DeleteTweetStreamAsync(elem.Id) == 1);

            res = await client.GetInfoTweetStreamAsync();

            Assert.IsTrue(res.Length == objectiveCount);
            elem = res.FirstOrDefault(x => x.Tag == "TwitterSharp UnitTest");
            Assert.IsNull(elem);
        }
    }
}
