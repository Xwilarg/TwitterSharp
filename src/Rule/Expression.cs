using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace TwitterSharp.Rule
{
    // https://developer.twitter.com/en/docs/twitter-api/tweets/filtered-stream/integrate/build-a-rule
    public class Expression
    {
        internal Expression(string prefix, string userInput)
        {
            _internal = prefix + (userInput != null ? (userInput.Contains(' ') && !userInput.StartsWith('\"') && !userInput.EndsWith('\"') ? "\"" + userInput + "\"" : userInput) : "");
        }

        /// <summary>
        /// Constructor for building single expressions
        /// </summary>
        /// <param name="prefix">Prefix like #, @, from: or is:retweet</param>
        /// <param name="userInput">Value of the expression</param>
        /// <param name="type">Type of the expression</param>
        private Expression(string prefix, string userInput, ExpressionType type) : this(prefix, userInput)
        {
            Type = type;
        }        
        
        /// <summary>
        /// Constructor for building expression tree
        /// </summary>
        /// <param name="prefix">Prefix like #, @, from: or is:retweet</param>
        /// <param name="userInput">Value of the expression</param>
        /// <param name="type">Type of the expression</param>
        /// <param name="firstExpression">first expression from And/Or logic</param>
        /// <param name="expressions">grouped expressions</param>
        private Expression(string prefix, string userInput, ExpressionType type, Expression firstExpression, Expression[] expressions) : this(prefix, userInput, type)
        {
            if (firstExpression != null && expressions is { Length: > 0 })
            {
                Expressions = new Expression[expressions.Length + 1];
                Expressions[0] = firstExpression;
                expressions.CopyTo(Expressions, 1);
            }
            else if (firstExpression != null)
            {
                Expressions = new [] { firstExpression };
            }
        }

        public ExpressionType Type { get; }
        public Expression[] Expressions { get; }
        public bool IsNegate { get; private set; }

        private string _internal;

        public override string ToString()
            => _internal;

        /// <summary>
        /// Parses the given string into an expression with type and expression tree
        /// </summary>
        /// <param name="s">Expression string following the rules from <see cref="!:https://developer.twitter.com/en/docs/twitter-api/tweets/filtered-stream/integrate/build-a-rule">Building rules for filtered stream</see></param>
        /// <returns>Expression with type and expression tree</returns>
        public static Expression Parse(string s)
        {
            const char r = '~'; // replaceCharacter
            
            // Find Quote and Replace
            // https://regex101.com/r/9P1hCA/1
            var replacements = new List<string>();
            var replacementCount = 0;

            foreach (Match match in Regex.Matches(s, "(\\\").*?(\")"))
            {
                replacements.Add(match.Value);
                s = ReplaceFirst(s, match.Value, $"{r}q{replacementCount++}{r}");
            }

            // Find coordinates and Replace
            foreach (Match match in Regex.Matches(s, @"\[.*?\]"))
            {
                replacements.Add(match.Value);
                s = ReplaceFirst(s, match.Value, $"{r}q{replacementCount++}{r}");
            }

            var expressions = new List<Expression>();
            var expressionCount = 0;

            // Finding most bottom (single) expressions
            foreach (var stringExpression in s.Replace($" OR ", " ").Replace("-(", "").Replace("(", "").Replace(")", "").Split(' '))
            {
                var isNegate = stringExpression.StartsWith('-') && !stringExpression.Equals("-is:nullcast", StringComparison.InvariantCultureIgnoreCase);
                var sr = isNegate ? stringExpression[1..] : stringExpression;

                if (stringExpression.Contains($"{r}q"))
                    sr = Regex.Replace(sr, @$"{r}q(\d+){r}", replacements[int.Parse(Regex.Match(stringExpression, @"\d+").Value)]);

                if (sr.StartsWith('#'))
                    AddExpression(Hashtag(sr[1..]));
                else if (sr.StartsWith('$'))
                    AddExpression(Cashtag(sr[1..]));
                else if (sr.StartsWith('@'))
                    AddExpression(Mention(sr[1..]));
                else if (sr.StartsWith("from:"))
                    AddExpression(Author(sr.Replace("from:", "")));
                else if (sr.StartsWith("to:"))
                    AddExpression(Recipient(sr.Replace("to:", "")));
                else if (sr.StartsWith("url:"))
                    AddExpression(Url(sr.Replace("url:", "")));
                else if (sr.StartsWith("retweets_of:"))
                    AddExpression(Retweet(sr.Replace("retweets_of:", "")));
                else if (sr.StartsWith("retweets_of_user:")) // Alias
                    AddExpression(Retweet(sr.Replace("retweets_of:", "")));
                else if (sr.StartsWith("context:"))
                    AddExpression(Context(sr.Replace("context:", "")));
                else if (sr.StartsWith("entity:"))
                    AddExpression(Entity(sr.Replace("entity:", "")));
                else if (sr.StartsWith("conversation_id:"))
                    AddExpression(ConversationId(sr.Replace("conversation_id:", "")));
                else if (sr.StartsWith("bio:"))
                    AddExpression(Bio(sr.Replace("bio:", "")));
                else if (sr.StartsWith("user_bio:")) // Alias
                    AddExpression(Bio(sr.Replace("bio:", "")));
                else if (sr.StartsWith("bio_location:"))
                    AddExpression(BioLocation(sr.Replace("bio_location:", "")));
                else if (sr.StartsWith("user_bio_location:")) // Alias
                    AddExpression(BioLocation(sr.Replace("bio_location:", "")));
                else if (sr.StartsWith("place:"))
                    AddExpression(Place(sr.Replace("place:", "")));
                else if (sr.StartsWith("place_country:"))
                    AddExpression(PlaceCountry(sr.Replace("place_country:", "")));
                else if (sr.StartsWith("point_radius:"))
                {
                    var coordinates = sr.Replace("point_radius:", "").Replace("[", "").Replace("]", "").Split(' ');
                    var unit = coordinates[2].EndsWith("km") ? RadiusUnit.Kilometer : RadiusUnit.Mile;
                    AddExpression(PointRadius(coordinates[0], coordinates[1], coordinates[2][..^2], unit));
                }
                else if (sr.StartsWith("bounding_box:"))
                {
                    var coordinates = sr.Replace("bounding_box:", "").Replace("[", "").Replace("]", "").Split(' ');
                    AddExpression(BoundingBox(coordinates[0], coordinates[1], coordinates[2], coordinates[3]));
                }
                else if (sr.StartsWith("geo_bounding_box:")) // Alias
                {
                    var coordinates = sr.Replace("geo_bounding_box:", "").Replace("[", "").Replace("]", "").Split(' ');
                    AddExpression(BoundingBox(coordinates[0], coordinates[1], coordinates[2], coordinates[3]));
                }
                else if (sr.StartsWith("sample:"))
                    AddExpression(Sample(int.Parse(sr.Replace("sample:", ""))));
                else if (sr.StartsWith("lang:"))
                    AddExpression(Lang(sr.Replace("lang:", "")));
                else if (sr == "is:retweet")
                    AddExpression(IsRetweet());
                else if (sr == "is:reply")
                    AddExpression(IsReply());
                else if (sr == "is:quote")
                    AddExpression(IsQuote());
                else if (sr == "is:verified")
                    AddExpression(IsVerified());
                else if (sr == "-is:nullcast")
                    AddExpression(IsNotNullcast());
                else if (sr == "has:hashtags")
                    AddExpression(HasHashtags());
                else if (sr == "has:cashtags")
                    AddExpression(HasCashtags());
                else if (sr == "has:links")
                    AddExpression(HasLinks());
                else if (sr == "has:mentions")
                    AddExpression(HasMentions());
                else if (sr == "has:media")
                    AddExpression(HasMedia());
                else if (sr == "has:media_link") // Alias
                    AddExpression(HasMedia());
                else if (sr == "has:images")
                    AddExpression(HasImages());
                else if (sr == "has:videos")
                    AddExpression(HasVideos());
                else if (sr == "has:video_link") // Alias
                    AddExpression(HasVideos());
                else if (sr == "has:geo")
                    AddExpression(HasGeo());
                else if (sr.StartsWith("followers_count:"))
                    AddCountExpression(sr, "followers_count:");
                else if (sr.StartsWith("tweets_count:"))
                    AddCountExpression(sr, "tweets_count:");
                else if (sr.StartsWith("statuses_count:")) // Alias
                    AddCountExpression(sr, "statuses_count:");
                else if (sr.StartsWith("following_count:"))
                    AddCountExpression(sr, "following_count:");
                else if (sr.StartsWith("friends_count:")) // Alias
                    AddCountExpression(sr, "friends_count:");
                else if (sr.StartsWith("listed_count:"))
                    AddCountExpression(sr, "listed_count:");
                else if (sr.StartsWith("user_in_lists_count:")) // Alias
                    AddCountExpression(sr, "user_in_lists_count:");
                else if (sr.StartsWith("url_title:"))
                    AddExpression(UrlTitle(sr.Replace("url_title:", "")));
                else if (sr.StartsWith("within_url_title:")) // Alias
                    AddExpression(UrlTitle(sr.Replace("within_url_title:", "")));
                else if (sr.StartsWith("url_description:"))
                    AddExpression(UrlDescription(sr.Replace("url_description:", "")));
                else if (sr.StartsWith("within_url_description:")) // Alias
                    AddExpression(UrlDescription(sr.Replace("within_url_description:", "")));
                else if (sr.StartsWith("url_contains:"))
                    AddExpression(UrlContains(sr.Replace("url_contains:", "")));
                else if (sr.StartsWith("in_reply_to_tweet_id:"))
                    AddExpression(InReplyToTweetId(long.Parse(sr.Replace("in_reply_to_tweet_id:", ""))));
                else if (sr.StartsWith("in_reply_to_status_id:"))
                    AddExpression(InReplyToTweetId(long.Parse(sr.Replace("in_reply_to_status_id:", ""))));
                else if (sr.StartsWith("retweets_of_tweet_id:"))
                    AddExpression(RetweetsOfTweetId(long.Parse(sr.Replace("retweets_of_tweet_id:", ""))));
                else if (sr.StartsWith("retweets_of_status_id:"))
                    AddExpression(RetweetsOfTweetId(long.Parse(sr.Replace("retweets_of_status_id:", ""))));
                else
                    AddExpression(Keyword(sr));

                void AddCountExpression(string countExpressionString, string searchString)
                {
                    if (countExpressionString.Contains(".."))
                    {
                        var matches = Regex.Matches(countExpressionString, @"\d+");

                        switch (searchString)
                        {
                            case "followers_count:": AddExpression(FollowersCount(int.Parse(matches[0].Value), int.Parse(matches[1].Value))); break;
                            case "tweets_count:": AddExpression(TweetsCount(int.Parse(matches[0].Value), int.Parse(matches[1].Value))); break;
                            case "statuses_count:": AddExpression(TweetsCount(int.Parse(matches[0].Value), int.Parse(matches[1].Value))); break;
                            case "following_count:": AddExpression(FollowingCount(int.Parse(matches[0].Value), int.Parse(matches[1].Value))); break;
                            case "friends_count:": AddExpression(FollowingCount(int.Parse(matches[0].Value), int.Parse(matches[1].Value))); break;
                            case "listed_count:": AddExpression(ListedCount(int.Parse(matches[0].Value), int.Parse(matches[1].Value))); break;
                            case "user_in_lists_count:": AddExpression(ListedCount(int.Parse(matches[0].Value), int.Parse(matches[1].Value))); break;
                        }
                    }
                    else if (countExpressionString.StartsWith(searchString))
                    {
                        var count = int.Parse(countExpressionString.Replace(searchString, ""));

                        switch (searchString)
                        {
                            case "followers_count:": AddExpression(FollowersCount(count)); break;
                            case "tweets_count:": AddExpression(TweetsCount(count)); break;
                            case "statuses_count:": AddExpression(TweetsCount(count)); break;
                            case "following_count:": AddExpression(FollowingCount(count)); break;
                            case "friends_count:": AddExpression(FollowingCount(count)); break;
                            case "listed_count:": AddExpression(ListedCount(count)); break;
                            case "user_in_lists_count:": AddExpression(ListedCount(count)); break;
                        }
                    }
                }

                void AddExpression(Expression exp)
                {
                    expressions.Add(isNegate ? exp.Negate() : exp);
                }

                s = ReplaceFirst(s, stringExpression, $"{r}e{expressionCount++}{r}");
            }

            string ReplaceFirst(string text, string search, string replace)
            {
                var pos = text.IndexOf(search, StringComparison.Ordinal);
                if (pos < 0)
                {
                    return text;
                }
                return text[..pos] + replace + text[(pos + search.Length)..];
            }

            // Find groups recursive
            // https://regex101.com/r/xJaODO/2
            var groups = new Dictionary<string[], bool>();

            FindGroups();

            void FindGroups()
            {
                // https://regex101.com/r/ONVk50/2
                foreach (Match match in Regex.Matches(s, "\\([^\\(]*?\\)"))
                {
                    var group = match.Value.Replace("(", "").Replace(")", "");
                    AddToGroup(group);
                    s = ReplaceFirst(s, $"({group})", $"{r}g{expressionCount++}{r}");
                }

                if (s.Contains('('))
                {
                    FindGroups();
                }
                else if (s.Contains(" OR ") && s.Contains($"{r} {r}")) // Mixed groups
                {
                    var ors = s.Split(" OR ");

                    for (int i = 0; i <= ors.Length-1; i++)
                    {
                        if (ors[i].Contains(' '))
                        {
                            ors[i] = $"({ors[i]})";
                        }
                    }

                    s = string.Join(" OR ", ors);
                    FindGroups();
                }
                else
                {
                    AddToGroup(s); // most top group should be left
                }

                void AddToGroup(string ga)
                {
                    if (ga.Contains(" OR "))
                    {
                        groups.Add(ga.Split(" OR "), false);
                    }
                    else
                    {
                        groups.Add(ga.Split(" "), true);
                    }
                }
            }

            // build expression
            foreach (var group in groups)
            {
                var groupExpression = new List<Expression>();

                foreach (var k in group.Key)
                {
                    var index = Regex.Match(k, @"\d+").Value;
                    groupExpression.Add(expressions[int.Parse(index)]);
                    
                    if (k.StartsWith('-'))
                    {
                        expressions[int.Parse(index)].Negate();
                    }
                }

                if (groupExpression.Count > 1)
                {
                    expressions.Add(group.Value // true is And / false is Or
                        ? groupExpression[0].And(groupExpression.Skip(1).ToArray())
                        : groupExpression[0].Or(groupExpression.Skip(1).ToArray()));
                }
            }

            return expressions.Last();
        }

        // LOGIC

        /// <summary>
        /// Tweet match one of the expression given in parameter
        /// </summary>
        public Expression Or(params Expression[] others)
        {
            return new(others.Any() ? "(" + _internal + " OR " + string.Join(" OR ", others.Select(x => x.ToString())) + ")" : _internal, "", ExpressionType.Or, this, others);
        }

        /// <summary>
        /// Tweet match all the expressions given in parameter
        /// </summary>
        public Expression And(params Expression[] others)
        {
            return new(others.Any() ? "(" + _internal + " " + string.Join(" ", others.Select(x => x.ToString())) + ")" : _internal, "", ExpressionType.And, this, others);
        }

        /// <summary>
        /// Tweet match the negation of the current expression
        /// </summary>
        public Expression Negate()
        {
            if (!_internal.StartsWith('-')) // prevent double negation
                _internal = "-" + _internal;

            IsNegate = true;
            return this;
        }

        // OPERATORS

        /// <summary>
        /// Match a keyword in the body of a tweet
        /// </summary>
        public static Expression Keyword(string str)
            => new("", str, ExpressionType.Keyword);

        /// <summary>
        /// Any tweet with the given hashtag
        /// </summary>
        public static Expression Hashtag(string entity)
            => new("#", entity, ExpressionType.Hashtag);

        /// <summary>
        /// Any tweet with the given cashtag
        /// </summary>
        public static Expression Cashtag(string entity)
            => new("$", entity, ExpressionType.Cashtag);

        /// <summary>
        /// Any tweet that contains a mention of the given user
        /// </summary>
        public static Expression Mention(string username)
            => new("@", username, ExpressionType.Mention);

        /// <summary>
        /// Any tweet sent from a specific user
        /// </summary>
        public static Expression Author(string username)
            => new("from:", username, ExpressionType.Author);

        /// <summary>
        /// Any tweet that is in reply to a specific user
        /// </summary>
        public static Expression Recipient(string username)
            => new("to:", username, ExpressionType.Recipient);

        /// <summary>
        /// Match a valid tweeter URL
        /// </summary>
        public static Expression Url(string twitterUrl)
            => new("url:", twitterUrl, ExpressionType.Url);

        /// <summary>
        /// Match tweet that are a retweet of a specific user
        /// </summary>
        public static Expression Retweet(string username)
            => new("retweets_of:", username, ExpressionType.Retweet);

        /// <summary>
        /// https://developer.twitter.com/en/docs/twitter-api/annotations
        /// </summary>
        public static Expression Context(string str)
            => new("context:", str, ExpressionType.Context);

        /// <summary>
        /// Match an entity (parameter is the string declaration of entity/place)
        /// </summary>
        public static Expression Entity(string str)
            => new("entity:", str, ExpressionType.Entity);

        /// <summary>
        /// Match tweet with a specific conversation ID
        /// A conversation ID is the ID of a tweet that started a conversation
        /// </summary>
        public static Expression ConversationId(string id)
            => new("conversation_id:", id, ExpressionType.ConversationId);

        /// <summary>
        /// Match a keyword within the tweet publisher's user bio name
        /// </summary>
        public static Expression Bio(string keyword)
            => new("bio:", keyword, ExpressionType.Bio);

        /// <summary>
        /// Match tweet that are published by users whose location contains a specific keyword
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static Expression BioLocation(string location)
            => new("bio_location:", location, ExpressionType.BioLocation);

        /// <summary>
        /// Tweets tagged with a specific location
        /// </summary>
        public static Expression Place(string location)
            => new("place:", location, ExpressionType.Place);

        /// <summary>
        /// Tweets tagged with a specific country
        /// </summary>
        /// <param name="country">Must be a valid ISO 3166-1 alpha-2 code</param>
        public static Expression PlaceCountry(string country)
            => new("place_country:", country, ExpressionType.PlaceCountry);

        /// <summary>
        /// Match against the geocoordinate of a tweet
        /// </summary>
        /// <param name="latitude">Must be in decimal degree, is in range of ±180</param>
        /// <param name="longitude">Must be in decimal degree, is in range of ±90</param>
        /// <param name="radius">Must be less than 25 miles</param>
        /// <param name="radiusUnit">km or mi</param>
        public static Expression PointRadius(string longitude, string latitude, string radius, RadiusUnit radiusUnit)
            => new($"point_radius:[{longitude} {latitude} {radius}{(radiusUnit == RadiusUnit.Kilometer ? "km" : "mi")}]", "", ExpressionType.PointRadius);

        /// <summary>
        /// Width and height of the box must be less than 25 miles
        /// </summary>
        /// <param name="westLongitude">Longitude of the southwest corner, is in range of ±180, decimal degree</param>
        /// <param name="southLatitude">Latitude of the southwest corner, is in range of ±90, decimal degree</param>
        /// <param name="eastLongitude">Longitude of the northeast corner, is in range of ±180, decimal degree</param>
        /// <param name="northLatitude">Latitude of the northeast corner, is in range of ±90, decimal degree</param>
        public static Expression BoundingBox(string westLongitude, string southLatitude, string eastLongitude, string northLatitude)
            => new($"bounding_box:[{westLongitude} {southLatitude} {eastLongitude} {northLatitude}]", "", ExpressionType.BoundingBox);

        /// <summary>
        /// Match retweets (doesn't include quote tweets)
        /// </summary>
        public static Expression IsRetweet()
            => new("is:retweet", "", ExpressionType.IsRetweet);

        /// <summary>
        /// Match replies
        /// </summary>
        public static Expression IsReply()
            => new("is:reply", "", ExpressionType.IsReply);

        /// <summary>
        /// Match quote tweets
        /// </summary>
        public static Expression IsQuote()
            => new("is:quote", "", ExpressionType.IsQuote);

        /// <summary>
        /// Only match tweets from verified accounts
        /// </summary>
        public static Expression IsVerified()
            => new("is:verified", "", ExpressionType.IsVerified);

        /// <summary>
        /// Remove tweets created for promotion only on ads.twitter.com
        /// Can't be negated
        /// </summary>
        public static Expression IsNotNullcast()
            => new("-is:nullcast", "", ExpressionType.IsNotNullcast);

        /// <summary>
        /// Only match tweets that contains at least one hashtag
        /// </summary>
        public static Expression HasHashtags()
            => new("has:hashtags", "", ExpressionType.HasHashtags);

        /// <summary>
        /// Only match tweets that contains at least one cashtag
        /// </summary>
        public static Expression HasCashtags()
            => new("has:cashtags", "", ExpressionType.HasCashtags);

        /// <summary>
        /// Only match tweets that contains at least one link/media in their body
        /// </summary>
        public static Expression HasLinks()
            => new("has:links", "", ExpressionType.HasLinks);

        /// <summary>
        /// Only match tweets that mention another user
        /// </summary>
        public static Expression HasMentions()
            => new("has:mentions", "", ExpressionType.HasMentions);

        /// <summary>
        /// Only match tweets that contains at least one media (photo/GIF/video)
        /// Doesn't match media created with Periscope or tweets that link to other media hosting sites
        /// </summary>
        public static Expression HasMedia()
            => new("has:media", "", ExpressionType.HasMedia);

        /// <summary>
        /// Only match tweets that contains an URL to an image
        /// </summary>
        public static Expression HasImages()
            => new("has:images", "", ExpressionType.HasImages);

        /// <summary>
        /// Only match tweets that contains a video uploaded to 
        /// Doesn't match media created with Periscope or tweets that link to other media hosting sites
        /// </summary>
        public static Expression HasVideos()
            => new("has:videos", "", ExpressionType.HasVideos);

        /// <summary>
        /// Only match tweet that contains geolocation data
        /// </summary>
        public static Expression HasGeo()
            => new("has:geo", "", ExpressionType.HasGeo);

        /// <summary>
        /// Only returns a percentage of tweet that match a rule
        /// </summary>
        public static Expression Sample(int percent)
            => new("sample:" + percent, "", ExpressionType.Sample);

        /// <summary>
        /// Match tweets that has been classified as being of a specific language
        /// A tweet can only be of one language
        /// </summary>
        /// <param name="countryCode">Must be a valid BCP 47 code</param>
        public static Expression Lang(string countryCode)
            => new("lang:", countryCode, ExpressionType.Lang);

        /// <summary>
        /// Matches Tweets when the author has a followers count within the given range.
        /// Example: followers_count:500
        /// </summary>
        /// <param name="count">Any number equal to or higher will match</param>
        public static Expression FollowersCount(int count)
            => new("followers_count:", count.ToString(), ExpressionType.FollowersCount);

        /// <summary>
        /// Matches Tweets when the author has a followers count within the given range.
        /// Example: followers_count:1000..10000
        /// </summary>
        public static Expression FollowersCount(int from, int to)
            => new("followers_count:", from + ".." + to, ExpressionType.FollowersCount);

        /// <summary>
        /// Matches Tweets when the author has posted a number of Tweets that falls within the given range.
        /// Example: tweets_count:500
        /// </summary>
        /// <param name="count">Any number equal to or higher will match</param>
        public static Expression TweetsCount(int count)
            => new("tweets_count:", count.ToString(), ExpressionType.TweetsCount);

        /// <summary>
        /// Matches Tweets when the author has posted a number of Tweets that falls within the given range.
        /// Example: tweets_count:1000..10000
        /// </summary>
        public static Expression TweetsCount(int from, int to)
            => new("tweets_count:", from + ".." + to, ExpressionType.TweetsCount);

        /// <summary>
        /// Matches Tweets when the author has a friends count (the number of users they follow) that falls within the given range.
        /// Example: following_count:500
        /// </summary>
        /// <param name="count">Any number equal to or higher will match</param>
        public static Expression FollowingCount(int count)
            => new("following_count:", count.ToString(), ExpressionType.FollowingCount);

        /// <summary>
        /// Matches Tweets when the author has a friends count (the number of users they follow) that falls within the given range.
        /// Example: following_count:1000..10000
        /// </summary>
        public static Expression FollowingCount(int from, int to)
            => new("following_count:", from + ".." + to, ExpressionType.FollowingCount);

        /// <summary>
        /// Matches Tweets when the author is included in the specified number of Lists. 
        /// Example: listed_count:500
        /// </summary>
        /// <param name="count">Any number equal to or higher will match</param>
        public static Expression ListedCount(int count)
            => new("listed_count:", count.ToString(), ExpressionType.ListedCount);
        
        /// <summary>
        /// Matches Tweets when the author is included in the specified number of Lists. 
        /// Example: listed_count:1000..10000
        /// </summary>
        public static Expression ListedCount(int from, int to)
            => new("listed_count:", from + ".." + to, ExpressionType.ListedCount);

        /// <summary>
        /// Performs a keyword/phrase match on the expanded URL HTML title metadata.
        /// Example: url_title:snow
        /// </summary>
        public static Expression UrlTitle(string title)
            => new("url_title:", title, ExpressionType.UrlTitle);

        /// <summary>
        /// Performs a keyword/phrase match on the expanded page description metadata.
        /// Example: url_description:weather
        /// </summary>
        public static Expression UrlDescription(string description)
            => new("url_description:", description, ExpressionType.UrlDescription);

        /// <summary>
        /// Matches Tweets with URLs that literally contain the given phrase or keyword. To search for patterns with punctuation in them (i.e. google.com) enclose the search term in quotes.
        /// NOTE: This will match against the expanded URL as well.
        /// Example: url_contains:photos
        /// </summary>
        public static Expression UrlContains(string contains)
            => new("url_contains:", contains, ExpressionType.UrlContains);

        /// <summary>
        /// Deliver only explicit Replies to the specified Tweet.
        /// Example: in_reply_to_tweet_id:1539382664746020864
        /// </summary>
        public static Expression InReplyToTweetId(long tweetId)
            => new("in_reply_to_tweet_id:", tweetId.ToString(), ExpressionType.InReplyToTweetId);

        /// <summary>
        /// Deliver only explicit (or native) Retweets of the specified Tweet. Note that the status ID used should be the ID of an original Tweet and not a Retweet.
        /// Example: retweets_of_tweet_id:1539382664746020864
        /// </summary>
        public static Expression RetweetsOfTweetId(long tweetId)
            => new("retweets_of_tweet_id:", tweetId.ToString(), ExpressionType.RetweetsOfTweetId);
    }
}
