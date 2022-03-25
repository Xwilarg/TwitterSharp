using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using TwitterSharp.ApiEndpoint;
using TwitterSharp.Model;
using TwitterSharp.Response;

namespace TwitterSharp.Client
{
  public partial class TwitterClient
  {
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
    private static readonly Type _matchingRulesInterface = typeof(IHaveMatchingRules);
    private static void InternalIncludesParse<T>(Answer<T> answer)
    {
      if (answer.Includes != null)
      {
        if (answer.Includes.Users != null && answer.Includes.Users.Any() && _authorInterface.IsAssignableFrom(typeof(T)))
        {
          var data = answer.Data;
          IncludesParseUser((IHaveAuthor)data, answer.Includes);
          answer.Data = data;
        }
        if (answer.Includes.Media != null && answer.Includes.Media.Any() && _mediaInterface.IsAssignableFrom(typeof(T)))
        {
          var data = answer.Data;
          IncludesParseMedias((IHaveMedia)data, answer.Includes);
          answer.Data = data;
        }
        if (answer.MatchingRules != null && answer.MatchingRules.Any() && _matchingRulesInterface.IsAssignableFrom(typeof(T)))
        {
          (answer.Data as IHaveMatchingRules).MatchingRules = answer.MatchingRules;
        }
      }
    }
    private static void InternalIncludesParse<T>(Answer<T[]> answer)
    {
      if (answer.Includes != null)
      {
        if (answer.Includes.Users != null && answer.Includes.Users.Any() && answer.Includes.Users.Any() && _authorInterface.IsAssignableFrom(typeof(T)))
        {
          var data = answer.Data;
          IncludesParseUser(data.Cast<IHaveAuthor>().ToArray(), answer.Includes);
          answer.Data = data;
        }
        if (answer.Includes.Media != null && answer.Includes.Media.Any() && _mediaInterface.IsAssignableFrom(typeof(T)))
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

    private void BuildRateLimit(HttpResponseHeaders headers, Endpoint endpoint)
    {
      if (headers == null)
      {
        return;
      }

      var rateLimit = new RateLimit(endpoint);

      if (headers.TryGetValues("x-rate-limit-limit", out var limit))
      {
        rateLimit.Limit = Convert.ToInt32(limit.FirstOrDefault());
      }

      if (headers.TryGetValues("x-rate-limit-remaining", out var remaining))
      {
        rateLimit.Remaining = Convert.ToInt32(remaining.FirstOrDefault());
      }

      if (headers.TryGetValues("x-rate-limit-reset", out var reset))
      {
        rateLimit.Reset = Convert.ToInt32(reset.FirstOrDefault());
      }

      RateLimitChanged?.Invoke(this, rateLimit);
    }
  }
}
