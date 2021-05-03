using System;

namespace TwitterSharp.Response
{
    public struct StreamInfo : IEquatable<StreamInfo>
    {
        public string Id { init; get; }
        public string Value { init; get; }

        public override bool Equals(object obj)
            => obj is StreamInfo t && t.Id == Id;

        public bool Equals(StreamInfo other)
            => other.Id == Id;

        public override int GetHashCode()
            => Id.GetHashCode();

        public static bool operator ==(StreamInfo left, StreamInfo right)
            => left.Id == right.Id;

        public static bool operator !=(StreamInfo left, StreamInfo right)
            => left.Id != right.Id;
    }
}
