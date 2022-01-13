using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using TwitterSharp.JsonOption;
using TwitterSharp.Model;
using TwitterSharp.Request;
using TwitterSharp.Request.AdvancedSearch;
using TwitterSharp.Request.Internal;
using TwitterSharp.Response.RStream;
using TwitterSharp.Response.RTweet;
using TwitterSharp.Response.RUser;

namespace TwitterSharp.Client
{
    /// <summary>
    /// Base client to do all your requests
    /// </summary>
    public class TwitterClient
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
        private static void InternalIncludesParse<T>(Answer<T> answer)
        {
            if (answer.Includes != null)
            {
                if (answer.Includes.Users != null && answer.Includes.Users.Length > 0 && _authorInterface.IsAssignableFrom(typeof(T)))
                {
                    var data = answer.Data;
                    IncludesParseUser((IHaveAuthor)data, answer.Includes);
                    answer.Data = data;
                }
                if (answer.Includes.Media != null && answer.Includes.Media.Length > 0 && _mediaInterface.IsAssignableFrom(typeof(T)))
                {
                    var data = answer.Data;
                    IncludesParseMedias((IHaveMedia)data, answer.Includes);
                    answer.Data = data;
                }
            }
        }
        private static void InternalIncludesParse<T>(Answer<T[]> answer)
        {
            if (answer.Includes != null)
            {
                if (answer.Includes.Users != null && answer.Includes.Users.Length > 0 && answer.Includes.Users.Length > 0 && _authorInterface.IsAssignableFrom(typeof(T)))
                {
                    var data = answer.Data;
                    IncludesParseUser(data.Cast<IHaveAuthor>().ToArray(), answer.Includes);
                    answer.Data = data;
                }
                if (answer.Includes.Media != null && answer.Includes.Media.Length > 0 && _mediaInterface.IsAssignableFrom(typeof(T)))
                {
                    var data = answer.Data;
                    IncludesParseMedias(data.Cast<IHaveMedia>().ToArray(), answer.Includes);
                    answer.Data = data;
                }
            }
        }

        private T[] ParseArrayData<T>(string json)
        {
            var answer = JsonSerializer.Deserialize<Answer<T[]>>(json, _jsonOptions);
            if (answer.Detail != null)
            {
                throw new TwitterException(answer.Detail);
            }
            if (answer.Data == null)
            {
                return Array.Empty<T>();
            }
            InternalIncludesParse(answer);
            return answer.Data;
        }

        private Answer<T> ParseData<T>(string json)
        {
            var answer = JsonSerializer.Deserialize<Answer<T>>(json, _jsonOptions);
            if (answer.Detail != null)
            {
                throw new TwitterException(answer.Detail);
            }
            InternalIncludesParse(answer);
            return answer;
        }
        #endregion AdvancedParsing

        #region AddOptions

        private static void AddMediaOptions(RequestOptions request, MediaOption[] options)
        {
            if (options != null)
            {
                request.AddOptions("media.fields", options.Select(x => x.ToString().ToLowerInvariant()));
            }
        }

        /// <param name="needExpansion">False is we are requesting a tweet, else true</param>
        private static void AddUserOptions(RequestOptions request, UserOption[] options, bool needExpansion)
        {
            if (options != null)
            {
                if (needExpansion)
                {
                    request.AddOption("expansions", "author_id");
                }
                request.AddOptions("user.fields", options.Select(x => x.ToString().ToLowerInvariant()));
            }
        }

        private static void AddTweetOptions(RequestOptions request, TweetOption[] options)
        {

            if (options != null)
            {
                if (options.Contains(TweetOption.Attachments))
                {
                    request.AddOption("expansions", "attachments.media_keys");
                }
                request.AddOptions("tweet.fields", options.Select(x =>
                {
                    if (x == TweetOption.Attachments_Ids)
                    {
                        return "attachments";
                    }
                    return x.ToString().ToLowerInvariant();
                }));
            }
        }
        #endregion AddOptions

        #region TweetSearch
        /// <summary>
        /// Get a tweet given its ID
        /// </summary>
        /// <param name="id">ID of the tweet</param>
        public async Task<Tweet> GetTweetAsync(string id, TweetOption[] tweetOptions = null, UserOption[] userOptions = null, MediaOption[] mediaOptions = null)
        {
            var req = new RequestOptions();
            AddTweetOptions(req, tweetOptions);
            AddUserOptions(req, userOptions, true);
            AddMediaOptions(req, mediaOptions);
            var str = await _httpClient.GetStringAsync(_baseUrl + "tweets/" + HttpUtility.UrlEncode(id) + "?" + req.Build());
            return ParseData<Tweet>(str).Data;
        }

        /// <summary>
        /// Get a list of tweet given their IDs
        /// </summary>
        /// <param name="ids">All the IDs you want the tweets of</param>
        public async Task<Tweet[]> GetTweetsAsync(string[] ids, TweetOption[] tweetOptions = null, UserOption[] userOptions = null, MediaOption[] mediaOptions = null)
        {
            var req = new RequestOptions();
            AddTweetOptions(req, tweetOptions);
            AddUserOptions(req, userOptions, true);
            AddMediaOptions(req, mediaOptions);
            var str = await _httpClient.GetStringAsync(_baseUrl + "tweets?ids=" + string.Join(",", ids.Select(x => HttpUtility.UrlEncode(x))) + "&" + req.Build());
            return ParseArrayData<Tweet>(str);
        }

        /// <summary>
        /// Get the latest tweets of an user
        /// </summary>
        /// <param name="userId">Username of the user you want the tweets of</param>
        public async Task<Tweet[]> GetTweetsFromUserIdAsync(string userId, TweetOption[] tweetOptions = null, UserOption[] userOptions = null, MediaOption[] mediaOptions = null)
        {
            var req = new RequestOptions();
            AddTweetOptions(req, tweetOptions);
            AddUserOptions(req, userOptions, true);
            AddMediaOptions(req, mediaOptions);
            var str = await _httpClient.GetStringAsync(_baseUrl + "users/" + HttpUtility.HtmlEncode(userId) + "/tweets?" + req.Build());
            return ParseArrayData<Tweet>(str);
        }
        #endregion TweetSearch

        #region TweetStream
        public async Task<StreamInfo[]> GetInfoTweetStreamAsync()
        {
            var str = await _httpClient.GetStringAsync(_baseUrl + "tweets/search/stream/rules");
            return ParseArrayData<StreamInfo>(str);
        }

        public async Task NextTweetStreamAsync(Action<Tweet> onNextTweet, TweetOption[] tweetOptions = null, UserOption[] options = null, MediaOption[] mediaOptions = null)
        {
            var req = new RequestOptions();
            AddTweetOptions(req, tweetOptions);
            AddUserOptions(req, options, true);
            AddMediaOptions(req, mediaOptions);
            var stream = await _httpClient.GetStreamAsync(_baseUrl + "tweets/search/stream?" + req.Build());
            using StreamReader reader = new(stream);
            while (!reader.EndOfStream)
            {
                var str = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(str))
                {
                    continue;
                }
                onNextTweet(ParseData<Tweet>(str).Data);
            }
        }

        public async Task<StreamInfo[]> AddTweetStreamAsync(params StreamRequest[] request)
        {
            var content = new StringContent(JsonSerializer.Serialize(new StreamRequestAdd { Add = request }, _jsonOptions), Encoding.UTF8, "application/json");
            var str = await (await _httpClient.PostAsync(_baseUrl + "tweets/search/stream/rules", content)).Content.ReadAsStringAsync();
            return ParseArrayData<StreamInfo>(str);
        }

        public async Task<int> DeleteTweetStreamAsync(params string[] ids)
        {
            var content = new StringContent(JsonSerializer.Serialize(new StreamRequestDelete { Delete = new StreamRequestDeleteIds { Ids = ids } }, _jsonOptions), Encoding.UTF8, "application/json");
            var str = await (await _httpClient.PostAsync(_baseUrl + "tweets/search/stream/rules", content)).Content.ReadAsStringAsync();
            return ParseData<object>(str).Meta.Summary.Deleted;
        }
        #endregion TweetStream

        #region UserSearch
        /// <summary>
        /// Get an user given his username
        /// </summary>
        /// <param name="username">Username of the user you want information about</param>
        public async Task<User> GetUserAsync(string username, UserOption[] options = null)
        {
            var req = new RequestOptions();
            AddUserOptions(req, options, false);
            var str = await _httpClient.GetStringAsync(_baseUrl + "users/by/username/" + HttpUtility.UrlEncode(username) + "?" + req.Build());
            return ParseData<User>(str).Data;
        }

        /// <summary>
        /// Get a list of users given their usernames
        /// </summary>
        /// <param name="usernames">Usernames of the users you want information about</param>
        public async Task<User[]> GetUsersAsync(string[] usernames, UserOption[] options = null)
        {
            var req = new RequestOptions();
            AddUserOptions(req, options, false);
            var str = await _httpClient.GetStringAsync(_baseUrl + "users/by?usernames=" + string.Join(",", usernames.Select(x => HttpUtility.UrlEncode(x))) + "&" + req.Build());
            return ParseArrayData<User>(str);
        }

        /// <summary>
        /// Get an user given his ID
        /// </summary>
        /// <param name="id">ID of the user you want information about</param>
        public async Task<User> GetUserByIdAsync(string id, UserOption[] options = null)
        {
            var req = new RequestOptions();
            AddUserOptions(req, options, false);
            var str = await _httpClient.GetStringAsync(_baseUrl + "users/" + HttpUtility.UrlEncode(id) + "?" + req.Build());
            return ParseData<User>(str).Data;
        }

        /// <summary>
        /// Get a list of user given their IDs
        /// </summary>
        /// <param name="ids">IDs of the user you want information about</param>
        public async Task<User[]> GetUsersByIdsAsync(string[] ids, UserOption[] options = null)
        {
            var req = new RequestOptions();
            AddUserOptions(req, options, false);
            var str = await _httpClient.GetStringAsync(_baseUrl + "users?ids=" + string.Join(",", ids.Select(x => HttpUtility.UrlEncode(x))) + "&" + req.Build());
            return ParseArrayData<User>(str);
        }

        #endregion UserSearch

        #region Follows
        private async Task<Follow> NextFollowAsync(string baseQuery, string token)
        {
            var str = await _httpClient.GetStringAsync(baseQuery + (!baseQuery.EndsWith("?") ? "&" : "") + "pagination_token=" + token);
            var data = ParseData<User[]>(str);
            return new()
            {
                Users = data.Data,
                NextAsync = data.Meta.NextToken == null ? null : async () => await NextFollowAsync(baseQuery, data.Meta.NextToken)
            };
        }

        /// <summary>
        /// Get the follower of an user
        /// </summary>
        /// <param name="id">ID of the user</param>
        /// <param name="limit">Max number of result, max is 1000</param>
        public async Task<Follow> GetFollowersAsync(string id, int limit = 100, UserOption[] options = null)
        {
            var req = new RequestOptions();
            AddUserOptions(req, options, false);
            var query = _baseUrl + "users/" + HttpUtility.UrlEncode(id) + "/followers?max_results=" + limit + "&" + req.Build();
            var str = await _httpClient.GetStringAsync(query);
            var data = ParseData<User[]>(str);
            return new()
            {
                Users = data.Data,
                NextAsync = data.Meta.NextToken == null ? null : async () => await NextFollowAsync(query, data.Meta.NextToken)
            };
        }

        /// <summary>
        /// Get the following of an user
        /// </summary>
        /// <param name="id">ID of the user</param>
        /// <param name="limit">Max number of result, max is 1000</param>
        public async Task<Follow> GetFollowingAsync(string id, int limit = 100, UserOption[] options = null)
        {
            var req = new RequestOptions();
            AddUserOptions(req, options, false);
            var query = _baseUrl + "users/" + HttpUtility.UrlEncode(id) + "/following?max_results=" + limit + "&" + req.Build();
            var str = await _httpClient.GetStringAsync(query);
            var data = ParseData<User[]>(str);
            return new()
            {
                Users = data.Data,
                NextAsync = data.Meta.NextToken == null ? null : async () => await NextFollowAsync(query, data.Meta.NextToken)
            };
        }
        #endregion Follows

        private const string _baseUrl = "https://api.twitter.com/2/";

        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
    }
}
