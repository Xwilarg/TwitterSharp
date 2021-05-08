using System.Text.Json.Serialization;

namespace TwitterSharp.Response.RMedia
{
    // MUST BE PARSED IN MEDIACONVERTER
    public class Media
    {
        public string Key { set; get; }
        public MediaType? Type { set; get; }
        public int? Height { init; get; }
        public int? Width { init; get; }
        public int? DurationMs { init; get; }
        public string PreviewImageUrl { init; get; }
    }
}
