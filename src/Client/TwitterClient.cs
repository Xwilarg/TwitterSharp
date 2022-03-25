using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using TwitterSharp.JsonOption;
using TwitterSharp.Response;

namespace TwitterSharp.Client
{
    /// <summary>
    /// Base client to do all your requests
    /// </summary>
    public partial class TwitterClient : IDisposable
    {
        /// <summary>
        /// Create a new instance of the client
        /// </summary>
        /// <param name="bearerToken">Bearer token generated from https://developer.twitter.com/</param>
        public TwitterClient(string bearerToken)
        {
            _httpClient = new();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new SnakeCaseNamingPolicy()
            };
            _jsonOptions.Converters.Add(new EntitiesConverter());
            _jsonOptions.Converters.Add(new ExpressionConverter());
            _jsonOptions.Converters.Add(new ReferencedTweetConverter());
            _jsonOptions.Converters.Add(new ReplySettingsConverter());
            _jsonOptions.Converters.Add(new MediaConverter());
        }

        public event EventHandler<RateLimit> RateLimitChanged;
        private CancellationTokenSource _tweetStreamCancellationTokenSource;

        private const string _baseUrl = "https://api.twitter.com/2/";

        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public void Dispose()
        {
            CancelTweetStream();
            _httpClient?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
