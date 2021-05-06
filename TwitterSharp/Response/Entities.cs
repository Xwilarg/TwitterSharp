using TwitterSharp.Response.Entity;

namespace TwitterSharp.Response
{
    public class Entities
    {
        public EntityUrl[] Urls { set; get; }
        public EntityTag[] Hashtags { set; get; }
        public EntityTag[] Cashtags { set; get; }
        public EntityTag[] Mentions { set; get; }
    }
}
