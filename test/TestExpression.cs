using System;
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
        public void NegateIsNotNullcast()
        {
            var exp = Expression.Keyword("foo").And(Expression.IsNotNullcast().Negate());
            Assert.AreEqual("(foo -is:nullcast)", exp.ToString());
        }

        [TestMethod]
        public void PreventDoubleNegate()
        {
            var exp = Expression.Keyword("foo").And(Expression.Keyword("bar").Negate().Negate().Negate());
            Assert.AreEqual("(foo -bar)", exp.ToString());
        }      
        
        [TestMethod]
        public void EmptyGroup()
        { 
            var exp = Expression.Keyword("foo").Or(Array.Empty<Expression>());
            Assert.AreEqual("foo", exp.ToString());
        } 
        
        [TestMethod]
        public void FindExpressionsByType()
        {
            var exp1 = Expression.Keyword("Twitter API");
            var exp2 = Expression.Hashtag("v2");
            var expA = exp1.Or(exp2);
            var exp3 = Expression.Keyword("recent search");
            var exp3B = exp3.Negate();
            var expB = expA.And(exp3B);
            var exp4 = Expression.Keyword("grumpy");
            var exp5 = Expression.Keyword("cat");
            var expC = exp4.And(exp5);
            var exp6 = Expression.Hashtag("meme");
            var exp7 = Expression.HasImages();
            var exp8 = Expression.IsRetweet();
            var exp8B = exp8.Negate();
            var expD = exp6.And(exp7, exp8B);
            var expE = expC.Or(expD);
            var expF = expB.Or(expE);

            int CountExpressionsOfType(Expression expression, ExpressionType type)
            {
                var i = 0;

                if (expression.Type == type)
                {
                    i++;
                }

                if (expression.Expressions != null)
                {
                    foreach (var exp in expression.Expressions)
                    {
                        i += CountExpressionsOfType(exp, type);
                    }
                }
                
                return i;
            }

            Assert.AreEqual(CountExpressionsOfType(expF, ExpressionType.Hashtag), 2);
            Assert.AreEqual(CountExpressionsOfType(expF, ExpressionType.Keyword), 4);
            Assert.AreEqual(CountExpressionsOfType(expF, ExpressionType.And), 3);
            Assert.AreEqual(CountExpressionsOfType(expF, ExpressionType.HasImages), 1);
            Assert.AreEqual(CountExpressionsOfType(expF, ExpressionType.IsRetweet), 1);
        }

        [TestMethod]
        public void FailParseExpression()
        {
            try
            {
                Expression.Parse("expression to fail: following_count:aaa..bbbb");
                Assert.Fail("Test MUST fail! Not failed");
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(ArgumentOutOfRangeException));
            }
        }

        [TestMethod]
        public void ToExpression()
        {
            var rules = new ExpressionTest[]
            {
                new ("(\"Twitter API\" OR #v2) -\"recent search\""), // Group with different types
                new ("((\"Twitter API\" OR #v2) -\"recent search\") OR ((grumpy cat) OR (#meme has:images -is:retweet))"), // MultiLevelRule
                new ("apple OR iphone ipad OR iAd iCloud iCar", "apple OR (iphone ipad) OR (iAd iCloud iCar)"), // Mixed with no specific order
               
                // all examples on https://developer.twitter.com/en/docs/twitter-api/tweets/filtered-stream/integrate/build-a-rule
                new ("#twitterapiv2"),
                new ("\"twitter data\" has:mentions (has:media OR has:links)"),
                new ("snow day #NoSchool"),
                new ("grumpy OR cat OR #meme"),
                new ("cat #meme -grumpy"),
                new ("(grumpy cat) OR (#meme has:images)"),
                new ("skiing -(snow OR day OR noschool)"), // not good negation, but valid
                new ("skiing -snow -day -noschool"), // good negation
                new ("apple OR iphone ipad", "apple OR (iphone ipad)"), // Mixed with no specific order
                new ("apple OR (iphone ipad)"), // specific order
                new ("ipad iphone OR android", "(ipad iphone) OR android"), // Mixed with no specific order, but valid
                new ("(iphone ipad) OR android"), // specific order
                new ("diacrítica"),
                new ("#cumpleaños"),
                new ("happy"),
                new ("(happy OR happiness) place_country:GB -birthday -is:retweet"),
                new ("happy OR happiness"),
                new ("(happy OR happiness) lang:en"),
                new ("(happy OR happiness) lang:en -birthday -is:retweet"),
                new ("(happy OR happiness OR excited OR elated) lang:en -birthday -is:retweet"),
                new ("(happy OR happiness OR excited OR elated) lang:en -birthday -is:retweet -holidays"),
                new ("pepsi OR cola OR \"coca cola\""),
                new ("(\"Twitter API\" OR #v2) -\"filtered stream\""),
                new ("#thankunext #fanart OR @arianagrande", "(#thankunext #fanart) OR @arianagrande"), // Mixed with no specific order
                new ("(@twitterdev OR @twitterapi) -@twitter"),
                new ("$twtr OR @twitterdev -$fb", "$twtr OR (@twitterdev -$fb)"), // Mixed with no specific order, but valid
                new ("from:twitterdev OR from:twitterapi -from:twitter", "from:twitterdev OR (from:twitterapi -from:twitter)"), // Mixed with no specific order, but valid
                new ("to:twitterdev OR to:twitterapi -to:twitter", "to:twitterdev OR (to:twitterapi -to:twitter)"), // Mixed with no specific order, but valid
                new ("from:TwitterDev url:\"https://developer.twitter.com\""),
                new ("from:TwitterDev url:\"https://t.co\""), 
                new ("url:\"https://developer.twitter.com\""),
                new ("retweets_of:twitterdev OR retweets_of:twitterapi"),
                new ("context:10.799022225751871488"),
                new ("context:47.*"),
                new ("context:*.799022225751871488"),
                new ("entity:\"string declaration of entity/place\""),
                new ("entity:\"Michael Jordan\" OR entity:\"Barcelona\""), // quote where no quotes needed
                new ("conversation_id:1334987486343299072 (from:twitterdev OR from:twitterapi)"),
                new ("bio:developer OR bio:\"data engineer\" OR bio:academic"),
                new ("bio_name:phd OR bio_name:md"),
                new ("bio_location:\"big apple\" OR bio_location:nyc OR bio_location:manhattan"),
                new ("place:\"new york city\" OR place:seattle OR place:fd70c22040963ac7"),
                new ("place_country:US OR place_country:MX OR place_country:CA"),
                new ("point_radius:[2.355128 48.861118 16km] OR point_radius:[-41.287336 174.761070 20mi]"),
                new ("bounding_box:[-105.301758 39.964069 -105.178505 40.09455]"),
                new ("data @twitterdev -is:retweet"),
                new ("from:twitterdev is:reply"),
                new ("\"sentiment analysis\" is:quote"),
                new ("#nowplaying is:verified"),
                new ("\"mobile games\" -is:nullcast"),
                new ("from:twitterdev -has:hashtags"),
                new ("#stonks has:cashtags"),
                new ("(kittens OR puppies) has:media"),
                new ("#meme has:images"),
                new ("#icebucketchallenge has:video_link", "#icebucketchallenge has:videos"), // Alias handling
                new ("recommend #paris has:geo -bakery"),
                new ("#nowplaying @spotify sample:15"),
                new ("recommend #paris lang:en"),
                new ("followers_count:1000..10000"),
                new ("tweets_count:1000..10000"),
                new ("following_count:500"),
                new ("following_count:1000..10000"),
                new ("listed_count:10"),
                new ("listed_count:10..100"),
                new ("url_title:snow"),
                new ("url_description:weather"),
                new ("url_contains:photos"),
                new ("source:\"Twitter for iPhone\""),
                new ("in_reply_to_tweet_id:1539382664746020864"),
                new ("retweets_of_tweet_id:1539382664746020864"),
            };

            foreach (var rule in rules)
            {
                var expression = Expression.Parse(rule.ExpressionString);
                // strip extra brackets
                var expressionString = (expression.Type == ExpressionType.And || expression.Type == ExpressionType.Or) && !expression.IsNegate ? expression.ToString().Substring(1, expression.ToString().Length - 2) : expression.ToString();
                Assert.AreEqual(rule.ExpectedString, expressionString);
            }
        }

        private class ExpressionTest
        {
            public string ExpressionString { get; set; }
            public string ExpectedString { get; set; }
            public ExpressionTest(string expressionString, string expectedString = "")
            {
                ExpressionString = expressionString;
                ExpectedString = expectedString == "" ? expressionString : expectedString;
            }
        }
    }
}