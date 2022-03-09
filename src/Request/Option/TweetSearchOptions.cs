using TwitterSharp.Request.AdvancedSearch;

namespace TwitterSharp.Request.Option
{
    public class TweetSearchOptions
    {
        /// <summary>
        /// Additional information to get about the users
        /// </summary>
        public UserOption[] UserOptions = null;

        /// <summary>
        /// Additional information to get about the tweets
        /// </summary>
        public TweetOption[] TweetOptions = null;

        /// <summary>
        /// Additional information to get about the medias attached
        /// </summary>
        public MediaOption[] MediaOptions = null;
    }
}
