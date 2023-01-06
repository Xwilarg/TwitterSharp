using System;
using TwitterSharp.Response.Entity;

namespace TwitterSharp.Response.RUser
{
    public class User : IEquatable<User>
    {
        /// <summary>
        /// Unique identifier of the user
        /// </summary>
        public string Id { init; get; }
        /// <summary>
        /// Name shown on the user profile
        /// </summary>
        public string Name { init; get; }
        /// <summary>
        /// Twitter handle
        /// </summary>
        public string Username { init; get; }

        // OPTIONAL

        /// <summary>
        /// Creation date of the account
        /// </summary>
        public DateTime? CreatedAt { init; get; }
        /// <summary>
        /// Profile description
        /// </summary>
        public string Description { init; get; }
        /// <summary>
        /// Location specified on user profile
        /// </summary>
        public string Location { init; get; }
        /// <summary>
        /// Identifier of the pinned tweet
        /// </summary>
        public string PinnedTweetId { init; get; }
        /// <summary>
        /// URL to the picture profile
        /// </summary>
        public string ProfileImageUrl { init; get; }
        /// <summary>
        /// Are the user tweet private
        /// </summary>
        public bool? Protected { init; get; }
        /// <summary>
        /// URL specified on user profile
        /// </summary>
        public string Url { init; get; }
        /// <summary>
        /// Information about special entities in user description
        /// </summary>
        public Entities Entities { init; get; }
        /// <summary>
        /// Public metrics of the user
        /// </summary>
        public UserPublicMetrics PublicMetrics { init; get; }
        /// <summary>
        /// If the user is a verified Twitter user
        /// </summary>
        public bool? Verified { init; get; }

        /// <summary>
        /// indicates the type of verification a user account has: blue, business, government or none
        /// </summary>
        public string VerifiedType { init; get; }

        public override bool Equals(object obj)
            => obj is User t && t?.Id == Id;

        public bool Equals(User other)
            => other?.Id == Id;

        public override int GetHashCode()
            => Id.GetHashCode();

        public static bool operator ==(User left, User right)
            => left?.Id == right?.Id;

        public static bool operator !=(User left, User right)
            => left?.Id != right?.Id;
    }
}
