namespace TwitterSharp.Response.RRateLimit
{
    /// <summary>
    /// All endpoints with rate limits enhanced with all information from <see href="https://developer.twitter.com/en/docs/twitter-api/rate-limits"/>
    /// and <see href="https://developer.twitter.com/en/docs/api-reference-index"/>
    /// </summary>
    public enum Endpoint
    {
        [RateLimit(Resource = Resource.Tweets, Group = "Tweet lookup", LimitPerApp = 900, LimitPerUser = 900)]
        TweetLookup,
        [RateLimit(Resource = Resource.Tweets, Group = "Timelines", LimitPerApp = 1500, LimitPerUser = 900)]
        UserTweetTimeline,
        [RateLimit(Resource = Resource.Tweets, Group = "Timelines", LimitPerApp = 450, LimitPerUser = 180)]
        UserMentionTimeline,
        [RateLimit(Resource = Resource.Tweets, Group = "Search Tweets", LimitPerApp = 450, LimitPerUser = 180)]
        RecentSearch,
        [RateLimit(Resource = Resource.Tweets, Group = "Search Tweets", LimitPerApp = 300, AdditionalInfo= "Full-archive also has a 1 request / 1 second limit")]
        FullArchiveSearch,
        [RateLimit(Resource = Resource.Tweets, Group = "Tweet counts", LimitPerApp = 300)]
        RecentTweetCounts,
        [RateLimit(Resource = Resource.Tweets, Group = "Tweet counts", LimitPerApp = 300)]
        FullArchiveTweetCounts,
        [RateLimit(Resource = Resource.Tweets, Group = "Filtered stream", LimitPerApp = 50)]
        ConnectingFilteresStream,
        [RateLimit(Resource = Resource.Tweets, Group = "Filtered stream", LimitPerApp = 25, AdditionalInfo = "Essential access - 25, Elevated access - 50, Academic Research access - 100")]
        AddingDeletingFilters,
        [RateLimit(Resource = Resource.Tweets, Group = "Filtered stream", LimitPerApp = 450)]
        ListingFilters,
        [RateLimit(Resource = Resource.Tweets, Group = "Sampled stream", LimitPerApp = 50)]
        SampledStream,
        [RateLimit(Resource = Resource.Tweets, Group = "Retweets lookup", LimitPerApp = 75, LimitPerUser = 75)]
        RetweetsLookup,
        [RateLimit(Resource = Resource.Tweets, Group = "Manage Retweets", LimitPerUser = 50, MaxPerUser = 300, MaxResetIntervalHours = 3, AdditionalInfo = "max shared with CreateTweet")]
        CreateRetweet,
        [RateLimit(Resource = Resource.Tweets, Group = "Manage Retweets", LimitPerUser = 50, MaxPerUser = 1000, MaxResetIntervalHours = 24)]
        DeleteRetweet,
        [RateLimit(Resource = Resource.Tweets, Group = "Likes lookup", LimitPerApp = 75, LimitPerUser = 75)]
        TweetsLiked,
        [RateLimit(Resource = Resource.Tweets, Group = "Likes lookup", LimitPerApp = 75, LimitPerUser = 75)]
        UsersLiked,
        [RateLimit(Resource = Resource.Tweets, Group = "Manage Tweets", LimitPerUser = 200, MaxPerUser = 300, MaxResetIntervalHours = 3, AdditionalInfo = "max shared with CreateRetweet")]
        CreateTweet,
        [RateLimit(Resource = Resource.Tweets, Group = "Manage Tweets", LimitPerUser = 50)]
        DeleteTweet,
        [RateLimit(Resource = Resource.Tweets, Group = "Manage Likes", LimitPerUser = 50, MaxPerUser = 1000, MaxResetIntervalHours = 24, AdditionalInfo = "max shared with UnlikeTweet")]
        LikeTweet,
        [RateLimit(Resource = Resource.Tweets, Group = "Manage Likes", LimitPerUser = 50, MaxPerUser = 1000, MaxResetIntervalHours = 24, AdditionalInfo = "max shared with LikeTweet")]
        UnlikeTweet,
        [RateLimit(Resource = Resource.HideReplies, Group = "Hide replies", LimitPerUser = 50)]
        HideReplies,
        // [RateLimit(Resource = Resource.Users, Group = "User lookup", LimitPerApp = 900, LimitPerUser = 75)]
        // UserLookup,
        [RateLimit(Resource = Resource.Users, Group = "User lookup", LimitPerApp = 300, LimitPerUser = 25)] // Not documented but tested, LimitPerUser guested
        GetUserByName,
        [RateLimit(Resource = Resource.Users, Group = "User lookup", LimitPerApp = 300, LimitPerUser = 25)] // Not documented but tested, LimitPerUser guested
        GetUsersByNames,
        [RateLimit(Resource = Resource.Users, Group = "User lookup", LimitPerApp = 300, LimitPerUser = 25)] // Not documented but tested, LimitPerUser guested
        GetUserById,
        [RateLimit(Resource = Resource.Users, Group = "User lookup", LimitPerApp = 300, LimitPerUser = 25)] // Not documented but tested, LimitPerUser guested
        GetUsersByIds,
        [RateLimit(Resource = Resource.Users, Group = "User lookup", LimitPerUser = 75)] // Not documented but tested, LimitPerUser guested
        GetUserMe,
        [RateLimit(Resource = Resource.Users, Group = "Follows lookup", LimitPerApp = 15, LimitPerUser = 15)]
        FollowsLookup,
        [RateLimit(Resource = Resource.Users, Group = "Manage follows", LimitPerUser = 50, MaxPerUser = 400, MaxPerApp = 1000, MaxResetIntervalHours = 24)]
        FollowUser,
        [RateLimit(Resource = Resource.Users, Group = "Manage follows", LimitPerUser = 50, MaxPerApp = 500, MaxResetIntervalHours = 24)]
        UnfollowUser,
        [RateLimit(Resource = Resource.Users, Group = "Blocks lookup", LimitPerUser = 15)]
        BlocksLookup,
        [RateLimit(Resource = Resource.Users, Group = "Manage blocks", LimitPerUser = 50)]
        BlockUser,
        [RateLimit(Resource = Resource.Users, Group = "Manage blocks", LimitPerUser = 50)]
        UnblockUser,
        [RateLimit(Resource = Resource.Users, Group = "Mutes lookup", LimitPerUser = 15)]
        MutesLookup,
        [RateLimit(Resource = Resource.Users, Group = "Manage mutes", LimitPerUser = 50)]
        MuteUser,
        [RateLimit(Resource = Resource.Users, Group = "Manage mutes", LimitPerUser = 50)]
        UnmuteUser,
        [RateLimit(Resource = Resource.Lists, Group = "Manage Lists", LimitPerUser = 300)]
        CreateList,
        [RateLimit(Resource = Resource.Lists, Group = "Manage Lists", LimitPerUser = 300)]
        DeleteList,
        [RateLimit(Resource = Resource.Lists, Group = "Manage Lists", LimitPerUser = 300)]
        UpdateList,
        [RateLimit(Resource = Resource.Lists, Group = "List lookup", LimitPerApp = 75, LimitPerUser = 75)]
        GetListById,
        [RateLimit(Resource = Resource.Lists, Group = "List lookup", LimitPerApp = 15, LimitPerUser = 15)]
        GetUserOwnedLists,
        [RateLimit(Resource = Resource.Lists, Group = "List Tweets lookup", LimitPerApp = 900, LimitPerUser = 900)]
        ListTweetsLookup,
        [RateLimit(Resource = Resource.Lists, Group = "List members", LimitPerUser = 300)]
        ListAddMember,
        [RateLimit(Resource = Resource.Lists, Group = "List members", LimitPerUser = 300)]
        ListRemoveMember,
        [RateLimit(Resource = Resource.Lists, Group = "List members", LimitPerApp = 900, LimitPerUser = 900)]
        GetListMember,
        [RateLimit(Resource = Resource.Lists, Group = "List members", LimitPerApp = 75, LimitPerUser = 75)]
        GetUsersList,
        [RateLimit(Resource = Resource.Lists, Group = "List follows", LimitPerUser = 50)]
        FollowList,
        [RateLimit(Resource = Resource.Lists, Group = "List follows", LimitPerUser = 50)]
        UnfollowList,
        [RateLimit(Resource = Resource.Lists, Group = "List follows", LimitPerApp = 180, LimitPerUser = 180)]
        GetListFollowers,
        [RateLimit(Resource = Resource.Lists, Group = "List follows", LimitPerApp = 15, LimitPerUser = 15)]
        GetUsersFollowedList,
        [RateLimit(Resource = Resource.Lists, Group = "Manage pinned Lists", LimitPerUser = 50)]
        PinList,
        [RateLimit(Resource = Resource.Lists, Group = "Manage pinned Lists", LimitPerUser = 50)]
        UnpinList,
        [RateLimit(Resource = Resource.Lists, Group = "Manage pinned Lists", LimitPerUser = 15)]
        GetUsersPinnedLists,
        [RateLimit(Resource = Resource.Spaces, Group = "Spaces lookup", LimitPerApp = 300)]
        SpacesLookup,
        [RateLimit(Resource = Resource.Spaces, Group = "Search Spaces", LimitPerApp = 300)]
        SearchSpaces,
        [RateLimit(Resource = Resource.Compliance, Group = "Batch compliance", LimitPerUser = 150)]
        CreateJob,
        [RateLimit(Resource = Resource.Compliance, Group = "Batch compliance", LimitPerUser = 150)]
        GetJobs,
        [RateLimit(Resource = Resource.Compliance, Group = "Batch compliance", LimitPerUser = 150)]
        GetJobById,
    }
}
