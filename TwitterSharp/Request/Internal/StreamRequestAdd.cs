using System.Text.Json.Serialization;

namespace TwitterSharp.Request.Internal
{
    internal struct StreamRequestAdd
    {
        [JsonPropertyName("add")]
        public StreamRequest[] Add { init; get; }
    }
}
