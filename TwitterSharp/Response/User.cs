using System;

namespace TwitterSharp.Response
{
    public struct User : IEquatable<User>
    {
        public string Id { init; get; }
        public string Name { init; get; }
        public string Username { init; get; }

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
