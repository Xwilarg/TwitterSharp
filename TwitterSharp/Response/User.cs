using System;
using System.Text.Json.Serialization;

namespace TwitterSharp.Response
{
    public struct User : IEquatable<User>
    {
        public string Id { init; get; }
        public string Name { init; get; }
        public string Username { init; get; }

        // OPTIONAL

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { init; get; }
        public string Description { init; get; }
        public string Location { init; get; }
        [JsonPropertyName("pinned_tweet_id")]
        public string PinnedTweetId { init; get; }
        [JsonPropertyName("profile_image_url")]
        public string ProfileImageUrl { init; get; }
        [JsonPropertyName("protected")]
        public bool? IsProtected { init; get; }
        public string Url { init; get; }
        [JsonPropertyName("public_metrics")]
        public PublicMetrics PublicMetrics { init; get; }
        [JsonPropertyName("verified")]
        public bool? IsVerified { init; get; }

        public override bool Equals(object obj)
            => obj is User t && t.Id == Id;

        public bool Equals(User other)
            => other.Id == Id;

        public override int GetHashCode()
            => Id.GetHashCode();

        public static bool operator ==(User left, User right)
            => left.Id == right.Id;

        public static bool operator !=(User left, User right)
            => left.Id != right.Id;
    }
}
