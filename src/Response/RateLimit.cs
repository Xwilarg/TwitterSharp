namespace TwitterSharp.Response
{
    public class RateLimit
    {
        public string Endpoint { get; set; }
        public int Limit { get; internal set; }
        public int Remaining { get; internal set; }
        public int Reset { get; internal set; }

        public RateLimit(string endpoint)
        {
            Endpoint = endpoint;
        }
    }
}
