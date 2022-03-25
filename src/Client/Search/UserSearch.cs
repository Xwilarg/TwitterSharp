using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwitterSharp.ApiEndpoint;
using TwitterSharp.Request.Option;
using TwitterSharp.Response.RUser;

namespace TwitterSharp.Client
{
    public partial class TwitterClient
    {
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
            return ParseArrayData<User>(await res.Content.ReadAsStringAsync());
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
            return ParseArrayData<User>(await res.Content.ReadAsStringAsync());
        }
    }
}
