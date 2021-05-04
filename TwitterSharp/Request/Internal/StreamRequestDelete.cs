using System.Text.Json.Serialization;

namespace TwitterSharp.Request.Internal
{
    internal struct StreamRequestDelete
    {
        [JsonPropertyName("delete")]
        public StreamRequestDeleteIds Delete { init; get; }
    }

    internal struct StreamRequestDeleteIds
    {
        [JsonPropertyName("ids")]
        public string[] Ids { init; get; }
    }
}
