using System.Text.Json.Serialization;

namespace TwitterSharp.Response
{
    public class PublicMetrics
    {
        [JsonPropertyName("followers_count")]
        public int FollowersCount { init; get; }

        [JsonPropertyName("following_count")]
        public int FollowingCount { init; get; }

        [JsonPropertyName("tweet_count")]
        public int TweetCount { init; get; }

        [JsonPropertyName("listed_count")]
        public int ListedCount { init; get; }
    }
}
