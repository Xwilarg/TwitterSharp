namespace TwitterSharp.Response
{
    public class Error
    {
        public string Parameter { get; internal set; }
        public string[] Details { get; internal set; }
        public string Code { get; internal set; }
        public string Value { get; internal set; }
        public string Message { get; internal set; }
        public string Title { get; internal set; }
        public string Type { get; internal set; }
    }
}
