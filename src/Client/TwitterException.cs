using System;

namespace TwitterSharp.Client
{
    public class TwitterException : Exception
    {
        public TwitterException(string message) : base(message)
        { }
    }
}
