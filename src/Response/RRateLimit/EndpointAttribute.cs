using System;

namespace TwitterSharp.Response.RRateLimit
{
    public class EndpointAttribute : Attribute
    {
        public Resource Resource { get; set; }
        public EndpointType EndpointType { get; set; } = EndpointType.GET;
        public AccessLevel AccessLevel { get; set; } = AccessLevel.All;
        public string Url { get; set; }

        /// <summary>
        /// Certain endpoints (like filtered stream and recent search) have a limit on how many Tweets they can pull
        /// per month. <see href="https://developer.twitter.com/en/docs/twitter-api/tweet-caps">Learn more</see>
        /// </summary>
        public bool TweetCap { get; set; }
        public string Group { get; set; }
        public int LimitPerApp { get; set; }
        public int LimitPerUser { get; set; }
        public int LimitResetIntervalMinutes { get; set; } = 15;
        public int MaxPerApp { get; set; }
        public int MaxPerUser { get; set; }
        public int MaxResetIntervalHours { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
