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
    public class TwitterClient
    {
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
            for (int i = 0; i < medias.Length; i++)
            {
                medias[i] = includes.Media.Where(x => x.Key == medias[i].Key).FirstOrDefault();
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
        /// <param name="url">Url to modify (will thenbe returned)</param>
        /// <param name="options">Options that need to be added</param>
        /// <param name="isFirstLink">True if is the first URL parameter (meaning we must put a ? instead of a &)</param>
        private static bool AddOptions(ref string url, string[] options, bool isFirstLink, string expansionString, string requestString)
        {
            if (expansionString != null)
            {
                url += isFirstLink ? "?" : "&";
                url += expansionString;
                isFirstLink = false;
            }
            if (options.Length > 0)
            {
                url += isFirstLink ? "?" : "&";
                url += requestString + "=" + string.Join(",", options);
                isFirstLink = false;
            }
            return isFirstLink;
        }

        /// <param name="needExpansion">False is we are requesting a tweet, else true</param>
        private static bool AddUserOptions(ref string url, UserOption[] options, bool needExpansion, bool isFirstLink)
        {
            if (options == null)
            {
                return isFirstLink;
            }
            return AddOptions(ref url, options.Select(x => x.ToString().ToLowerInvariant()).ToArray(), isFirstLink, needExpansion ? "expansions=author_id" : null, "user.fields");
        }

        private static bool AddTweetOptions(ref string url, TweetOption[] options, bool isFirstLink)
        {
            if (options == null)
            {
                return isFirstLink;
            }
            return AddOptions(ref url, options.Select((x) =>
            {
                if (x == TweetOption.AttachmentsIds)
                {
                    return "attachments";
                }
                else
                {
                    return x.ToString().ToLowerInvariant();
                }
            }).ToArray(), isFirstLink,
                options.Contains(TweetOption.Attachments) ? "expansions=attachments.media_keys" : null, "tweet.fields");
        }
        #endregion AddOptions

        #region TweetSearch
        public async Task<Tweet> GetTweetAsync(string id)
            => await GetTweetAsync(id, null, null);

        public async Task<Tweet> GetTweetAsync(string id, TweetOption[] tweetOptions, UserOption[] userOptions)
        {
            var url = _baseUrl + "tweets/" + HttpUtility.UrlEncode(id);
            AddTweetOptions(ref url, tweetOptions, false);
            AddUserOptions(ref url, userOptions, true, false);
            var str = await _httpClient.GetStringAsync(url);
            return ParseData<Tweet>(str).Data;
        }

        public async Task<Tweet[]> GetTweetsAsync(params string[] ids)
            => await GetTweetsAsync(ids, null, null);

        public async Task<Tweet[]> GetTweetsAsync(string[] ids, TweetOption[] tweetOptions, UserOption[] userOptions)
        {
            var url = _baseUrl + "tweets?ids=" + string.Join(",", ids.Select(x => HttpUtility.HtmlEncode(x)));
            AddTweetOptions(ref url, tweetOptions, false);
            AddUserOptions(ref url, userOptions, true, false);
            var str = await _httpClient.GetStringAsync(url);
            return ParseArrayData<Tweet>(str);
        }

        public async Task<Tweet[]> GetTweetsFromUserIdAsync(string userId)
            => await GetTweetsFromUserIdAsync(userId, null, null);

        public async Task<Tweet[]> GetTweetsFromUserIdAsync(string userId, TweetOption[] tweetOptions, UserOption[] options)
        {
            var url = _baseUrl + "users/" + HttpUtility.HtmlEncode(userId) + "/tweets";
            var b = AddTweetOptions(ref url, tweetOptions, true);
            AddUserOptions(ref url, options, true, b);
            var str = await _httpClient.GetStringAsync(url);
            return ParseArrayData<Tweet>(str);
        }
        #endregion TweetSearch

        #region TweetStream
        public async Task<StreamInfo[]> GetInfoTweetStreamAsync()
        {
            var str = await _httpClient.GetStringAsync(_baseUrl + "tweets/search/stream/rules");
            return ParseArrayData<StreamInfo>(str);
        }

        public async Task NextTweetStreamAsync(Action<Tweet> onNextTweet)
            => await NextTweetStreamAsync(onNextTweet, null, null);

        public async Task NextTweetStreamAsync(Action<Tweet> onNextTweet, TweetOption[] tweetOptions, UserOption[] options)
        {
            var url = _baseUrl + "tweets/search/stream";
            var b = AddTweetOptions(ref url, tweetOptions, true);
            AddUserOptions(ref url, options, true, b);
            var stream = await _httpClient.GetStreamAsync(url);
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
        public async Task<User[]> GetUsersAsync(params string[] usernames)
            => await GetUsersAsync(usernames, null);

        public async Task<User[]> GetUsersAsync(string[] usernames, UserOption[] options)
        {
            var url = _baseUrl + "users/by?usernames=" + string.Join(",", usernames.Select(x => HttpUtility.HtmlEncode(x)));
            AddUserOptions(ref url, options, false, false);
            var str = await _httpClient.GetStringAsync(url);
            return ParseArrayData<User>(str);
        }
        #endregion UserSearch

        private const string _baseUrl = "https://api.twitter.com/2/";

        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
    }
}
