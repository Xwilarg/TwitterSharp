using System;
using TwitterSharp.Response;

namespace TwitterSharp.Client
{
    public class TwitterException : Exception
    {
        internal TwitterException(string message, Error[] errors = null) : base(message)
        {
            Errors = errors;
        }

        public Error[] Errors { init; get; }
    }
}
