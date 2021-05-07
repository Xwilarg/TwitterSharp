using System;
using System.Text.Json.Serialization;
using TwitterSharp.Model;

namespace TwitterSharp.Response
{
    public class Tweet : IEquatable<Tweet>, IHaveAuthor
    {
        public string Id { init; get; }
        public string Text { init; get; }
        public string AuthorId { init; get; }
        public string ConversationId { init; get; }
        public DateTime? CreatedAt { init; get; }
        public Entities Entities { init; get; }
        public string InReplyToUserId { init; get; }
        public string Lang { init; get; }
        public bool? PossiblySensitive { init; get; }
        public PublicMetrics PublicMetrics { init; get; }
        public string Source { init; get; }
        public ReferencedTweet[] ReferencedTweets { init; get; }
        public ReplySettings ReplySettings { init; get; }

        [JsonIgnore]
        public User Author { internal set; get; }

        // Interface
        void IHaveAuthor.SetAuthor(User author)
            => Author = author;
        [JsonIgnore]
        string IHaveAuthor.AuthorId => AuthorId;

        // Comparison

        public override bool Equals(object obj)
            => obj is Tweet t && t?.Id == Id;

        public bool Equals(Tweet other)
            => other?.Id == Id;

        public override int GetHashCode()
            => Id.GetHashCode();

        public static bool operator ==(Tweet left, Tweet right)
            => left?.Id == right?.Id;

        public static bool operator !=(Tweet left, Tweet right)
            => left?.Id != right?.Id;
    }
}
