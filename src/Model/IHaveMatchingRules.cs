using TwitterSharp.Response.RTweet;

namespace TwitterSharp.Model
{
    interface IHaveMatchingRules
    {
        public MatchingRule[] MatchingRules { set; get; }
    }
}
