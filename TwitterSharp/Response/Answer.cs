using TwitterSharp.Response;

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
        public User[] Users;
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
