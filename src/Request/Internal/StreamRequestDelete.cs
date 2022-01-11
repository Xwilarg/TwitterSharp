using System.Text.Json.Serialization;

namespace TwitterSharp.Request.Internal
{
    internal class StreamRequestDelete
    {
        [JsonPropertyName("delete")]
        public StreamRequestDeleteIds Delete { init; get; }
    }

    internal class StreamRequestDeleteIds
    {
        [JsonPropertyName("ids")]
        public string[] Ids { init; get; }
    }
}
