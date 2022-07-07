using System.Threading.Tasks;
using System.Web;
using TwitterSharp.ApiEndpoint;
using TwitterSharp.Request.Option;
using TwitterSharp.Response.RUser;

namespace TwitterSharp.Client
{
    public partial class TwitterClient
    {
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
    }
}
