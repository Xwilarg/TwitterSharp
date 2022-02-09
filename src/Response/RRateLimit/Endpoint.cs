namespace TwitterSharp.Response.RRateLimit
{
    /// <summary>
    /// All endpoints with rate limits enhanced with all information from <see href="https://developer.twitter.com/en/docs/twitter-api/rate-limits"/>
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
        [RateLimit(Resource = Resource.Tweets, Group = "Manage Retweets", LimitPerUser = 50, Max = 300, MaxResetIntervalHours = 3, AdditionalInfo = "max shared with CreateTweet")]
        CreateRetweet,
        [RateLimit(Resource = Resource.Tweets, Group = "Manage Retweets", LimitPerUser = 50, Max = 1000, MaxResetIntervalHours = 24)]
        DeleteRetweet,
        [RateLimit(Resource = Resource.Tweets, Group = "Likes lookup", LimitPerApp = 75, LimitPerUser = 75)]
        TweetsLikedByAUser,
        [RateLimit(Resource = Resource.Tweets, Group = "Likes lookup", LimitPerApp = 75, LimitPerUser = 75)]
        UsersWhoHaveLikedATweet,
        [RateLimit(Resource = Resource.Tweets, Group = "Manage Tweets", LimitPerUser = 200, Max = 300, MaxResetIntervalHours = 3, AdditionalInfo = "max shared with CreateRetweet")]
        CreateTweet,
        [RateLimit(Resource = Resource.Tweets, Group = "Manage Tweets", LimitPerUser = 50)]
        DeleteTweet,
        [RateLimit(Resource = Resource.Tweets, Group = "Manage Likes", LimitPerUser = 50, Max = 1000, MaxResetIntervalHours = 24, AdditionalInfo = "max shared with UnlikeTweet")]
        LikeTweet,
        [RateLimit(Resource = Resource.Tweets, Group = "Manage Likes", LimitPerUser = 50, Max = 1000, MaxResetIntervalHours = 24, AdditionalInfo = "max shared with LikeTweet")]
        UnlikeTweet,
        [RateLimit(Resource = Resource.HideReplies, Group = "Hide replies", LimitPerUser = 50)]
        HideReplies,
    }
}
