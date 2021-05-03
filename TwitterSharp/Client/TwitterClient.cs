using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using TwitterSharp.Response;

namespace TwitterSharp.Client
{
    public class TwitterClient
    {
        public TwitterClient(string bearerToken)
        {
            _httpClient = new();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        }

        #region TweetSearch
        public async Task<Tweet[]> GetTweetsByIdsAsync(params string[] ids)
        {
            var str = await _httpClient.GetStringAsync(_baseUrl + "tweets?ids=" + string.Join(",", ids.Select(x => HttpUtility.HtmlEncode(x))));
            return JsonSerializer.Deserialize<Answer<Tweet[]>>(str, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).Data;
        }

        public async Task<Tweet[]> GetTweetsFromUserIdAsync(string userId)
        {
            var str = await _httpClient.GetStringAsync(_baseUrl + "users/" + HttpUtility.HtmlEncode(userId) + "/tweets");
            return JsonSerializer.Deserialize<Answer<Tweet[]>>(str, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).Data;
        }
        #endregion TweetSearch

        #region TweetStream
        public async Task<StreamInfo[]> GetTweetStreamInfoAsync()
        {
            var str = await _httpClient.GetStringAsync(_baseUrl + "tweets/search/stream/rules");
            return JsonSerializer.Deserialize<Answer<StreamInfo[]>>(str, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).Data;
        }
        #endregion TweetStream

        #region UserSearch
        public async Task<User[]> GetUsersAsync(params string[] usernames)
        {
            var str = await _httpClient.GetStringAsync(_baseUrl + "users/by?usernames=" + string.Join(",", usernames.Select(x => HttpUtility.HtmlEncode(x))));
            return JsonSerializer.Deserialize<Answer<User[]>>(str, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).Data;
        }
        #endregion UserSearch

        private const string _baseUrl = "https://api.twitter.com/2/";

        private readonly HttpClient _httpClient;
    }
}
