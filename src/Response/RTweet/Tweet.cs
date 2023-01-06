using System;
using System.Text.Json.Serialization;
using TwitterSharp.Client;
using TwitterSharp.Model;
using TwitterSharp.Response.Entity;
using TwitterSharp.Response.RMedia;
using TwitterSharp.Response.RUser;

namespace TwitterSharp.Response.RTweet
{
    public class Tweet : IEquatable<Tweet>, IHaveAuthor, IHaveMedia, IHaveMatchingRules
    {
        /// <summary>
        /// Unique identifier of the tweet
        /// </summary>
        public string Id { init; get; }
        /// <summary>
        /// Text of the tweet
        /// </summary>
        public string Text { init; get; }
        /// <summary>
        /// ID of the user who created the tweet
        /// </summary>
        public string AuthorId { init; get; }
        /// <summary>
        /// The ID of the tweet that the conversation is from
        /// </summary>
        public string ConversationId { init; get; }
        /// <summary>
        /// When the tweet was created
        /// </summary>
        public DateTime? CreatedAt { init; get; }
        /// <summary>
        /// Information about special entities in the tweet
        /// </summary>
        public Entities Entities { init; get; }
        /// <summary>
        /// ID of the user to whom the tweet is replying
        /// </summary>
        public string InReplyToUserId { init; get; }
        /// <summary>
        /// Language of the tweet (detected by Twitter)
        /// </summary>
        public string Lang { init; get; }
        /// <summary>
        /// If the tweet may contain sensitive information
        /// </summary>
        public bool? PossiblySensitive { init; get; }
        /// <summary>
        /// Public metrics of the tweet
        /// </summary>
        public TweetPublicMetrics PublicMetrics { init; get; }
        /// <summary>
        /// Tweet that were referenced by this one
        /// </summary>
        public ReferencedTweet[] ReferencedTweets { init; get; }
        /// <summary>
        /// Who can reply to this tweet
        /// </summary>
        public ReplySettings? ReplySettings { init; get; }
        /// <summary>
        /// Objects attached to the tweet
        /// </summary>
        public Attachments Attachments { set; get; }

        [JsonIgnore]
        public User Author { internal set; get; }

        // Interface
        void IHaveAuthor.SetAuthor(User author)
            => Author = author;
        [JsonIgnore]
        string IHaveAuthor.AuthorId => AuthorId;

        Media[] IHaveMedia.GetMedia()
            => Attachments?.Media;

        [JsonIgnore]
        public MatchingRule[] MatchingRules { set; get; }

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
