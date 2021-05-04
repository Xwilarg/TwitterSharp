# TwitterSharp
C# wrapper around Twitter API V2

| CI | Code Quality | Coverage |
| -- | ------------ | -------- |
| [![.NET](https://github.com/Xwilarg/TwitterSharp/actions/workflows/ci.yml/badge.svg)](https://github.com/Xwilarg/TwitterSharp/actions/workflows/ci.yml) | [![Codacy Badge](https://app.codacy.com/project/badge/Grade/726fd5c6287644d48807fcf03a18d868)](https://www.codacy.com/gh/Xwilarg/TwitterSharp/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Xwilarg/TwitterSharp&amp;utm_campaign=Badge_Grade) | [![Codacy Badge](https://app.codacy.com/project/badge/Coverage/726fd5c6287644d48807fcf03a18d868)](https://www.codacy.com/gh/Xwilarg/TwitterSharp/dashboard?utm_source=github.com&utm_medium=referral&utm_content=Xwilarg/TwitterSharp&utm_campaign=Badge_Coverage) |

## Examples (WIP)

To begin with, please go to the [Twitter Developer Portal](https://developer.twitter.com/) and create a new application

### Get a tweet from its ID
```cs
var client = new TwitterSharp.Client.TwitterClient(bearerToken);
var answer = await client.GetTweetsByIdsAsync("1389189291582967809");
Console.WriteLine(answer[0].Text); // たのしみ！！\uD83D\uDC93 https://t.co/DgBYVYr9lN
```

### Get an user from its username
```cs
var client = new TwitterSharp.Client.TwitterClient(bearerToken);
var answer = await client.GetUsersAsync("theindra5");
Console.WriteLine(answer[0].Id); // 1022468464513089536
```

### Get latest tweets from an user id
```cs
var client = new TwitterSharp.Client.TwitterClient(bearerToken);
// You can get the id using GetUsersAsync
var answer = await client.GetTweetsFromUserIdAsync("1109748792721432577");
for (int i = 0; i < answer.Length; i++)
{
    Console.WriteLine($"Tweet n°{i}:\n{answer[i].Text}\n\n");
}
```

### Follow users and get all new tweets
```cs
var client = new TwitterSharp.Client.TwitterClient(bearerToken);

// Subscribe to 5 Twitter accounts
var request = new TwitterSharp.Request.StreamRequest(
    "moricalliope OR takanashikiara OR ninomaeinanis OR gawrgura OR watsonameliaEN"
);
await client.AddTweetStreamAsync(request); // Add them to the stream

// We display all the subscriptions we have
var subs = await client.GetInfoTweetStreamAsync();
Console.WriteLine("Subscriptions: " + string.Join("\n", subs.Select(x => x.Value)));

// NextTweetStreamAsync will continue to run in background
Task.Run(async () =>
{
    // Take in parameter a callback called for each new tweet
    await client.NextTweetStreamAsync((tweet) =>
    {
        Console.WriteLine(tweet.Text);
    });
});
```