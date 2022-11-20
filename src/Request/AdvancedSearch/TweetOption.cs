namespace TwitterSharp.Request.AdvancedSearch
{
    // https://developer.twitter.com/en/docs/twitter-api/data-dictionary/object-model/tweet
    public enum TweetOption
    {
        // TODO:
        // context_annotations
        // geo
        // non_public_metrics
        // organic_metrics
        // promoted_metrics

        /// <summary>
        /// The ID of the author of the tweet
        /// </summary>
        Author_Id,

        /// <summary>
        /// The ID of the tweet this one is replying to
        /// </summary>
        Conversation_Id,

        /// <summary>
        /// Datetime when the tweet was crated
        /// </summary>
        Created_At,

        /// <summary>
        /// Entities in the tweet
        /// </summary>
        Entities,

        /// <summary>
        /// In the case of a reply, will contains the original user id
        /// </summary>
        In_Reply_To_User_Id,

        /// <summary>
        /// Language of the tweet, detect by Twitter
        /// </summary>
        Lang,

        /// <summary>
        /// If contains a link, indicate if it might lead to a sensitive content
        /// </summary>
        Possibly_Sensitive,

        /// <summary>
        /// Contains the following information: number of likes, number of retweets, number of replies, number of quotes 
        /// </summary>
        Public_Metrics,

        /// <summary>
        /// Number of tweet this one reference
        /// </summary>
        Referenced_Tweets,

        /// <summary>
        /// Show who can reply to this tweet
        /// </summary>
        Reply_Settings,

        /// <summary>
        /// Name of the app this tweet was sent from
        /// </summary>
        Source,

        /// <summary>
        /// Polls and medias attached to the tweet
        /// Also get their ID
        /// </summary>
        Attachments,

        /// <summary>
        /// Same at "Attachments" but only get their ID
        /// </summary>
        Attachments_Ids
    }
}
