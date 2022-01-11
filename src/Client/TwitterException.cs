using System;

namespace TwitterSharp.Client
{
    public class TwitterException : Exception
    {
        internal TwitterException(string message) : base(message)
        { }
    }
}
