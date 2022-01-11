using System.Collections.Generic;
using System.Linq;

namespace TwitterSharp.Client
{
    internal class RequestOptions
    {
        internal void AddOption(string key, string value)
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

        internal void AddOptions(string key, IEnumerable<string> values)
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

        internal string Build()
        {
            return string.Join("&", _options.Select(x => x.Key + "=" + string.Join(",", x.Value)));
        }

        private readonly Dictionary<string, List<string>> _options = new();
    }
}
