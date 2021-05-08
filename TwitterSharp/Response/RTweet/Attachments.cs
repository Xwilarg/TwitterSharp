using System.Text.Json.Serialization;
using TwitterSharp.Response.RMedia;

namespace TwitterSharp.Response.RTweet
{
    public class Attachments
    {
        //[JsonPropertyName("poll_ids")]
        //public Media[] Polls { init; get; }
        public Media[] Medias { set; get; }
    }
}
