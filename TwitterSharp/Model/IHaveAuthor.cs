using TwitterSharp.Response;

namespace TwitterSharp.Model
{
    interface IHaveAuthor
    {
        internal void SetAuthor(User author);
        internal string AuthorId { get; }
    }
}
