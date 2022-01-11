using System.Text.Json.Serialization;

namespace TwitterSharp.Response.Entity
{
    public class EntityUrl : AEntity
    {
        public string Url { init; get; }
        [JsonPropertyName("expanded_url")]
        public string ExpandedUrl { init; get; }
        [JsonPropertyName("display_url")]
        public string DisplayUrl { init; get; }
    }
}
