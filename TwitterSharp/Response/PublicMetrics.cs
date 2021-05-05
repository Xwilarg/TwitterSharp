using System.Text.Json.Serialization;

namespace TwitterSharp.Response
{
    public class PublicMetrics
    {
        [JsonPropertyName("followers_count")]
        public int FollowersCount { internal init; get; }

        [JsonPropertyName("following_count")]
        public int FollowingCount { internal init; get; }

        [JsonPropertyName("tweet_count")]
        public int TweetCount { internal init; get; }

        [JsonPropertyName("listed_count")]
        public int ListedCount { internal init; get; }
    }
}
