using System;
using System.Threading.Tasks;

namespace TwitterSharp.Response.RTweet
{
    public class RTweets
    {
        public Tweet[] Tweets { init; get; }

        public Func<Task<RTweets>> NextAsync { init; get; }
    }
}
