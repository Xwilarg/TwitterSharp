namespace TwitterSharp.Request.AdvancedSearch
{
    // https://developer.twitter.com/en/docs/twitter-api/data-dictionary/object-model/tweet
    public enum TweetOption
    {
        // TODO:
        // attachments
        // context_annotations
        // geo
        // non_public_metrics
        // organic_metrics
        // promoted_metrics

        Conversation_Id,
        Created_At,
        Entities,
        In_Reply_To_User_Id,
        Lang,
        Possibly_Sensitive,
        Public_Metrics,
        Referenced_Tweets,
        Reply_Settings,
        Source
    }
}
