using System;
using TwitterSharp.Rule;

namespace TwitterSharp.Response
{
    public class StreamInfo : IEquatable<StreamInfo>
    {
        public string Id { init; get; }

        // TODO: I want to better handle this but there is too much parsing: https://developer.twitter.com/en/docs/twitter-api/tweets/search/integrate/build-a-query
        public Expression Value { init; get; }
        public string Tag { init; get; }

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
