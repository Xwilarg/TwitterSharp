using System.Text.Json.Serialization;

namespace TwitterSharp.Response.RTweet
{
    public class Attachments
    {
        [JsonPropertyName("poll_ids")]
        public AttachmentElement<object>[] Polls { init; get; }
        [JsonPropertyName("media_keys")]
        public AttachmentElement<object>[] Medias { init; get; }
    }
}
