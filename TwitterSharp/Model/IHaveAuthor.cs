using TwitterSharp.Response;

namespace TwitterSharp.Model
{
    interface IHaveAuthor
    {
        internal User Author { set; }
        internal string AuthorId { get; }
    }
}
