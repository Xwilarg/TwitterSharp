using TwitterSharp.Client;

namespace TwitterSharp.Model
{
    interface IHaveMatchingRules
    {
        public MatchingRule[] MatchingRules { set; get; }
    }
}
