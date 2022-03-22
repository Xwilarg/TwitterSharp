using System;
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

        /// <summary>
        /// Only returns tweet that were sent before the referenced id
        /// </summary>
        public string SinceId { set; get; }

        /// <summary>
        /// Only returns tweet that were sent after the given date
        /// </summary>
        public DateTime? StartTime { set; get; }

        protected override void PreBuild(bool needExpansion)
        {
            AddUserOptions(UserOptions, needExpansion);
            AddTweetOptions(TweetOptions);
            AddMediaOptions(MediaOptions);
            if (SinceId != null)
            {
                _options.Add("since_id", new() { SinceId });
            }
            if (StartTime.HasValue)
            {
                _options.Add("start_time", new() { StartTime.Value.ToString("yyyy-MM-ddTHH:mm:ssZ") });
            }
        }
    }
}
