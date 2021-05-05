using System.Linq;

namespace TwitterSharp.Rule
{
    // https://developer.twitter.com/en/docs/twitter-api/tweets/filtered-stream/integrate/build-a-rule
    public struct Expression
    {
        internal Expression(string prefix, string userInput)
        {
            _internal = prefix + (userInput.Contains(' ') ? "\"" + userInput + "\"" : userInput);
        }

        private readonly string _internal;

        public override string ToString()
            => _internal;

        // LOGIC

        /// <summary>
        /// Tweet match one of the expression given in parameter
        /// </summary>
        public Expression Or(params Expression[] others)
            => new(_internal + " OR " + string.Join(" OR ", others.Select(x => x.ToString())), "");

        /// <summary>
        /// Tweet match all the expressions given in parameter
        /// </summary>
        public Expression And(params Expression[] others)
            => new(_internal + " AND " + string.Join(" AND ", others.Select(x => x.ToString())), "");

        /// <summary>
        /// Tweet match the negation of the current expression
        /// </summary>
        public Expression Negate()
            => new("-" + _internal, "");

        // OPERATORS

        /// <summary>
        /// Match a keyword in the body of a tweet
        /// </summary>
        public static Expression Keyword(string str)
            => new("", str);

        /// <summary>
        /// Any tweet with the given hashtag
        /// </summary>
        public static Expression Hashtag(string entity)
            => new("#", entity);

        /// <summary>
        /// Any tweet with the given cashtag
        /// </summary>
        public static Expression Cashtag(string entity)
            => new("$", entity);

        /// <summary>
        /// Any tweet that contains a mention of the given user
        /// </summary>
        public static Expression Mention(string username)
            => new("@", username);

        /// <summary>
        /// Any tweet sent from a specific user
        /// </summary>
        public static Expression Author(string username)
            => new("from:", username);

        /// <summary>
        /// Any tweet that is in reply to a specific user
        /// </summary>
        public static Expression Recipient(string username)
            => new("to:", username);

        /// <summary>
        /// Match a valid tweeter URL
        /// </summary>
        public static Expression Url(string twitterUrl)
            => new("url:", twitterUrl);

        /// <summary>
        /// Match tweet that are a retweet of a specific user
        /// </summary>
        public static Expression Retweet(string username)
            => new("retweets_of:", username);

        /// <summary>
        /// https://developer.twitter.com/en/docs/twitter-api/annotations
        /// </summary>
        public static Expression Context(string str)
            => new("context:", str);

        /// <summary>
        /// Match an entity (parameter is the string declaration of entity/place)
        /// </summary>
        public static Expression Entity(string str)
            => new("entity: ", str);

        /// <summary>
        /// Match tweet with a specific conversation ID
        /// A conversation ID is the ID of a tweet that started a conversation
        /// </summary>
        public static Expression ConversationId(string id)
            => new("conversation_id:", id);

        /// <summary>
        /// Match a keyword within the tweet publisher's user bio name
        /// </summary>
        public static Expression Bio(string keyword)
            => new("bio:", keyword);

        /// <summary>
        /// Match tweet that are published by users whose location contains a specific keyword
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static Expression BioLocation(string location)
            => new("bio_location:", location);

        /// <summary>
        /// Tweets tagged with a specific location
        /// </summary>
        public static Expression Place(string location)
            => new("place:", location);

        /// <summary>
        /// Tweets tagged with a specific country
        /// </summary>
        /// <param name="country">Must be a valid ISO 3166-1 alpha-2 code</param>
        public static Expression PlaceCountry(string country)
            => new("place_country:", country);

        /// <summary>
        /// Match against the geocoordinate of a tweet
        /// </summary>
        /// <param name="latitude">Must be in decimal degree, is in range of ±180</param>
        /// <param name="longitude">Must be in decimal degree, is in range of ±90</param>
        /// <param name="radius">Must be less than 25 miles</param>
        public static Expression PointRadius(string longitude, string latitude, string radius, RadiusUnit radiusUnit)
            => new($"point_radius:[{longitude} {latitude} {radius}{(radiusUnit == RadiusUnit.Kilometer ? "km" : "mi")}]", "");

        /// <summary>
        /// Width and height of the box must be less than 25 miles
        /// </summary>
        /// <param name="westLongitude">Longitude of the southwest corner, is in range of ±180, decimal degree</param>
        /// <param name="southLatitude">Latitude of the southwest corner, is in range of ±90, decimal degree</param>
        /// <param name="eastLongitude">Longitude of the northeast corner, is in range of ±180, decimal degree</param>
        /// <param name="northLatitude">Latitude of the northeast corner, is in range of ±90, decimal degree</param>
        public static Expression BoundingBox(string westLongitude, string southLatitude, string eastLongitude, string northLatitude)
            => new($"bounding_box:[{westLongitude} {southLatitude} {eastLongitude} {northLatitude}]", "");

        /// <summary>
        /// Match retweets (doesn't include quote tweets)
        /// </summary>
        public static Expression IsRetweet()
            => new("is:retweet", "");

        /// <summary>
        /// Match replies
        /// </summary>
        public static Expression IsReply()
            => new("is:reply", "");

        /// <summary>
        /// Match quote tweets
        /// </summary>
        public static Expression IsQuote()
            => new("is:quote", "");

        /// <summary>
        /// Only match tweets from verified accounts
        /// </summary>
        public static Expression IsVerified()
            => new("is:verified", "");

        /// <summary>
        /// Remove tweets created for promotion only on ads.twitter.com
        /// Can't be negated
        /// </summary>
        public static Expression IsNotNullcast()
            => new("-is:nullcast", "");

        /// <summary>
        /// Only match tweets that contains at least one hashtag
        /// </summary>
        public static Expression HasHashtags()
            => new("has:hashtags", "");

        /// <summary>
        /// Only match tweets that contains at least one cashtag
        /// </summary>
        public static Expression HasCashtags()
            => new("has:cashtags", "");

        /// <summary>
        /// Only match tweets that contains at least one link/media in their body
        /// </summary>
        public static Expression HasLinks()
            => new("has:links", "");

        /// <summary>
        /// Only match tweets that mention another user
        /// </summary>
        public static Expression HasMentions()
            => new("has:mentions", "");

        /// <summary>
        /// Only match tweets that contains at least one media (photo/GIF/video)
        /// Doesn't match media created with Periscope or tweets that link to other media hosting sites
        /// </summary>
        public static Expression HasMedia()
            => new("has:media", "");

        /// <summary>
        /// Only match tweets that contains an URL to an image
        /// </summary>
        public static Expression HasImages()
            => new("has:images", "");

        /// <summary>
        /// Only match tweets that contains a video uploaded to 
        /// Doesn't match media created with Periscope or tweets that link to other media hosting sites
        /// </summary>
        public static Expression HasVideos()
            => new("has:videos", "");

        /// <summary>
        /// Only match tweet that contains geolocation data
        /// </summary>
        public static Expression HasGeo()
            => new("has:geo", "");

        /// <summary>
        /// Only returns a percentage of tweet that match a rule
        /// </summary>
        public static Expression Sample(int percent)
            => new("sample:" + percent, "");

        /// <summary>
        /// Match tweets that has been classified as being of a specific language
        /// A tweet can only be of one language
        /// </summary>
        /// <param name="countryCode">Must be a valid BCP 47 code</param>
        public static Expression Lang(string countryCode)
            => new("lang:", countryCode);
    }
}
