using System.Text.Json.Serialization;

namespace TwitterSharp.Response.RMedia
{
    // MUST BE PARSED IN MEDIACONVERTER
    public class Media
    {
        public string Key { set; get; }
        public MediaType? Type { set; get; }
    }
}
