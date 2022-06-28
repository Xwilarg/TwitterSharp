using System.ComponentModel;

namespace TwitterSharp.ApiEndpoint
{
    /// <summary>
    /// All endpoints enhanced with rate limitsall and information from <see href="https://developer.twitter.com/en/portal/products/"/>,
    /// <see href="https://developer.twitter.com/en/docs/twitter-api/rate-limits"/> and <see href="https://developer.twitter.com/en/docs/api-reference-index"/>
    /// There is also a Tweet Cap per month depending the Acceess Level:
    /// Essential: 500K Tweets per month / Project
    /// Elevated: 2M Tweets per month / Project
    /// Academic: 10M Tweets per month / Project
    /// If you have Academic Research access, you can connect up to two redundant connections to maximize your streaming up-time.
    /// </summary>
    public enum Endpoint
    {
        #region Tweets

        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/tweets", Group = "Tweet lookup", LimitPerApp = 300, LimitPerUser = 900)]
        [Description("Retrieve multiple Tweets with a list of IDs")]
        GetTweetsByIds,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/tweets/:id", Group = "Tweet lookup", LimitPerApp = 300, LimitPerUser = 900)]
        [Description("Retrieve a single Tweet with an ID")]
        GetTweetById,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.POST, Url = "/2/tweets", Group = "Manage Tweets", LimitPerUser = 200, MaxPerUser = 300, MaxResetIntervalHours = 3, AdditionalInfo = "max shared with CreateRetweet")]
        [Description("Post a Tweet")]
        CreateTweet,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.DELETE, Url = "/2/tweets/:id", Group = "Manage Tweets", LimitPerUser = 50)]
        [Description("Delete a Tweet")]
        DeleteTweet,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/users/:id/timelines/reverse_chronological", Group = "Timelines", LimitPerUser = 180)]
        [Description("Allows you to retrieve a collection of the most recent Tweets and Retweets posted by you and users you follow")]
        ReverseChronologicalTimeline,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/users/:id/tweets", Group = "Timelines", LimitPerApp = 1500, LimitPerUser = 900, TweetCap = true)]
        [Description("Returns most recent Tweets composed a specified user ID")]
        UserTweetTimeline,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/users/:id/mentions", Group = "Timelines", LimitPerApp = 450, LimitPerUser = 180, TweetCap = true)]
        [Description("Returns most recent Tweets mentioning a specified user ID")]
        UserMentionTimeline,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/tweets/search/recent", Group = "Search Tweets", LimitPerApp = 450, LimitPerUser = 180, TweetCap = true, AdditionalInfo = "Additional Limits for all access levels: - 10 default results per response - 100 results per response. Essential + Elevated: - core operators - 512 query length. Academic: - enhanced operators - 1024 query length")]
        [Description("Search for Tweets published in the last 7 days")]
        RecentSearch,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/tweets/search/all", Group = "Search Tweets", LimitPerApp = 300, TweetCap = true, AccessLevel = AccessLevel.AcademicResearch, AdditionalInfo = "Full-archive also has a 1 request / 1 second limit. Additional Limits: - 500 results per response - 10 default results per response - enhanced operators - 1024 query length")]
        [Description("Search the full archive of Tweets")]
        FullArchiveSearch,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/tweets/counts/recent", Group = "Tweet counts", LimitPerApp = 300, AdditionalInfo = "Essential + Elevated: - core operators - 512 query length. Academic: - enhanced operators - 1024 query length")]
        [Description("Receive a count of Tweets that match a query in the last 7 days")]
        RecentTweetCounts,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/tweets/counts/all", Group = "Tweet counts", LimitPerApp = 300, AccessLevel = AccessLevel.AcademicResearch, AdditionalInfo = "- enhanced operators, - 1024 query length")]
        [Description("Receive a count of Tweets that match a query")]
        FullArchiveTweetCounts,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/tweets/search/stream", Group = "Filtered stream", LimitPerApp = 50, TweetCap = true, AdditionalInfo = "Essential + Elevated: - core operators - 512 rule length. Essential - 5 rules. Elevated: - 25 rules. Academic: - 1000 rules - enhanced operators - 1024 query length")]
        [Description("Connect to the stream")]
        ConnectingFilteresStream,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.POST, Url = "/2/tweets/search/stream/rules", Group = "Filtered stream", LimitPerApp = 25, AdditionalInfo = "Essential + Elevated: - does not support backfill - 50 Tweets per second - 1 connections. Academic: - supports backfill - 2 connections - 250 Tweets per second")]
        [Description("Add or delete rules from your stream")]
        AddingDeletingFilters,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/tweets/search/stream/rules", Group = "Filtered stream", LimitPerApp = 450)]
        [Description("Retrieve your stream's rules")]
        ListingFilters,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/tweets/sample/stream", Group = "Sampled stream", LimitPerApp = 50)]
        [Description("Streams about 1% of all Tweets in real-time.")]
        SampledStream,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/tweets/:id/retweeted_by", Group = "Retweets lookup", LimitPerApp = 75, LimitPerUser = 75, TweetCap = true)]
        [Description("Users who have Retweeted a Tweet")]
        RetweetsLookup,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/tweets/:id/quote_tweets", Group = "Retweets lookup", LimitPerApp = 75, LimitPerUser = 75, TweetCap = true)]
        [Description("Returns Quote Tweets for a Tweet specified by the requested Tweet ID.")]
        QuotesLookup,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.POST, Url = "/2/users/:id/retweets", Group = "Manage Retweets", LimitPerUser = 50, MaxPerUser = 300, MaxResetIntervalHours = 3, AdditionalInfo = "max shared with CreateTweet")]
        [Description("Allows a user ID to Retweet a Tweet")]
        CreateRetweet,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.DELETE, Url = "/2/users/:id/retweets/:tweet_id", Group = "Manage Retweets", LimitPerUser = 50, MaxPerUser = 1000, MaxResetIntervalHours = 24)]
        [Description("Allows a user ID to undo a Retweet")]
        DeleteRetweet,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/users/:id/liked_tweets", Group = "Likes lookup", LimitPerApp = 75, LimitPerUser = 75, TweetCap = true)]
        [Description("Tweets liked by a user")]
        TweetsLiked,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/tweets/:id/liking_users", Group = "Likes lookup", LimitPerApp = 75, LimitPerUser = 75)]
        [Description("Users who have liked a Tweet")]
        UsersLiked,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.POST, Url = "/2/users/:id/likes", Group = "Manage Likes", LimitPerUser = 50, MaxPerUser = 1000, MaxResetIntervalHours = 24, AdditionalInfo = "max shared with UnlikeTweet")]
        [Description("Allows a user ID to like a Tweet")]
        LikeTweet,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.DELETE, Url = "/2/users/:id/likes/:tweet_id", Group = "Manage Likes", LimitPerUser = 50, MaxPerUser = 1000, MaxResetIntervalHours = 24, AdditionalInfo = "max shared with LikeTweet")]
        [Description("Allows a user ID to unlike a Tweet")]
        UnlikeTweet,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.PUT, Url = "/2/tweets/:tweet_id/hidden", Group = "Hide replies", LimitPerUser = 50)]
        [Description("Hides or unhides a reply to a Tweet.")]
        HideReplies,

        #endregion

        #region Users

        [Endpoint(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "/2/users/by/username/:username", Group = "User lookup", LimitPerApp = 300, LimitPerUser = 900)]
        [Description("Retrieve a single user with a usernames")]
        GetUserByName,
        [Endpoint(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "/2/users/by", Group = "User lookup", LimitPerApp = 300, LimitPerUser = 900)]
        [Description("Retrieve multiple users with usernames")]
        GetUsersByNames,
        [Endpoint(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "/2/users/:id", Group = "User lookup", LimitPerApp = 300, LimitPerUser = 900)]
        [Description("Retrieve a single user with an ID")]
        GetUserById,
        [Endpoint(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "/2/users", Group = "User lookup", LimitPerApp = 300, LimitPerUser = 900)]
        [Description("Retrieve multiple users with IDs")]
        GetUsersByIds,
        [Endpoint(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "/2/users/me", Group = "User lookup", LimitPerUser = 75)]
        [Description("Returns the information about an authorized user")]
        GetUserMe,
        [Endpoint(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "/2/users/:id/followers", Group = "Follows lookup", LimitPerApp = 15, LimitPerUser = 15)]
        [Description("Lookup followers of a user by ID")]
        GetFollowersById,
        [Endpoint(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "/2/users/by/username/:username/followers", Group = "Follows lookup", LimitPerApp = 15, LimitPerUser = 15)]
        [Description("Lookup followers of a user by Username")]
        GetFollowersByName,
        [Endpoint(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "/2/users/:id/following", Group = "Follows lookup", LimitPerApp = 15, LimitPerUser = 15)]
        [Description("Lookup following of a user by ID")]
        GetFollowingsById,
        [Endpoint(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "/2/users/by/username/:username/following", Group = "Follows lookup", LimitPerApp = 15, LimitPerUser = 15)]
        [Description("Lookup following of a user by Username")]
        GetFollowingsByName,
        [Endpoint(Resource = Resource.Users, EndpointType = EndpointType.POST, Url = "/2/users/:id/following", Group = "Manage follows", LimitPerUser = 50, MaxPerUser = 400, MaxPerApp = 1000, MaxResetIntervalHours = 24)]
        [Description("Allows a user ID to follow another user")]
        FollowUser,
        [Endpoint(Resource = Resource.Users, EndpointType = EndpointType.DELETE, Url = "/2/users/:source_user_id/following/:target_user_id", Group = "Manage follows", LimitPerUser = 50, MaxPerApp = 500, MaxResetIntervalHours = 24)]
        [Description("Allows a user ID to unfollow another user")]
        UnfollowUser,
        [Endpoint(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "/2/users/:id/blocking", Group = "Blocks lookup", LimitPerUser = 15)]
        [Description("Returns a list of users who are blocked by the specified user ID")]
        BlocksLookup,
        [Endpoint(Resource = Resource.Users, EndpointType = EndpointType.POST, Url = "/2/users/:id/blocking", Group = "Manage blocks", LimitPerUser = 50)]
        [Description("Allows a user ID to block another user")]
        BlockUser,
        [Endpoint(Resource = Resource.Users, EndpointType = EndpointType.DELETE, Url = "/2/users/:source_user_id/blocking/:target_user_id", Group = "Manage blocks", LimitPerUser = 50)]
        [Description("Allows a user ID to unblock another user")]
        UnblockUser,
        [Endpoint(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "/2/users/:id/muting", Group = "Mutes lookup", LimitPerUser = 15)]
        [Description("Returns a list of users who are muted by the specified user ID")]
        MutesLookup,
        [Endpoint(Resource = Resource.Users, EndpointType = EndpointType.POST, Url = "/2/users/:id/muting", Group = "Manage mutes", LimitPerUser = 50)]
        [Description("Allows a user ID to mute another user")]
        MuteUser,
        [Endpoint(Resource = Resource.Users, EndpointType = EndpointType.DELETE, Url = "/2/users/:source_user_id/muting/:target_user_id", Group = "Manage mutes", LimitPerUser = 50)]
        [Description("Allows a user ID to unmute another user")]
        UnmuteUser,

        #endregion

        #region Spaces

        [Endpoint(Resource = Resource.Spaces, EndpointType = EndpointType.GET, Url = "/2/spaces/:id", Group = "Spaces lookup", LimitPerApp = 300, LimitPerUser = 300)]
        [Description("Lookup Space by ID")]
        GetSpaceById,
        [Endpoint(Resource = Resource.Spaces, EndpointType = EndpointType.GET, Url = "/2/spaces", Group = "Spaces lookup", LimitPerApp = 300, LimitPerUser = 300)]
        [Description("Lookup multiple Spaces by ID")]
        GetSpacesByIds,
        [Endpoint(Resource = Resource.Spaces, EndpointType = EndpointType.GET, Url = "/2/spaces/:id/tweets", Group = "Spaces lookup", LimitPerApp = 300, LimitPerUser = 300)]
        [Description("Returns Tweets shared in the requested Spaces.")]
        GetSpacesTweets,
        [Endpoint(Resource = Resource.Spaces, EndpointType = EndpointType.GET, Url = "/2/spaces/:id/buyers", Group = "Spaces lookup", LimitPerApp = 300, LimitPerUser = 300)]
        [Description("Get users who purchased a ticket to a Space")]
        GetSpaceBuyers,
        [Endpoint(Resource = Resource.Spaces, EndpointType = EndpointType.GET, Url = "/2/spaces/by/creator_ids", Group = "Spaces lookup", LimitPerApp = 300, LimitPerUser = 300)]
        [Description("Discover Spaces created by user ID")]
        GetSpaceByCreator,
        [Endpoint(Resource = Resource.Spaces, EndpointType = EndpointType.GET, Url = "/2/spaces/search", Group = "Search Spaces", LimitPerApp = 300, LimitPerUser = 300)]
        [Description("Return live or scheduled Spaces matching your specified search terms")]
        SearchSpaces,

        #endregion

        #region Lists

        [Endpoint(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "/2/lists/:id", Group = "List lookup", LimitPerApp = 75, LimitPerUser = 75)]
        [Description("Lookup a specific list by ID")]
        GetListById,
        [Endpoint(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "/2/users/:id/owned_lists", Group = "List lookup", LimitPerApp = 15, LimitPerUser = 15)]
        [Description("Lookup a user's owned List")]
        GetUserOwnedLists,
        [Endpoint(Resource = Resource.Lists, EndpointType = EndpointType.POST, Url = "/2/lists", Group = "Manage Lists", LimitPerUser = 300)]
        [Description("Creates a new List on behalf of an authenticated user")]
        CreateList,
        [Endpoint(Resource = Resource.Lists, EndpointType = EndpointType.DELETE, Url = "/2/lists/:id", Group = "Manage Lists", LimitPerUser = 300)]
        [Description("Deletes a List the authenticated user owns")]
        DeleteList,
        [Endpoint(Resource = Resource.Lists, EndpointType = EndpointType.PUT, Url = "/2/lists/:id", Group = "Manage Lists", LimitPerUser = 300)]
        [Description("Updates the metadata for a List the authenticated user owns")]
        UpdateList,
        [Endpoint(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "/2/lists/:id/tweets", Group = "List Tweets lookup", LimitPerApp = 900, LimitPerUser = 900, TweetCap = true)]
        [Description("Lookup Tweets from a specified List")]
        ListsTweetsLookup,
        [Endpoint(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "/2/lists/:id/members", Group = "List members", LimitPerApp = 900, LimitPerUser = 900)]
        [Description("Returns a list of members from a specified List")]
        GetListMember,
        [Endpoint(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "/2/users/:id/list_memberships", Group = "List members", LimitPerApp = 75, LimitPerUser = 75)]
        [Description("Returns all Lists a specified user is a member of")]
        GetUsersList,
        [Endpoint(Resource = Resource.Lists, EndpointType = EndpointType.POST, Url = "/2/lists/:id/members", Group = "Manage List members", LimitPerUser = 300)]
        [Description("Add a member to a List that the authenticated user owns")]
        ListAddMember,
        [Endpoint(Resource = Resource.Lists, EndpointType = EndpointType.DELETE, Url = "/2/lists/:id/members/:user_id", Group = "Manage List members", LimitPerUser = 300)]
        [Description("Removes a member from a List the authenticated user owns")]
        ListRemoveMember,
        [Endpoint(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "/2/lists/:id/followers", Group = "List follows", LimitPerApp = 180, LimitPerUser = 180)]
        [Description("Returns all followers of a specified List")]
        GetListFollowers,
        [Endpoint(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "/2/users/:id/followed_lists", Group = "List follows", LimitPerApp = 15, LimitPerUser = 15)]
        [Description("Returns all Lists a specified user follows")]
        GetUsersFollowedList,
        [Endpoint(Resource = Resource.Lists, EndpointType = EndpointType.POST, Url = "/2/users/:id/followed_lists", Group = "Manage List follows", LimitPerUser = 50)]
        [Description("Follows a List on behalf of an authenticated user")]
        FollowList,
        [Endpoint(Resource = Resource.Lists, EndpointType = EndpointType.DELETE, Url = "/2/users/:id/followed_lists/:list_id", Group = "Manage List follows", LimitPerUser = 50)]
        [Description("Unfollows a List on behalf of an authenticated user")]
        UnfollowList,
        [Endpoint(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "/2/users/:id/pinned_lists", Group = "Manage pinned Lists", LimitPerUser = 15)]
        [Description("Returns the pinned Lists of the authenticated user")]
        GetUsersPinnedLists,
        [Endpoint(Resource = Resource.Lists, EndpointType = EndpointType.POST, Url = "/2/users/:id/pinned_lists", Group = "Manage pinned Lists", LimitPerUser = 50)]
        [Description("Pins a List on behalf of an authenticated user")]
        PinList,
        [Endpoint(Resource = Resource.Lists, EndpointType = EndpointType.DELETE, Url = "/2/users/:id/pinned_lists/:list_id", Group = "Manage pinned Lists", LimitPerUser = 50)]
        [Description("Unpins a List on behalf of an authenticated user")]
        UnpinList,

        #endregion

        #region Bookmarks

        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/users/:id/bookmarks", Group = "Bookmarks lookup", LimitPerUser = 180)]
        [Description("Allows you to get an authenticated user's 800 most recent bookmarked Tweets")]
        BookmarksLookup,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.POST, Url = "/2/users/:id/bookmarks", Group = "Manage Bookmarks", LimitPerUser = 50)]
        [Description("Causes the user ID of an authenticated user identified in the path parameter to Bookmark the target Tweet provided in the request body")]
        BookmarkTweet,
        [Endpoint(Resource = Resource.Tweets, EndpointType = EndpointType.DELETE, Url = "/2/users/:id/bookmarks/:tweet_id", Group = "Manage Bookmarks", LimitPerUser = 50)]
        [Description("Allows a user or authenticated user ID to remove a Bookmark of a Tweet")]
        RemoveBookmark,

        #endregion

        #region Compliance

        [Endpoint(Resource = Resource.Compliance, EndpointType = EndpointType.POST, Url = "/2/compliance/jobs", Group = "Batch compliance", LimitPerApp = 150)]
        [Description("Creates a new compliance job")]
        CreateJob,
        [Endpoint(Resource = Resource.Compliance, EndpointType = EndpointType.GET, Url = "/2/compliance/jobs", Group = "Batch compliance", LimitPerApp = 150)]
        [Description("Returns a list of recent compliance jobs")]
        GetJobs,
        [Endpoint(Resource = Resource.Compliance, EndpointType = EndpointType.GET, Url = "/2/compliance/jobs/:job_id", Group = "Batch compliance", LimitPerApp = 150)]
        [Description("Returns status and download information about a specified compliance job")]
        GetJobById,

        #endregion
    }
}
