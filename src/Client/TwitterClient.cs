using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TwitterSharp.ApiEndpoint;
using TwitterSharp.JsonOption;
using TwitterSharp.Model;
using TwitterSharp.Request;
using TwitterSharp.Request.Internal;
using TwitterSharp.Request.Option;
using TwitterSharp.Response;
using TwitterSharp.Response.RStream;
using TwitterSharp.Response.RTweet;
using TwitterSharp.Response.RUser;
using TwitterSharp.Rule;

namespace TwitterSharp.Client
{
    /// <summary>
    /// Base client to do all your requests
    /// </summary>
    public class TwitterClient : IDisposable
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

        #region AdvancedParsing
        private static void IncludesParseUser(IHaveAuthor data, Includes includes)
        {
            data.SetAuthor(includes.Users.Where(x => x.Id == data.AuthorId).FirstOrDefault());
        }

        private static void IncludesParseUser(IHaveAuthor[] data, Includes includes)
        {
            foreach (var d in data)
            {
                IncludesParseUser(d, includes);
            }
        }

        private static void IncludesParseMedias(IHaveMedia data, Includes includes)
        {
            var medias = data.GetMedia();
            if (medias != null)
            {
                for (int i = 0; i < medias.Length; i++)
                {
                    medias[i] = includes.Media.Where(x => x.Key == medias[i].Key).FirstOrDefault();
                }
            }
        }

        private static void IncludesParseMedias(IHaveMedia[] data, Includes includes)
        {
            foreach (var m in data)
            {
                IncludesParseMedias(m, includes);
            }
        }

        private static readonly Type _authorInterface = typeof(IHaveAuthor);
        private static readonly Type _mediaInterface = typeof(IHaveMedia);
        private static readonly Type _matchingRulesInterface = typeof(IHaveMatchingRules);
        private static void InternalIncludesParse<T>(Answer<T> answer)
        {
            if (answer.Includes != null)
            {
                if (answer.Includes.Users != null && answer.Includes.Users.Any() && _authorInterface.IsAssignableFrom(typeof(T)))
                {
                    var data = answer.Data;
                    IncludesParseUser((IHaveAuthor)data, answer.Includes);
                    answer.Data = data;
                }
                if (answer.Includes.Media != null && answer.Includes.Media.Any() && _mediaInterface.IsAssignableFrom(typeof(T)))
                {
                    var data = answer.Data;
                    IncludesParseMedias((IHaveMedia)data, answer.Includes);
                    answer.Data = data;
                }
                if (answer.MatchingRules != null && answer.MatchingRules.Any() && _matchingRulesInterface.IsAssignableFrom(typeof(T)))
                {
                    (answer.Data as IHaveMatchingRules).MatchingRules = answer.MatchingRules;
                }
            }
        }
        private static void InternalIncludesParse<T>(Answer<T[]> answer)
        {
            if (answer.Includes != null)
            {
                if (answer.Includes.Users != null && answer.Includes.Users.Any() && answer.Includes.Users.Any() && _authorInterface.IsAssignableFrom(typeof(T)))
                {
                    var data = answer.Data;
                    IncludesParseUser(data.Cast<IHaveAuthor>().ToArray(), answer.Includes);
                    answer.Data = data;
                }
                if (answer.Includes.Media != null && answer.Includes.Media.Any() && _mediaInterface.IsAssignableFrom(typeof(T)))
                {
                    var data = answer.Data;
                    IncludesParseMedias(data.Cast<IHaveMedia>().ToArray(), answer.Includes);
                    answer.Data = data;
                }
            }
        }

        private Answer<T[]> ParseArrayData<T>(string json)
        {
            var answer = JsonSerializer.Deserialize<Answer<T[]>>(json, _jsonOptions);
            if (answer.Detail != null || answer.Errors != null)
            {
                throw new TwitterException(answer);
            }
            if (answer.Data == null)
            {
                answer.Data = Array.Empty<T>();
                return answer;
            }
            InternalIncludesParse(answer);
            return answer;
        }

        private Answer<T> ParseData<T>(string json)
        {
            var answer = JsonSerializer.Deserialize<Answer<T>>(json, _jsonOptions);
            if (answer.Detail != null || answer.Errors != null)
            {
                throw new TwitterException(answer);
            }
            InternalIncludesParse(answer);
            return answer;
        }

        private void BuildRateLimit(HttpResponseHeaders headers, Endpoint endpoint)
        {
            if (headers == null)
            {
                return;
            }

            var rateLimit = new RateLimit(endpoint);
            
            if (headers.TryGetValues("x-rate-limit-limit", out var limit))
            {
                rateLimit.Limit = Convert.ToInt32(limit.FirstOrDefault());
            }

            if (headers.TryGetValues("x-rate-limit-remaining", out var remaining))
            {
                rateLimit.Remaining = Convert.ToInt32(remaining.FirstOrDefault());
            }

            if (headers.TryGetValues("x-rate-limit-reset", out var reset))
            {
                rateLimit.Reset = Convert.ToInt32(reset.FirstOrDefault());
            }

            RateLimitChanged?.Invoke(this, rateLimit);
        }

        #endregion AdvancedParsing

        #region TweetSearch
		
        /// <summary>
        /// Get a tweet given its ID
        /// </summary>
        /// <param name="id">ID of the tweet</param>
        public async Task<Tweet> GetTweetAsync(string id, TweetSearchOptions options = null)
        {
            options ??= new();
            var query = _baseUrl + "tweets/" + HttpUtility.UrlEncode(id) + "?" + options.Build(true);

            var res = await _httpClient.GetAsync(query);
            BuildRateLimit(res.Headers, Endpoint.GetTweetById);
            return ParseData<Tweet>(await res.Content.ReadAsStringAsync()).Data;
        }

        /// <summary>
        /// Get a list of tweet given their IDs
        /// </summary>
        /// <param name="ids">All the IDs you want the tweets of</param>
        public async Task<RList<Tweet>> GetTweetsAsync(string[] ids, TweetSearchOptions options = null)
        {
            options ??= new();
            var query = _baseUrl + "tweets?ids=" + string.Join(",", ids.Select(x => HttpUtility.UrlEncode(x))) + "&" + options.Build(true);

            var res = await _httpClient.GetAsync(query);
            BuildRateLimit(res.Headers, Endpoint.GetTweetsByIds);
            var data = ParseArrayData<Tweet>(await res.Content.ReadAsStringAsync());
            var meta = data.Meta;
            return new()
            {
                Data = data.Data,
                NextAsync = data.Meta == null ? null : (data.Meta.NextToken == null ? null : async () => await NextAsync<Tweet>(query, data.Meta.NextToken, Endpoint.UserTweetTimeline))
            };
        }

        /// <summary>
        /// Get the latest tweets of an user
        /// </summary>
        /// <param name="userId">Username of the user you want the tweets of</param>
        public async Task<RList<Tweet>> GetTweetsFromUserIdAsync(string userId, TweetSearchOptions options = null)
        {
            options ??= new(); 
            var query = _baseUrl + "users/" + HttpUtility.HtmlEncode(userId) + "/tweets?" + options.Build(true);

            var res = await _httpClient.GetAsync(query);
            BuildRateLimit(res.Headers, Endpoint.UserTweetTimeline);
            var data = ParseArrayData<Tweet>(await res.Content.ReadAsStringAsync());
            return new()
            {
                Data = data.Data,
                NextAsync = data.Meta == null ? null : (data.Meta.NextToken == null ? null : async () => await NextAsync<Tweet>(query, data.Meta.NextToken, Endpoint.UserTweetTimeline))
            };
        }


        /// <summary>
        /// Get the latest tweets for an expression
        /// </summary>
        /// <param name="expression">An expression to build the query <seealso cref="https://developer.twitter.com/en/docs/twitter-api/tweets/search/integrate/build-a-query"/></param>
        /// <param name="options">properties send with the tweet</param>
        public async Task<RList<Tweet>> GetRecentTweets(Expression expression, TweetSearchOptions options = null)
        {
            options ??= new();
            var query = _baseUrl + "tweets/search/recent?query=" + HttpUtility.UrlEncode(expression.ToString()) + "&" + options.Build(true);

            var res = await _httpClient.GetAsync(query);
            BuildRateLimit(res.Headers, Endpoint.RecentSearch);
            var data = ParseArrayData<Tweet>(await res.Content.ReadAsStringAsync());
            return new()
            {
                Data = data.Data,
                NextAsync = data.Meta == null ? null : (data.Meta.NextToken == null ? null : async () => await NextAsync<Tweet>(query, data.Meta.NextToken, Endpoint.UserTweetTimeline))
            };
        }
        
        /// <summary>
        /// This endpoint is only available to those users who have been approved for <seealso cref="https://developer.twitter.com/en/docs/twitter-api/getting-started/about-twitter-api#v2-access-level">Academic Research access</seealso>.
        /// The full-archive search endpoint returns the complete history of public Tweets matching a search query; since the first Tweet was created March 26, 2006.
        /// </summary>
        /// <param name="expression">An expression to build the query <seealso cref="https://developer.twitter.com/en/docs/twitter-api/tweets/search/integrate/build-a-query"/></param>
        /// <param name="options">properties send with the tweet</param>
        public async Task<RList<Tweet>> GetAllTweets(Expression expression, TweetSearchOptions options = null)
        {
            options ??= new();
            var query = _baseUrl + "tweets/search/all?query=" + HttpUtility.UrlEncode(expression.ToString()) + "&" + options.Build(true);

            var res = await _httpClient.GetAsync(query);
            BuildRateLimit(res.Headers, Endpoint.FullArchiveSearch);
            var data = ParseArrayData<Tweet>(await res.Content.ReadAsStringAsync());
            return new()
            {
                Data = data.Data,
                NextAsync = data.Meta.NextToken == null ? null : async () => await NextAsync<Tweet>(query, data.Meta.NextToken, Endpoint.UserTweetTimeline)
            };
        }

        #endregion TweetSearch

        #region TweetStream

        public async Task<StreamInfo[]> GetInfoTweetStreamAsync()
        {
            var res = await _httpClient.GetAsync(_baseUrl + "tweets/search/stream/rules");
            BuildRateLimit(res.Headers, Endpoint.ListingFilters);
            return ParseArrayData<StreamInfo>(await res.Content.ReadAsStringAsync()).Data;
        }

        private StreamReader _reader;
        private static readonly object _streamLock = new();
        public static bool IsTweetStreaming { get; private set;}

        /// <summary>
        /// The stream is only meant to be open one time. So calling this method multiple time will result in an exception.
        /// For changing the rules with <see cref="AddTweetStreamAsync"/> and <see cref="DeleteTweetStreamAsync"/>
        /// "No disconnection needed to add/remove rules using rules endpoint."
        /// It has to be canceled with <see cref="CancelTweetStream"/>
        /// </summary>
        /// <param name="onNextTweet">The action which is called when a tweet arrives</param>
        /// <param name="tweetOptions">Properties send with the tweet</param>
        /// <param name="options">User properties send with the tweet</param>
        /// <param name="mediaOptions">Media properties send with the tweet</param>
        /// <returns></returns>
        public async Task NextTweetStreamAsync(Action<Tweet> onNextTweet, TweetSearchOptions options = null)
        {
            options ??= new();

            lock (_streamLock)
            { 
                if (IsTweetStreaming)
                {
                    throw new TwitterException("Stream already running. Please cancel the stream with CancelTweetStream", "TooManyConnections", "https://api.twitter.com/2/problems/streaming-connection");
                }
                
                IsTweetStreaming = true;
            }

            _tweetStreamCancellationTokenSource = new();
            var res = await _httpClient.GetAsync(_baseUrl + "tweets/search/stream?" + options.Build(true), HttpCompletionOption.ResponseHeadersRead, _tweetStreamCancellationTokenSource.Token);
            BuildRateLimit(res.Headers, Endpoint.ConnectingFilteresStream); 
            _reader = new(await res.Content.ReadAsStreamAsync(_tweetStreamCancellationTokenSource.Token));

            try
            {
                while (!_reader.EndOfStream && !_tweetStreamCancellationTokenSource.IsCancellationRequested)
                {
                    var str = await _reader.ReadLineAsync();
                    // Keep-alive signals: At least every 20 seconds, the stream will send a keep-alive signal, or heartbeat in the form of an \r\n carriage return through the open connection to prevent your client from timing out. Your client application should be tolerant of the \r\n characters in the stream.
                    if (string.IsNullOrWhiteSpace(str))
                    {
                        continue;
                    }
                    onNextTweet(ParseData<Tweet>(str).Data);
                }
            }
            catch (IOException e)
            {
                if(!(e.InnerException is SocketException se && se.SocketErrorCode == SocketError.ConnectionAborted))
                {
                    throw;
                }
            }

            CancelTweetStream();
        }

        /// <summary>
        /// Closes the tweet stream started by <see cref="NextTweetStreamAsync"/>. 
        /// </summary>
        /// <param name="force">If true, the stream will be closed immediately. With falls the thread had to wait for the next keep-alive signal (every 20 seconds)</param>
        public void CancelTweetStream(bool force = true)
        {
            _tweetStreamCancellationTokenSource?.Dispose();
            
            if (force)
            {
                _reader?.Close();
                _reader?.Dispose();
            }

            IsTweetStreaming = false;
        }

        /// <summary>
        /// Adds rules for the tweets/search/stream endpoint, which could be subscribed with the <see cref="NextTweetStreamAsync"/> method.
        /// No disconnection needed to add/remove rules using rules endpoint.
        /// </summary>
        /// <param name="request">The rules to be added</param>
        /// <returns>The added rule</returns>
        public async Task<StreamInfo[]> AddTweetStreamAsync(params StreamRequest[] request)
        {
            var content = new StringContent(JsonSerializer.Serialize(new StreamRequestAdd { Add = request }, _jsonOptions), Encoding.UTF8, "application/json");
            var res = await _httpClient.PostAsync(_baseUrl + "tweets/search/stream/rules", content);
            BuildRateLimit(res.Headers, Endpoint.AddingDeletingFilters);
            return ParseArrayData<StreamInfo>(await res.Content.ReadAsStringAsync()).Data;
        }

        /// <summary>
        /// Removes a rule for the tweets/search/stream endpoint, which could be subscribed with the <see cref="NextTweetStreamAsync"/> method.
        /// No disconnection needed to add/remove rules using rules endpoint.
        /// </summary>
        /// <param name="ids">Id of the rules to be removed</param>
        /// <returns>The number of deleted rules</returns>
        public async Task<int> DeleteTweetStreamAsync(params string[] ids)
        {
            var content = new StringContent(JsonSerializer.Serialize(new StreamRequestDelete { Delete = new StreamRequestDeleteIds { Ids = ids } }, _jsonOptions), Encoding.UTF8, "application/json");
            var res = await _httpClient.PostAsync(_baseUrl + "tweets/search/stream/rules", content);
            BuildRateLimit(res.Headers, Endpoint.AddingDeletingFilters);
            return ParseData<object>(await res.Content.ReadAsStringAsync()).Meta.Summary.Deleted;
        }
        #endregion TweetStream

        #region UserSearch
        /// <summary>
        /// Get an user given his username
        /// </summary>
        /// <param name="username">Username of the user you want information about</param>
        public async Task<User> GetUserAsync(string username, UserSearchOptions options = null)
        {
            options ??= new();
            var res = await _httpClient.GetAsync(_baseUrl + "users/by/username/" + HttpUtility.UrlEncode(username) + "?" + options.Build(false));
            BuildRateLimit(res.Headers, Endpoint.GetUserByName);
            return ParseData<User>(await res.Content.ReadAsStringAsync()).Data;
        }

        /// <summary>
        /// Get a list of users given their usernames
        /// </summary>
        /// <param name="usernames">Usernames of the users you want information about</param>
        public async Task<User[]> GetUsersAsync(string[] usernames, UserSearchOptions options = null)
        {
            options ??= new();
            var res = await _httpClient.GetAsync(_baseUrl + $"users/by?usernames={string.Join(",", usernames.Select(x => HttpUtility.UrlEncode(x)))}&{options.Build(false)}");
            BuildRateLimit(res.Headers, Endpoint.GetUsersByNames);
            return ParseArrayData<User>(await res.Content.ReadAsStringAsync()).Data;
        }

        /// <summary>
        /// Get an user given his ID
        /// </summary>
        /// <param name="id">ID of the user you want information about</param>
        public async Task<User> GetUserByIdAsync(string id, UserSearchOptions options = null)
        {
            options ??= new();
            var res = await _httpClient.GetAsync(_baseUrl + $"users/{HttpUtility.UrlEncode(id)}?{options.Build(false)}");
            BuildRateLimit(res.Headers, Endpoint.GetUserById);
            return ParseData<User>(await res.Content.ReadAsStringAsync()).Data;
        }

        /// <summary>
        /// Get a list of user given their IDs
        /// </summary>
        /// <param name="ids">IDs of the user you want information about</param>
        public async Task<User[]> GetUsersByIdsAsync(string[] ids, UserSearchOptions options = null)
        {
            options ??= new();
            var res = await _httpClient.GetAsync(_baseUrl + $"users?ids={string.Join(",", ids.Select(x => HttpUtility.UrlEncode(x)))}&{options.Build(false)}");
            BuildRateLimit(res.Headers, Endpoint.GetUsersByIds);
            return ParseArrayData<User>(await res.Content.ReadAsStringAsync()).Data;
        }

        #endregion UserSearch

        #region GetUsers


        /// <summary>
        /// Get the follower of an user
        /// </summary>
        /// <param name="id">ID of the user</param>
        /// <param name="limit">Max number of result, max is 1000</param>
        public async Task<RList<User>> GetFollowersAsync(string id, UserSearchOptions options = null)
        {
            options ??= new();
            var query = _baseUrl + $"users/{HttpUtility.UrlEncode(id)}/followers?{options.Build(false)}";
            var res = await _httpClient.GetAsync(query);
            var data = ParseData<User[]>(await res.Content.ReadAsStringAsync());
            BuildRateLimit(res.Headers, Endpoint.GetFollowersById);
            return new()
            {
                Data = data.Data,
                NextAsync = data.Meta.NextToken == null ? null : async () => await NextAsync<User>(query, data.Meta.NextToken, Endpoint.GetFollowersById)
            };
        }

        /// <summary>
        /// Get the following of an user
        /// </summary>
        /// <param name="id">ID of the user</param>
        /// <param name="limit">Max number of result, max is 1000</param>
        public async Task<RList<User>> GetFollowingAsync(string id, UserSearchOptions options = null)
        {
            options ??= new();
            var query = _baseUrl + $"users/{HttpUtility.UrlEncode(id)}/following?{options.Build(false)}";
            var res = await _httpClient.GetAsync(query);
            var data = ParseData<User[]>(await res.Content.ReadAsStringAsync());
            BuildRateLimit(res.Headers, Endpoint.GetFollowingsById);
            return new()
            {
                Data = data.Data,
                NextAsync = data.Meta.NextToken == null ? null : async () => await NextAsync<User>(query, data.Meta.NextToken, Endpoint.GetFollowingsById)
            };
        }
        
        /// <summary>
        /// Get the likes of a tweet
        /// </summary>
        /// <param name="id">ID of the tweet</param>
        /// <param name="options">This parameter enables you to select which specific user fields will deliver with each returned users objects. You can also set a Limit per page. Max is 100</param>
        public async Task<RList<User>> GetLikesAsync(string id, UserSearchOptions options = null)
        {
            options ??= new();
            var query = _baseUrl + $"tweets/{HttpUtility.UrlEncode(id)}/liking_users?{options.Build(false)}";
            var res = await _httpClient.GetAsync(query);
            var data = ParseData<User[]>(await res.Content.ReadAsStringAsync());
            BuildRateLimit(res.Headers, Endpoint.UsersLiked);
            return new()
            {
                Data = data.Data,
                NextAsync = data.Meta.NextToken == null ? null : async () => await NextAsync<User>(query, data.Meta.NextToken, Endpoint.UsersLiked)
            };
        }

        /// <summary>
        /// Get the retweets of a tweet
        /// </summary>
        /// <param name="id">ID of the tweet</param>
        /// <param name="options">This parameter enables you to select which specific user fields will deliver with each returned users objects. You can also set a Limit per page. Max is 100</param>
        public async Task<RList<User>> GetRetweetsAsync(string id, UserSearchOptions options = null)
        {
            options ??= new();
            var query = _baseUrl + $"tweets/{HttpUtility.UrlEncode(id)}/retweeted_by?{options.Build(false)}";
            var res = await _httpClient.GetAsync(query);
            var data = ParseData<User[]>(await res.Content.ReadAsStringAsync());
            BuildRateLimit(res.Headers, Endpoint.RetweetsLookup);
            return new()
            {
                Data = data.Data,
                NextAsync = data.Meta.NextToken == null ? null : async () => await NextAsync<User>(query, data.Meta.NextToken, Endpoint.RetweetsLookup)
            };
        }

        #endregion Users


        #region General


        /// <summary>
        /// General method for getting the next page with meta token
        /// </summary>
        /// <returns></returns>
        private async Task<RList<T>> NextAsync<T>(string baseQuery, string token, Endpoint endpoint)
        {
            var res = await _httpClient.GetAsync(baseQuery + (!baseQuery.EndsWith("?") ? "&" : "") + "pagination_token=" + token);
            var data = ParseArrayData<T>(await res.Content.ReadAsStringAsync());
            BuildRateLimit(res.Headers, endpoint);
            return new()
            {
                Data = data.Data,
                NextAsync = data.Meta.NextToken == null ? null : async () => await NextAsync<T>(baseQuery, data.Meta.NextToken, endpoint)
            };
        }

        #endregion
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
