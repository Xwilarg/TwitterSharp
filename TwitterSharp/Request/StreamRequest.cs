using System.Text.Json.Serialization;
using TwitterSharp.Rule;

namespace TwitterSharp.Request
{
    public struct StreamRequest
    {
        public StreamRequest(Expression value, string tag = "")
        {
            Value = value;
            Tag = tag;
        }

        [JsonPropertyName("value")]
        public Expression Value { private init; get; }
        [JsonPropertyName("tag")]
        public string Tag { private init; get; }
    }
}
