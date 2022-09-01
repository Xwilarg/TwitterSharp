using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitterSharp.Rule;

namespace TwitterSharp.UnitTests
{
    [TestClass]
    public class TestExpression
    {
        [TestMethod]
        public void TestOr()
        {
            var exp = Expression.Author("achan_UGA").Or(Expression.Author("tanigox"), Expression.Author("daidoushinove"));
            Assert.AreEqual("(from:achan_UGA OR from:tanigox OR from:daidoushinove)", exp.ToString());
        }        
        
        [TestMethod]
        public void TestAnd()
        {
            var exp = Expression.Keyword("Test Keyword").And(Expression.IsReply().Negate(), Expression.IsRetweet().Negate());
            Assert.AreEqual("(\"Test Keyword\" -is:reply -is:retweet)", exp.ToString());
        }

        [TestMethod]
        public void ToExpression()
        {
            var rules = new []
            {
                "(\"Twitter API\" OR #v2) -\"recent search\"", // Group with different types
                "((\"Twitter API\" OR #v2) -\"recent search\") OR ((grumpy cat) OR (#meme has:images -is:retweet))", // MultiLevelRule
                
                // all examples on https://developer.twitter.com/en/docs/twitter-api/tweets/filtered-stream/integrate/build-a-rule
                "#twitterapiv2",
                "\"twitter data\" has:mentions (has:media OR has:links)",
                "snow day #NoSchool",
                "grumpy OR cat OR #meme",
                "cat #meme -grumpy",
                "(grumpy cat) OR (#meme has:images)",
// !!!!!!! "skiing -(snow OR day OR noschool)", // Not good negation
                "skiing -snow -day -noschool", // good negation
// !!!!!!!                 "apple OR iphone ipad", // Mixed with no specific order
                "apple OR (iphone ipad)", // specific order
                // !!!!!!!                "ipad iphone OR android", // Mixed with no specific order
                "(iphone ipad) OR android", // specific order
                "diacrítica",
                "#cumpleaños",
                "happy",
                "(happy OR happiness) place_country:GB -birthday -is:retweet",
                "happy OR happiness",
                "(happy OR happiness) lang:en",
                "(happy OR happiness) lang:en -birthday -is:retweet",
                "(happy OR happiness OR excited OR elated) lang:en -birthday -is:retweet",
                "(happy OR happiness OR excited OR elated) lang:en -birthday -is:retweet -holidays",
                "pepsi OR cola OR \"coca cola\"",
                "(\"Twitter API\" OR #v2) -\"filtered stream\"",
// !!!!!!!                  "#thankunext #fanart OR @arianagrande", // Mixed with no specific order
                "(@twitterdev OR @twitterapi) -@twitter",
                "$twtr OR @twitterdev -$fb",
                "from:twitterdev OR from:twitterapi -from:twitter",
                "to:twitterdev OR to:twitterapi -to:twitter",
                "from:TwitterDev url:\"https://developer.twitter.com\"",
                "from:TwitterDev url:\"https://t.co\"",
                "url:\"https://developer.twitter.com\"",
                "retweets_of:twitterdev OR retweets_of:twitterapi",
                "context:10.799022225751871488",
                "context:47.*",
                "context:*.799022225751871488",
                "entity:\"string declaration of entity/place\"",
                "entity:\"Michael Jordan\" OR entity:\"Barcelona\"",
                "conversation_id:1334987486343299072 (from:twitterdev OR from:twitterapi)",
                "bio:developer OR bio:\"data engineer\" OR bio:academic",
                "bio_name:phd OR bio_name:md",
                "bio_location:\"big apple\" OR bio_location:nyc OR bio_location:manhattan",
                "place:\"new york city\" OR place:seattle OR place:fd70c22040963ac7",
                "place_country:US OR place_country:MX OR place_country:CA",
                "point_radius:[2.355128 48.861118 16km] OR point_radius:[-41.287336 174.761070 20mi]",
                "bounding_box:[west_long south_lat east_long north_lat]",
                "bounding_box:[-105.301758 39.964069 -105.178505 40.09455]",
                "data @twitterdev -is:retweet",
                "from:twitterdev is:reply",
                "\"sentiment analysis\" is:quote",
                "#nowplaying is:verified",
                "\"mobile games\" -is:nullcast",
                "from:twitterdev -has:hashtags",
                "#stonks has:cashtags",
                "(kittens OR puppies) has:media",
                "#meme has:images",
                "#icebucketchallenge has:video_link",
                "recommend #paris has:geo -bakery",
                "#nowplaying @spotify sample:15",
                "recommend #paris lang:en",
                "followers_count:1000..10000",
                "tweets_count:1000..10000",
                "following_count:500",
                "following_count:1000..10000",
                "listed_count:10",
                "listed_count:10..100",
                "url_title:snow",
                "url_description:weather",
                "url_contains:photos",
                "source:\"Twitter for iPhone\"",
                "in_reply_to_tweet_id:1539382664746020864",
                "retweets_of_tweet_id:1539382664746020864",
            };

            foreach (var rule in rules)
            {
                var expression = Expression.ToExpression(rule);
                var expressionString = expression.Type == ExpressionType.And || expression.Type == ExpressionType.Or ? expression.ToString().Substring(1, expression.ToString().Length - 2) : expression.ToString();
                Assert.AreEqual(rule, expressionString);
            }
        }
    }
}
