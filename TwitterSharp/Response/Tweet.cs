using System;

namespace TwitterSharp.Response
{
    public struct Tweet
    {
        public string Id { init; get; }
        public string Text { init;  get; }

        public override bool Equals(object obj)
            => obj is Tweet t && t.Id == Id;

        public override int GetHashCode()
            => Id.GetHashCode();
    }
}
