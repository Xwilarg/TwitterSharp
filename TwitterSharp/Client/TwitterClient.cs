using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using TwitterSharp.CustomConverter;
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
                PropertyNameCaseInsensitive = true,
            };
            _jsonOptions.Converters.Add(new ExpressionConverter());
        }

        private T[] ParseArrayData<T>(string json)
        {
            var answer = JsonSerializer.Deserialize<Answer<T[]>>(json, _jsonOptions);
            if (answer.Detail != null)
            {
                throw new TwitterException(answer.Detail);
            }
            return answer.Data ?? Array.Empty<T>();
        }

        private Answer<T, U> ParseDataWithIncludes<T, U>(string json)
        {
            var answer = JsonSerializer.Deserialize<Answer<T, U>>(json, _jsonOptions);
            if (answer.Detail != null)
            {
                throw new TwitterException(answer.Detail);
            }
            return answer;
        }

        private Answer<T> ParseData<T>(string json)
        {
            var answer = JsonSerializer.Deserialize<Answer<T>>(json, _jsonOptions);
            if (answer.Detail != null)
            {
                throw new TwitterException(answer.Detail);
            }
            return answer;
        }

        #region TweetSearch
        public async Task<Tweet[]> GetTweetsByIdsAsync(params string[] ids)
        {
            var str = await _httpClient.GetStringAsync(_baseUrl + "tweets?ids=" + string.Join(",", ids.Select(x => HttpUtility.HtmlEncode(x))));
            return ParseArrayData<Tweet>(str);
        }

        public async Task<Tweet[]> GetTweetsFromUserIdAsync(string userId)
        {
            var str = await _httpClient.GetStringAsync(_baseUrl + "users/" + HttpUtility.HtmlEncode(userId) + "/tweets");
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
        {
            var stream = await _httpClient.GetStreamAsync(_baseUrl + "tweets/search/stream");
            using StreamReader reader = new(stream);
            while (!reader.EndOfStream)
            {
                var str = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(str))
                    continue;
                var result = ParseDataWithIncludes<TweetInternal, UserContainer>(str);
                onNextTweet(new Tweet
                {
                    Id = result.Data.Id,
                    Text = result.Data.Text,
                    Author = result.Includes.Users[0]
                });
            }
        }

        public async Task<StreamInfo[]> AddTweetStreamAsync(params StreamRequest[] request)
            => await AddTweetStreamAsync(request, null);

        public async Task<StreamInfo[]> AddTweetStreamAsync(StreamRequest[] request, UserOption[] options)
        {
            var content = new StringContent(JsonSerializer.Serialize(new StreamRequestAdd { Add = request }), Encoding.UTF8, "application/json");
            var str = await (await _httpClient.PostAsync(_baseUrl + "tweets/search/stream/rules"
                    + (options == null ? "" : "?expansions=author_id&user.fields=" + string.Join(",", options.Select(x => x.ToString().ToLowerInvariant())))
                , content)).Content.ReadAsStringAsync();
            return ParseArrayData<StreamInfo>(str);
        }

        public async Task<int> DeleteTweetStreamAsync(params string[] ids)
        {
            var content = new StringContent(JsonSerializer.Serialize(new StreamRequestDelete { Delete = new StreamRequestDeleteIds { Ids = ids } }), Encoding.UTF8, "application/json");
            var str = await (await _httpClient.PostAsync(_baseUrl + "tweets/search/stream/rules", content)).Content.ReadAsStringAsync();
            return ParseData<object>(str).Meta.Summary.Deleted;
        }
        #endregion TweetStream

        #region UserSearch
        public async Task<User[]> GetUsersAsync(params string[] usernames)
            => await GetUsersAsync(usernames, null);

        public async Task<User[]> GetUsersAsync(string[] usernames, UserOption[] options)
        {
            var str = await _httpClient.GetStringAsync(
                _baseUrl + "users/by?usernames=" + string.Join(",", usernames.Select(x => HttpUtility.HtmlEncode(x)))
                    + (options == null ? "" : "&user.fields=" + string.Join(",", options.Select(x => x.ToString().ToLowerInvariant())))
                );
            return ParseArrayData<User>(str);
        }
        #endregion UserSearch

        private const string _baseUrl = "https://api.twitter.com/2/";

        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
    }
}
