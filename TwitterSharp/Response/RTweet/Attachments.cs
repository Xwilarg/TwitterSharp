using System.Text.Json.Serialization;
using TwitterSharp.Response.RMedia;

namespace TwitterSharp.Response.RTweet
{
    public class Attachments
    {
        //[JsonPropertyName("poll_ids")]
        //public Media[] Polls { init; get; }
        [JsonPropertyName("media_keys")]
        public Media[] Media { set; get; }
    }
}
