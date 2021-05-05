using System;

namespace TwitterSharp.Response
{
    internal struct TweetInternal
    {
        public string Id { internal init; get; }
        public string Text { internal init; get; }
    }

    public struct Tweet : IEquatable<Tweet>
    {
        public string Id { internal init; get; }
        public string Text { internal init;  get; }

        public User Author { internal init; get; }

        public override bool Equals(object obj)
            => obj is Tweet t && t.Id == Id;

        public bool Equals(Tweet other)
            => other.Id == Id;

        public override int GetHashCode()
            => Id.GetHashCode();

        public static bool operator ==(Tweet left, Tweet right)
            => left.Id == right.Id;

        public static bool operator !=(Tweet left, Tweet right)
            => left.Id != right.Id;
    }
}
