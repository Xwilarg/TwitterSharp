using System.Text.Json.Serialization;

namespace TwitterSharp.Response.RMedia
{
    public class Media
    {
        public string Key { set; get; }
        public MediaType? Type { set; get; }
    }
}
