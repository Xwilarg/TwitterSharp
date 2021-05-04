namespace TwitterSharp.Client
{
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

    internal struct Summary
    {
        public int Deleted { init; get; }
    }
}
