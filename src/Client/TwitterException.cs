using System;
using TwitterSharp.Response;

namespace TwitterSharp.Client
{
    public class TwitterException : Exception
    {
        internal TwitterException(string message, string title = default, string type = default, Error[] errors = null) : base(message == null && errors != null ? "Error. See Errors property." : message)
        {
            Title = title;
            Type = type ?? "Error";
            Errors = errors;
        }

        internal TwitterException(BaseAnswer answer) : this(answer.Detail, answer.Title, answer.Type, answer.Errors) {}

        public string Title { init; get; }
        public string Type { init; get; }
        public Error[] Errors { init; get; }
    }
}
