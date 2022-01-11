using TwitterSharp.Response.RMedia;

namespace TwitterSharp.Model
{
    interface IHaveMedia
    {
        internal Media[] GetMedia();
    }
}
