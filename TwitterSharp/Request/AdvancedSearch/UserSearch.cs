namespace TwitterSharp.Request.AdvancedSearch
{
    // https://developer.twitter.com/en/docs/twitter-api/data-dictionary/object-model/user
    public enum UserSearch
    {
        /// <summary>
        /// UTC Datetime that the user account was created
        /// </summary>
        Created_At,

        /// <summary>
        /// Text on user's profile
        /// </summary>
        Description,

        // Entities // Not implemented yet

        /// <summary>
        /// Location specified on user's profile
        /// </summary>
        Location,

        /// <summary>
        /// ID of the tweet pinned on top of the user's profile
        /// </summary>
        Pinned_Tweet_Id,

        /// <summary>
        /// Url of the profile image
        /// </summary>
        Profile_Image_Url,

        /// <summary>
        /// Boolean specifying if the user's tweets are visible or not
        /// </summary>
        Protected,

        /// <summary>
        /// Contains the following information: number of followers, number of following, numbed of tweet, number of "listed"
        /// </summary>
        Public_Metrics,

        /// <summary>
        /// Url specified on the user's profile
        /// </summary>
        Url,

        /// <summary>
        /// Boolean telling if the account is marked as "verified"
        /// </summary>
        Verified

        // Withheld // Not implemented yet
    }
}
