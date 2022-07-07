using System;
using TwitterSharp.Response;

namespace TwitterSharp.Client
{
    public class TwitterException : Exception
    {
        internal TwitterException(string message, string title = default, string type = default, Error[] errors = null) : base(message)
        {
            Title = title;
            Type = type;
            Errors = errors;
        }

        public string Title { init; get; }
        public string Type { init; get; }
        public Error[] Errors { init; get; }
    }
}
