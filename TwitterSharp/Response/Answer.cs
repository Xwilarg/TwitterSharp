using TwitterSharp.Response.RMedia;
using TwitterSharp.Response.RUser;

namespace TwitterSharp.Client
{
    internal class Answer<T>
    {
        public T Data { set; get; }
        public Meta Meta { init; get; }
        public Includes Includes { init; get; }

        // Error Handling
        public string Detail { init; get; }
    }

    internal class Includes
    {
        public User[] Users { init; get; }
        public Media[] Media { init; get; }
    }

    internal class Meta
    {
        public Summary Summary { init; get; }
        public string NextToken { init; get; }
    }

    internal class Summary
    {
        public int Deleted { init; get; }
    }
}
