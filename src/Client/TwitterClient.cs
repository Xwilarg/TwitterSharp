using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TwitterSharp.ApiEndpoint;
using TwitterSharp.JsonOption;
using TwitterSharp.Request.Option;
using TwitterSharp.Response;
using TwitterSharp.Response.RUser;

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

        #region Follows
        private async Task<Follow> NextFollowAsync(string baseQuery, string token, Endpoint endpoint)
        {
            var res = await _httpClient.GetAsync(baseQuery + (!baseQuery.EndsWith("?") ? "&" : "") + "pagination_token=" + token);
            var data = ParseData<User[]>(await res.Content.ReadAsStringAsync());
            BuildRateLimit(res.Headers, endpoint);
            return new()
            {
                Users = data.Data,
                NextAsync = data.Meta.NextToken == null ? null : async () => await NextFollowAsync(baseQuery, data.Meta.NextToken, endpoint)
            };
        }

        /// <summary>
        /// Get the follower of an user
        /// </summary>
        /// <param name="id">ID of the user</param>
        /// <param name="limit">Max number of result, max is 1000</param>
        public async Task<Follow> GetFollowersAsync(string id, UserSearchOptions options = null)
        {
            options ??= new();
            var query = _baseUrl + $"users/{HttpUtility.UrlEncode(id)}/followers?{options.Build(false)}";
            var res = await _httpClient.GetAsync(query);
            var data = ParseData<User[]>(await res.Content.ReadAsStringAsync());
            BuildRateLimit(res.Headers, Endpoint.GetFollowersById);
            return new()
            {
                Users = data.Data,
                NextAsync = data.Meta.NextToken == null ? null : async () => await NextFollowAsync(query, data.Meta.NextToken, Endpoint.GetFollowersById)
            };
        }

        /// <summary>
        /// Get the following of an user
        /// </summary>
        /// <param name="id">ID of the user</param>
        /// <param name="limit">Max number of result, max is 1000</param>
        public async Task<Follow> GetFollowingAsync(string id, UserSearchOptions options = null)
        {
            options ??= new();
            var query = _baseUrl + $"users/{HttpUtility.UrlEncode(id)}/following?{options.Build(false)}";
            var res = await _httpClient.GetAsync(query);
            var data = ParseData<User[]>(await res.Content.ReadAsStringAsync());
            BuildRateLimit(res.Headers, Endpoint.GetFollowingsById);
            return new()
            {
                Users = data.Data,
                NextAsync = data.Meta.NextToken == null ? null : async () => await NextFollowAsync(query, data.Meta.NextToken, Endpoint.GetFollowingsById)
            };
        }
        #endregion Follows

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
