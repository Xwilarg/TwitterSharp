using System;
using System.Text.Json.Serialization;

namespace TwitterSharp.Response
{
    public struct User : IEquatable<User>
    {
        public string Id { internal init; get; }
        public string Name { internal init; get; }
        public string Username { internal init; get; }

        // OPTIONAL

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { internal init; get; }
        public string Description { internal init; get; }
        public string Location { internal init; get; }
        [JsonPropertyName("pinned_tweet_id")]
        public string PinnedTweetId { internal init; get; }
        [JsonPropertyName("profile_image_url")]
        public string ProfileImageUrl { internal init; get; }
        [JsonPropertyName("protected")]
        public bool? IsProtected { internal init; get; }
        public string Url { internal init; get; }
        [JsonPropertyName("public_metrics")]
        public PublicMetrics PublicMetrics { internal init; get; }
        [JsonPropertyName("verified")]
        public bool? IsVerified { internal init; get; }

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
