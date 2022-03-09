using TwitterSharp.Request.AdvancedSearch;

namespace TwitterSharp.Request.Option
{
    public class UserSearchOptions : ASearchOptions
    {
        /// <summary>
        /// Additional information to get about the users
        /// </summary>
        public UserOption[] UserOptions { set; get; }

        protected override void PreBuild(bool needExpansion)
        {
            AddUserOptions(UserOptions, needExpansion);
        }
    }
}
