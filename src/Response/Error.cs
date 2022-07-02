namespace TwitterSharp.Response
{
    public class Error
    {
        public string Parameter { init; get; }
        public string[] Details { init; get; }
        public string Code { init; get; }
        public string Value { init; get; }
        public string Message { init; get; }
        public string Title { init; get; }
        public string Type { init; get; }
    }
}