using System.ComponentModel;

namespace TwitterSharp.Response.RRateLimit
{
    /// <summary>
    /// All endpoints with rate limits enhanced with all information from <see href="https://developer.twitter.com/en/portal/products/"/>,
    /// <see href="https://developer.twitter.com/en/docs/twitter-api/rate-limits"/> and <see href="https://developer.twitter.com/en/docs/api-reference-index"/>
    /// There is also a Tweet Cap per month depending the Acceess Level:
    /// Essential: 500K Tweets per month / Project
    /// Elevated: 2M Tweets per month / Project
    /// Academic: 10M Tweets per month / Project
    /// </summary>
    public enum Endpoint
    {
        #region tweets

        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/tweets", Group = "Tweet lookup", LimitPerApp = 300, LimitPerUser = 900)]
        [Description("Retrieve multiple Tweets with a list of IDs")]
        GetTweetsByIds,
        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/tweets/:id", Group = "Tweet lookup", LimitPerApp = 300, LimitPerUser = 900)]
        [Description("Retrieve a single Tweet with an ID")]
        GetTweetById,
        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.POST, Url = "/2/tweets", Group = "Manage Tweets", LimitPerUser = 200, MaxPerUser = 300, MaxResetIntervalHours = 3, AdditionalInfo = "max shared with CreateRetweet")]
        [Description("Post a Tweet")]
        CreateTweet,
        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.DELETE, Url = "/2/tweets/:id", Group = "Manage Tweets", LimitPerUser = 50)]
        [Description("Delete a Tweet")]
        DeleteTweet,
        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/users/:id/tweets", Group = "Timelines", LimitPerApp = 1500, LimitPerUser = 900, TweetCap = true)]
        [Description("Returns most recent Tweets composed a specified user ID")]
        UserTweetTimeline,
        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/users/:id/mentions", Group = "Timelines", LimitPerApp = 450, LimitPerUser = 180, TweetCap = true)]
        [Description("Returns most recent Tweets mentioning a specified user ID")]
        UserMentionTimeline,
        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/tweets/search/recent", Group = "Search Tweets", LimitPerApp = 450, LimitPerUser = 180, TweetCap = true)]
        [Description("Search for Tweets published in the last 7 days")]
        // Additional Limits
        // - 10 default results per response
        // - 100 results per response
        // 
        // Essential + Elevated:
        // - core operators
        // - 512 query length
        // Academic:
        // - enhanced operators
        // - 1024 query length
        RecentSearch,
        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "/2/tweets/search/all", Group = "Search Tweets", LimitPerApp = 300, TweetCap = true, AdditionalInfo = "Only available to those with Academic Research access. Full-archive also has a 1 request / 1 second limit")]
        [Description("Search the full archive of Tweets")]
        // Additional Limits
        // - 500 results per response
        // - 10 default results per response
        // - enhanced operators
        // - 1024 query length
        FullArchiveSearch,
        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Tweet counts", LimitPerApp = 300)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        RecentTweetCounts,
        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Tweet counts", LimitPerApp = 300)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        FullArchiveTweetCounts,
        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Filtered stream", LimitPerApp = 50)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        ConnectingFilteresStream,
        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Filtered stream", LimitPerApp = 25, AdditionalInfo = "Essential access - 25, Elevated access - 50, Academic Research access - 100")]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        AddingDeletingFilters,
        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Filtered stream", LimitPerApp = 450)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        ListingFilters,
        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Sampled stream", LimitPerApp = 50)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        SampledStream,
        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Retweets lookup", LimitPerApp = 75, LimitPerUser = 75)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        RetweetsLookup,
        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Manage Retweets", LimitPerUser = 50, MaxPerUser = 300, MaxResetIntervalHours = 3, AdditionalInfo = "max shared with CreateTweet")]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        CreateRetweet,
        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Manage Retweets", LimitPerUser = 50, MaxPerUser = 1000, MaxResetIntervalHours = 24)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        DeleteRetweet,
        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Likes lookup", LimitPerApp = 75, LimitPerUser = 75)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        TweetsLiked,
        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Likes lookup", LimitPerApp = 75, LimitPerUser = 75)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        UsersLiked,
        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Manage Likes", LimitPerUser = 50, MaxPerUser = 1000, MaxResetIntervalHours = 24, AdditionalInfo = "max shared with UnlikeTweet")]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        LikeTweet,
        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Manage Likes", LimitPerUser = 50, MaxPerUser = 1000, MaxResetIntervalHours = 24, AdditionalInfo = "max shared with LikeTweet")]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        UnlikeTweet,
        [RateLimit(Resource = Resource.Tweets, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Hide replies", LimitPerUser = 50)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        HideReplies,

        #endregion

        #region Users

        [RateLimit(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "User lookup", LimitPerApp = 300, LimitPerUser = 25)] // Not documented but tested, LimitPerUser guested
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        GetUserByName,
        [RateLimit(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "User lookup", LimitPerApp = 300, LimitPerUser = 25)] // Not documented but tested, LimitPerUser guested
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        GetUsersByNames,
        [RateLimit(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "User lookup", LimitPerApp = 300, LimitPerUser = 25)] // Not documented but tested, LimitPerUser guested
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        GetUserById,
        [RateLimit(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "User lookup", LimitPerApp = 300, LimitPerUser = 25)] // Not documented but tested, LimitPerUser guested
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        GetUsersByIds,
        [RateLimit(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "User lookup", LimitPerUser = 75)] // Not documented but tested, LimitPerUser guested
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        GetUserMe,
        [RateLimit(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Follows lookup", LimitPerApp = 15, LimitPerUser = 15)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        FollowsLookup,
        [RateLimit(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Manage follows", LimitPerUser = 50, MaxPerUser = 400, MaxPerApp = 1000, MaxResetIntervalHours = 24)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        FollowUser,
        [RateLimit(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Manage follows", LimitPerUser = 50, MaxPerApp = 500, MaxResetIntervalHours = 24)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        UnfollowUser,
        [RateLimit(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Blocks lookup", LimitPerUser = 15)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        BlocksLookup,
        [RateLimit(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Manage blocks", LimitPerUser = 50)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        BlockUser,
        [RateLimit(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Manage blocks", LimitPerUser = 50)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        UnblockUser,
        [RateLimit(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Mutes lookup", LimitPerUser = 15)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        MutesLookup,
        [RateLimit(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Manage mutes", LimitPerUser = 50)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        MuteUser,
        [RateLimit(Resource = Resource.Users, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Manage mutes", LimitPerUser = 50)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        UnmuteUser,

        #endregion

        #region Spaces

        [RateLimit(Resource = Resource.Spaces, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Spaces lookup", LimitPerApp = 300)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        SpacesLookup,
        [RateLimit(Resource = Resource.Spaces, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Search Spaces", LimitPerApp = 300)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        SearchSpaces,

        #endregion

        #region Lists

        [RateLimit(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Manage Lists", LimitPerUser = 300)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        CreateList,
        [RateLimit(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Manage Lists", LimitPerUser = 300)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        DeleteList,
        [RateLimit(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Manage Lists", LimitPerUser = 300)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        UpdateList,
        [RateLimit(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "List lookup", LimitPerApp = 75, LimitPerUser = 75)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        GetListById,
        [RateLimit(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "List lookup", LimitPerApp = 15, LimitPerUser = 15)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        GetUserOwnedLists,
        [RateLimit(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "List Tweets lookup", LimitPerApp = 900, LimitPerUser = 900)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        ListTweetsLookup,
        [RateLimit(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "List members", LimitPerUser = 300)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        ListAddMember,
        [RateLimit(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "List members", LimitPerUser = 300)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        ListRemoveMember,
        [RateLimit(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "List members", LimitPerApp = 900, LimitPerUser = 900)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        GetListMember,
        [RateLimit(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "List members", LimitPerApp = 75, LimitPerUser = 75)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        GetUsersList,
        [RateLimit(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "List follows", LimitPerUser = 50)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        FollowList,
        [RateLimit(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "List follows", LimitPerUser = 50)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        UnfollowList,
        [RateLimit(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "List follows", LimitPerApp = 180, LimitPerUser = 180)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        GetListFollowers,
        [RateLimit(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "List follows", LimitPerApp = 15, LimitPerUser = 15)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        GetUsersFollowedList,
        [RateLimit(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Manage pinned Lists", LimitPerUser = 50)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        PinList,
        [RateLimit(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Manage pinned Lists", LimitPerUser = 50)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        UnpinList,
        [RateLimit(Resource = Resource.Lists, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Manage pinned Lists", LimitPerUser = 15)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        GetUsersPinnedLists,

        #endregion

        #region Compliance

        [RateLimit(Resource = Resource.Compliance, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Batch compliance", LimitPerUser = 150)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        CreateJob,
        [RateLimit(Resource = Resource.Compliance, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Batch compliance", LimitPerUser = 150)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        GetJobs,
        [RateLimit(Resource = Resource.Compliance, EndpointType = EndpointType.GET, Url = "XXXXXXXXXXXXX", Group = "Batch compliance", LimitPerUser = 150)]
        [Description("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
        GetJobById,

        #endregion
    }
}
