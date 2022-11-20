using System.Collections.Generic;
using System.Linq;
using TwitterSharp.Request.AdvancedSearch;

namespace TwitterSharp.Request.Option
{
    public abstract class ASearchOptions
    {
        /// <summary>
        /// Max number of results returned by the API
        /// </summary>
        public int? Limit { set; get; }

        protected abstract void PreBuild(bool needExpansion);
        internal string Build(bool needExpansion)
        {
            PreBuild(needExpansion);
            var url = string.Join("&", _options.Select(x => x.Key + "=" + string.Join(",", x.Value)));
            if (Limit.HasValue)
            {
                url += $"&max_results={Limit}";
            }
            return url;
        }

        protected void AddMediaOptions(MediaOption[] options)
        {
            if (options != null)
            {
                AddOptions("media.fields", options.Select(x => x.ToString().ToLowerInvariant()));
            }
        }

        /// <param name="needExpansion">False is we are requesting a tweet, else true</param>
        protected void AddUserOptions(UserOption[] options, bool needExpansion)
        {
            if (options != null)
            {
                if (needExpansion)
                {
                    AddOption("expansions", "author_id");
                }
                AddOptions("user.fields", options.Select(x => x.ToString().ToLowerInvariant()));
            }
        }

        protected void AddTweetOptions(TweetOption[] options)
        {

            if (options != null)
            {
                if (options.Contains(TweetOption.Attachments))
                {
                    AddOption("expansions", "attachments.media_keys");
                }
                AddOptions("tweet.fields", options.Select(x =>
                {
                    if (x == TweetOption.Attachments_Ids)
                    {
                        return "attachments";
                    }
                    if (x == TweetOption.Author_Id)
                    {
                        return "author_id";
                    }
                    return x.ToString().ToLowerInvariant();
                }));
            }
        }

        private void AddOption(string key, string value)
        {
            if (!_options.ContainsKey(key))
            {
                _options.Add(key, new() { value });
            }
            else if (!_options[key].Contains(value))
            {
                _options[key].Add(value);
            }
        }

        private void AddOptions(string key, IEnumerable<string> values)
        {
            if (!_options.ContainsKey(key))
            {
                _options.Add(key, values.ToList());
            }
            else
            {
                _options[key].AddRange(values);
            }
            _options[key] = _options[key].Distinct().ToList();
        }

        protected readonly Dictionary<string, List<string>> _options = new();
    }
}
