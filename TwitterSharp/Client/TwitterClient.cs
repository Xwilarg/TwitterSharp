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
using TwitterSharp.Response;

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
        }

        #region AdvancedParsing
        private static void IncludesParseUser(IHaveAuthor data, Includes includes)
        {
            data.Author = includes.Users.FirstOrDefault();
        }

        private static void IncludesParseUser(IHaveAuthor[] data, Includes includes)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i].Author = includes.Users.Where(x => x.Id == data[i].AuthorId).FirstOrDefault();
            }
        }

        private static readonly Type _authorInterface = typeof(IHaveAuthor);
        private static void InternalIncludesParse<T>(Answer<T> answer)
        {
            if (answer.Includes != null && _authorInterface.IsAssignableFrom(typeof(T)))
            {
                var data = answer.Data;
                IncludesParseUser((IHaveAuthor)data, answer.Includes);
                answer.Data = data;
            }
        }
        private static void InternalIncludesParse<T>(Answer<T[]> answer)
        {
            if (answer.Includes != null && _authorInterface.IsAssignableFrom(typeof(T)))
            {
                var data = answer.Data;
                IncludesParseUser(data.Cast<IHaveAuthor>().ToArray(), answer.Includes);
                answer.Data = data;
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
        /// <summary>
        /// Add the user options to an url
        /// </summary>
        /// <param name="url">Url to modify (will thenbe returned)</param>
        /// <param name="options">Options that need to be added</param>
        /// <param name="needExpansion">False is we are requesting a tweet, else true</param>
        /// <param name="isFirstLink">True if is the first URL parameter (meaning we must put a ? instead of a &)</param>
        private static string AddUserOptions(string url, UserOption[] options, bool needExpansion, bool isFirstLink)
        {
            if (options == null)
            {
                return url;
            }
            if (needExpansion)
            {
                url += isFirstLink ? "?" : "&";
                url += "expansions=author_id";
                isFirstLink = false;
            }
            if (options.Length > 0)
            {
                url += isFirstLink ? "?" : "&";
                url += "user.fields=" + string.Join(",", options.Select(x => x.ToString().ToLowerInvariant()));
            }
            return url;
        }
        #endregion AddOptions

        #region TweetSearch
        public async Task<Tweet[]> GetTweetsByIdsAsync(params string[] ids)
            => await GetTweetsByIdsAsync(ids, null);

        public async Task<Tweet[]> GetTweetsByIdsAsync(string[] ids, UserOption[] options)
        {
            var url = _baseUrl + "tweets?ids=" + string.Join(",", ids.Select(x => HttpUtility.HtmlEncode(x)));
            url = AddUserOptions(url, options, true, false);
            var str = await _httpClient.GetStringAsync(url);
            return ParseArrayData<Tweet>(str);
        }

        public async Task<Tweet[]> GetTweetsFromUserIdAsync(string userId)
            => await GetTweetsFromUserIdAsync(userId, null);

        public async Task<Tweet[]> GetTweetsFromUserIdAsync(string userId, UserOption[] options)
        {
            var url = _baseUrl + "users/" + HttpUtility.HtmlEncode(userId) + "/tweets";
            url = AddUserOptions(url, options, true, true);
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
            => await NextTweetStreamAsync(onNextTweet, null);

        public async Task NextTweetStreamAsync(Action<Tweet> onNextTweet, UserOption[] options)
        {
            var url = _baseUrl + "tweets/search/stream";
            url = AddUserOptions(url, options, true, true);
            var stream = await _httpClient.GetStreamAsync(url);
            using StreamReader reader = new(stream);
            while (!reader.EndOfStream)
            {
                var str = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(str))
                    continue;
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
            url = AddUserOptions(url, options, false, false);
            var str = await _httpClient.GetStringAsync(url);
            return ParseArrayData<User>(str);
        }
        #endregion UserSearch

        private const string _baseUrl = "https://api.twitter.com/2/";

        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
    }
}
