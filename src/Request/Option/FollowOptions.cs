using TwitterSharp.Request.AdvancedSearch;

namespace TwitterSharp.Request.Option
{
    public class FollowOptions
    {
        /// <summary>
        /// Additional information to get about the users
        /// </summary>
        public UserOption[] UserOptions = null;

        /// <summary>
        /// Max number of results returned by the API
        /// </summary>
        public int Limit = 100;
    }
}
