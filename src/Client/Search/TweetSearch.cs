using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwitterSharp.Request.Option;
using TwitterSharp.Response.RTweet;

namespace TwitterSharp.Client
{
    public partial class TwitterClient
    {
        /// <summary>
        /// Get a tweet given its ID
        /// </summary>
        /// <param name="id">ID of the tweet</param>
        public async Task<Tweet> GetTweetAsync(string id, TweetSearchOptions options = null)
        {
            options ??= new();
            var str = await _httpClient.GetStringAsync(_baseUrl + "tweets/" + HttpUtility.UrlEncode(id) + "?" + options.Build(true));
            return ParseData<Tweet>(str).Data;
        }

        /// <summary>
        /// Get a list of tweet given their IDs
        /// </summary>
        /// <param name="ids">All the IDs you want the tweets of</param>
        public async Task<Tweet[]> GetTweetsAsync(string[] ids, TweetSearchOptions options = null)
        {
            options ??= new();
            var str = await _httpClient.GetStringAsync(_baseUrl + "tweets?ids=" + string.Join(",", ids.Select(x => HttpUtility.UrlEncode(x))) + "&" + options.Build(true));
            return ParseArrayData<Tweet>(str);
        }

        /// <summary>
        /// Get the latest tweets of an user
        /// </summary>
        /// <param name="userId">Username of the user you want the tweets of</param>
        public async Task<Tweet[]> GetTweetsFromUserIdAsync(string userId, TweetSearchOptions options = null)
        {
            options ??= new();
            var str = await _httpClient.GetStringAsync(_baseUrl + "users/" + HttpUtility.HtmlEncode(userId) + "/tweets?" + options.Build(true));
            return ParseArrayData<Tweet>(str);
        }
    }
}
