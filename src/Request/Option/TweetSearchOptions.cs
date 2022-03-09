using TwitterSharp.Request.AdvancedSearch;

namespace TwitterSharp.Request.Option
{
    public class TweetSearchOptions : ASearchOptions
    {
        /// <summary>
        /// Additional information to get about the users
        /// </summary>
        public UserOption[] UserOptions { set; get; }

        /// <summary>
        /// Additional information to get about the tweets
        /// </summary>
        public TweetOption[] TweetOptions { set; get; }

        /// <summary>
        /// Additional information to get about the medias attached
        /// </summary>
        public MediaOption[] MediaOptions { set; get; }

        protected override void PreBuild(bool needExpansion)
        {
            AddUserOptions(UserOptions, needExpansion);
            AddTweetOptions(TweetOptions);
            AddMediaOptions(MediaOptions);
        }
    }
}
