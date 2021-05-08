using System.Text.Json.Serialization;

namespace TwitterSharp.Response.RMedia
{
    public class Media
    {
        public string Key { init; get; }
        public MediaType? Type { init; get; }
    }
}
