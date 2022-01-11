using System.Text.Json.Serialization;
using TwitterSharp.Rule;

namespace TwitterSharp.Request
{
    public class StreamRequest
    {
        public StreamRequest(Expression value, string tag)
        {
            Value = value;
            Tag = tag;
        }

        public StreamRequest(Expression value)
        {
            Value = value;
            Tag = "";
        }

        [JsonPropertyName("value")]
        public Expression Value { private init; get; }
        [JsonPropertyName("tag")]
        public string Tag { private init; get; }
    }
}
