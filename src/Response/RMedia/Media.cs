namespace TwitterSharp.Response.RMedia
{
    // MUST BE PARSED IN MEDIACONVERTER
    public class Media
    {
        public string Key { init; get; }
        public MediaType? Type { init; get; }
        public int? Height { init; get; }
        public int? Width { init; get; }
        public int? DurationMs { init; get; }
        public string PreviewImageUrl { init; get; }
        public string Url { init; get; }
    }
}
