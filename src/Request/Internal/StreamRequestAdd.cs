using System.Text.Json.Serialization;

namespace TwitterSharp.Request.Internal
{
    internal class StreamRequestAdd
    {
        [JsonPropertyName("add")]
        public StreamRequest[] Add { init; get; }
    }
}
