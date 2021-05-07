using System;

namespace TwitterSharp.Response
{
    public class User : IEquatable<User>
    {
        public string Id { init; get; }
        public string Name { init; get; }
        public string Username { init; get; }

        // OPTIONAL

        public DateTime? CreatedAt { init; get; }
        public string Description { init; get; }
        public string Location { init; get; }
        public string PinnedTweetId { init; get; }
        public string ProfileImageUrl { init; get; }
        public bool? IsProtected { init; get; }
        public string Url { init; get; }
        public Entities Entities { init; get; }
        public UserPublicMetrics PublicMetrics { init; get; }
        public bool? IsVerified { init; get; }

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
