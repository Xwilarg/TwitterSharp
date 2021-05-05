using TwitterSharp.Response;

namespace TwitterSharp.Client
{
    internal struct Answer<T, U>
    {
        public T Data { init; get; }
        public U Includes { init; get; }
        public Meta Meta { init; get; }

        // Error Handling
        public string Detail { init; get; }
    }

    internal struct Answer<T>
    {
        public T Data { init; get; }
        public Meta Meta { init; get; }

        // Error Handling
        public string Detail { init; get; }
    }

    internal struct Meta
    {
        public Summary Summary { init; get; }
    }

    internal struct UserContainer
    {
        public User[] Users;
    }

    internal struct Summary
    {
        public int Deleted { init; get; }
    }
}
