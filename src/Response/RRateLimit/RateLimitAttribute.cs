using System;

namespace TwitterSharp.Response.RRateLimit
{
    public class RateLimitAttribute : Attribute
    {
        public Resource Resource { get; set; }
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
