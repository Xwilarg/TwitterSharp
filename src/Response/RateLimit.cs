﻿using TwitterSharp.ApiEndpoint;

namespace TwitterSharp.Response
{
    public class RateLimit
    {
        public Endpoint Endpoint { get; set; }
        public int Limit { get; internal set; }
        public int Remaining { get; internal set; }
        public int Reset { get; internal set; }

        public RateLimit(Endpoint endpoint)
        {
            Endpoint = endpoint;
        }
    }
}
