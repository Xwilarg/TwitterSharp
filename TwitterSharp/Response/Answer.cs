using TwitterSharp.Response;

namespace TwitterSharp.Client
{
    internal class Answer<T>
    {
        public T Data { init; get; }
        public Meta Meta { init; get; }

        // Error Handling
        public string Detail { init; get; }
    }

    internal class Meta
    {
        public Summary Summary { init; get; }
    }

    internal class Summary
    {
        public int Deleted { init; get; }
    }
}
