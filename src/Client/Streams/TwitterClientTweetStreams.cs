using System;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TwitterSharp.ApiEndpoint;
using TwitterSharp.Request;
using TwitterSharp.Request.Internal;
using TwitterSharp.Request.Option;
using TwitterSharp.Response.RStream;
using TwitterSharp.Response.RTweet;
using System.Net.Sockets;

namespace TwitterSharp.Client
{
  public partial class TwitterClient
  {
    private StreamReader _reader;
    private static readonly object _streamLock = new();
    public static bool IsTweetStreaming { get; private set; }

    /// <summary>
    /// The stream is only meant to be open one time. So calling this method multiple time will result in an exception.
    /// For changing the rules with <see cref="AddTweetStreamAsync"/> and <see cref="DeleteTweetStreamAsync"/>
    /// "No disconnection needed to add/remove rules using rules endpoint."
    /// It has to be canceled with <see cref="CancelTweetStream"/>
    /// </summary>
    /// <param name="onNextTweet">The action which is called when a tweet arrives</param>
    /// <param name="tweetOptions">Properties send with the tweet</param>
    /// <param name="options">User properties send with the tweet</param>
    /// <param name="mediaOptions">Media properties send with the tweet</param>
    /// <returns></returns>
    protected async Task GetTweetStreamAsync(
      Action<TwitterSharp.Response.RTweet.Tweet> onNextTweet,
      CancellationToken cancellationToken,
      TweetSearchOptions options
    )
    {
      try
      {
        while (!_reader.EndOfStream && !cancellationToken.IsCancellationRequested)
        {
          var str = await _reader.ReadLineAsync();
          // Keep-alive signals: At least every 20 seconds, the stream will send a keep-alive signal, or heartbeat in the form of an \r\n carriage return through the open connection to prevent your client from timing out. Your client application should be tolerant of the \r\n characters in the stream.
          if (string.IsNullOrWhiteSpace(str))
          {
            continue;
          }
          onNextTweet(ParseData<Tweet>(str).Data);
        }
      }
      catch (IOException e)
      {
        if (!(e.InnerException is SocketException se && se.SocketErrorCode == SocketError.ConnectionAborted))
        {
          throw;
        }
      }
      CancelTweetStream();
    }
    public async Task NextTweetStreamAsync(
      Action<Tweet> onNextTweet,
      CancellationToken cancelationToken = default,
      TweetSearchOptions options = null)
    {
      options ??= new();

      lock (_streamLock)
      {
        if (IsTweetStreaming)
        {
          throw new TwitterException("Stream already running. Please cancel the stream with CancelNextTweetStreamAsync");
        }

        IsTweetStreaming = true;
      }
      _tweetStreamCancellationTokenSource = new();
      CancellationToken cancellationToken = default(CancellationToken) == cancelationToken ? cancelationToken : _tweetStreamCancellationTokenSource.Token;
      var res = await _httpClient.GetAsync(_baseUrl + "tweets/search/stream?" + options.Build(true), HttpCompletionOption.ResponseHeadersRead, cancelationToken);
      BuildRateLimit(res.Headers, Endpoint.ConnectingFiltersStream);
      _reader = new(await res.Content.ReadAsStreamAsync(cancelationToken));
      await GetTweetStreamAsync(onNextTweet, cancelationToken, options);
    }

    /// <summary>
    /// Closes the tweet stream started by <see cref="NextTweetStreamAsync"/>. 
    /// </summary>
    /// <param name="force">If true, the stream will be closed immediately. With falls the thread had to wait for the next keep-alive signal (every 20 seconds)</param>        
    public void CancelTweetStream(bool force = true)
    {
      _tweetStreamCancellationTokenSource?.Dispose();

      if (force)
      {
        _reader?.Close();
        _reader?.Dispose();
      }
      IsTweetStreaming = false;
    }

    public async Task<StreamInfo[]> GetInfoTweetStreamAsync()
    {
      var res = await _httpClient.GetAsync(_baseUrl + "tweets/search/stream/rules");
      BuildRateLimit(res.Headers, Endpoint.ListingFilters);
      return ParseArrayData<StreamInfo>(await res.Content.ReadAsStringAsync());
    }

    /// <summary>
    /// Adds rules for the tweets/search/stream endpoint, which could be subscribed with the <see cref="NextTweetStreamAsync"/> method.
    /// No disconnection needed to add/remove rules using rules endpoint.
    /// </summary>
    /// <param name="request">The rules to be added</param>
    /// <returns>The added rule</returns>
    public async Task<StreamInfo[]> AddTweetStreamAsync(params StreamRequest[] request)
    {
      var content = new StringContent(JsonSerializer.Serialize(new StreamRequestAdd { Add = request }, _jsonOptions), Encoding.UTF8, "application/json");
      var res = await _httpClient.PostAsync(_baseUrl + "tweets/search/stream/rules", content);
      BuildRateLimit(res.Headers, Endpoint.AddingDeletingFilters);
      return ParseArrayData<StreamInfo>(await res.Content.ReadAsStringAsync());
    }

    /// <summary>
    /// Removes a rule for the tweets/search/stream endpoint, which could be subscribed with the <see cref="NextTweetStreamAsync"/> method.
    /// No disconnection needed to add/remove rules using rules endpoint.
    /// </summary>
    /// <param name="ids">Id of the rules to be removed</param>
    /// <returns>The number of deleted rules</returns>
    public async Task<int> DeleteTweetStreamAsync(params string[] ids)
    {
      var content = new StringContent(JsonSerializer.Serialize(new StreamRequestDelete { Delete = new StreamRequestDeleteIds { Ids = ids } }, _jsonOptions), Encoding.UTF8, "application/json");
      var res = await _httpClient.PostAsync(_baseUrl + "tweets/search/stream/rules", content);
      BuildRateLimit(res.Headers, Endpoint.AddingDeletingFilters);
      return ParseData<object>(await res.Content.ReadAsStringAsync()).Meta.Summary.Deleted;
    }
  }
}
