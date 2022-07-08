using TwitterSharp.Response.RMedia;
using TwitterSharp.Response.RTweet;
using TwitterSharp.Response.RUser;

namespace TwitterSharp.Response
{
    internal class Answer<T> : BaseAnswer
    {
        public T Data { set; get; }
    }

    internal class BaseAnswer
    {
        public Meta Meta { init; get; }
        public Includes Includes { init; get; }
        public MatchingRule[] MatchingRules { init; get; }

        // Error Handling
        public string Detail { init; get; }
        public string Title { init; get; }
        public string Type { init; get; }
        public Error[] Errors { init; get; }
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
        public int Created { init; get; }
        public int NotCreated { init; get; }
        public int Valid { init; get; }
        public int Invalid { init; get; }
        public int Deleted { init; get; }
    }
}
