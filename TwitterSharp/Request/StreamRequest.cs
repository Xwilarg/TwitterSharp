using System.Text.Json.Serialization;

namespace TwitterSharp.Request
{
    public struct StreamRequest
    {
        /// <param name="value"></param>
        /// <param name="tag"></param>
        public StreamRequest(string value, string tag = "")
        {
            Value = value;
            Tag = tag;
        }

        [JsonPropertyName("value")]
        public string Value { init; get; }
        [JsonPropertyName("tag")]
        public string Tag { init; get; }
    }
}
