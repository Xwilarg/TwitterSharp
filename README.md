# TwitterSharp
C# wrapper around Twitter API V2

| CI | Code Quality | Coverage |
| -- | ------------ | -------- |
| [![.NET](https://github.com/Xwilarg/TwitterSharp/actions/workflows/ci.yml/badge.svg)](https://github.com/Xwilarg/TwitterSharp/actions/workflows/ci.yml) | [![Codacy Badge](https://app.codacy.com/project/badge/Grade/726fd5c6287644d48807fcf03a18d868)](https://www.codacy.com/gh/Xwilarg/TwitterSharp/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Xwilarg/TwitterSharp&amp;utm_campaign=Badge_Grade) | [![Codacy Badge](https://app.codacy.com/project/badge/Coverage/726fd5c6287644d48807fcf03a18d868)](https://www.codacy.com/gh/Xwilarg/TwitterSharp/dashboard?utm_source=github.com&utm_medium=referral&utm_content=Xwilarg/TwitterSharp&utm_campaign=Badge_Coverage) |

## Download

The package is available on [NuGet](https://www.nuget.org/packages/TwitterSharp/)
```powershell
Install-Package TwitterSharp
```

## Documentation
https://twittersharp.zirk.eu

## How does it works?

To begin with, please go to the [Twitter Developer Portal](https://developer.twitter.com/) and create a new application\
Then you must instantiate a new client:
```cs
var client = new TwitterSharp.Client.TwitterClient(bearerToken);
```
From there you can access various methods to access tweets and users, however please make note that a basic request only includes:
 - For tweets: its ID and content
 - For users: its ID, name and username

To solve that, most function take an array of UserOption or TweetOption, make sure to add what you need there!

Need more help? You can use the examples below, if you're still lost feel free to open an issue or a discussion!

## Examples
### Get a tweet from its ID
```cs
var client = new TwitterSharp.Client.TwitterClient(bearerToken);
var answer = await client.GetTweetAsync("1389189291582967809");
Console.WriteLine(answer.Text); // たのしみ！！\uD83D\uDC93 https://t.co/DgBYVYr9lN
```

### Get an user from its username
```cs
var client = new TwitterSharp.Client.TwitterClient(bearerToken);
var answer = await client.GetUserAsync("theindra5");
Console.WriteLine(answer.Id); // 1022468464513089536
```

### Get latest tweets from an user id with the attached medias
```cs
var client = new TwitterSharp.Client.TwitterClient(bearerToken);
// You can get the id using GetUsersAsync
var answer = await client.GetTweetsFromUserIdAsync("1109748792721432577", new TweetOption[] { TweetOption.Attachments }, null, new MediaOption[] { MediaOption.Url });
for (int i = 0; i < answer.Length; i++)
{
    var tweet = answer[i];
    Console.WriteLine($"Tweet n°{i}");
    Console.WriteLine(tweet.Text);
    if (tweet.Attachments?.Media?.Any() ?? false)
    {
        Console.WriteLine("\nImages:");
        Console.WriteLine(string.Join("\n", tweet.Attachments.Media.Select(x => x.Url)));
    }
    Console.WriteLine("\n");
}
```

### Get the users that someone follow
```cs
var client = new TwitterClient(Environment.GetEnvironmentVariable("TWITTER_TOKEN"));

var answer = await client.GetFollowingAsync("1433657158067896325", 1000);
Console.WriteLine(string.Join("\n", answer.Users.Select(u => u.Username)));
while (answer.NextAsync != null) // We go to the next page if there is one
{
    answer = await answer.NextAsync();
    Console.WriteLine(string.Join("\n", answer.Users.Select(u => u.Username)));
}
```

### Continuously get all the new tweets from some users
```cs
var client = new TwitterSharp.Client.TwitterClient(bearerToken);

// Subscribe to 5 Twitter accounts
var request = new TwitterSharp.Request.StreamRequest(
    Expression.Author("moricalliope") // using TwitterSharp.Rule;
        .Or(
            Expression.Author("takanashikiara"),
            Expression.Author("ninomaeinanis"),
            Expression.Author("gawrgura"),
            Expression.Author("watsonameliaEN")
        )
, "Anime");
await client.AddTweetStreamAsync(request); // Add them to the stream

// We display all the subscriptions we have
var subs = await client.GetInfoTweetStreamAsync();
Console.WriteLine("Subscriptions: " + string.Join("\n", subs.Select(x => x.Value.ToString())));

// NextTweetStreamAsync will continue to run in background
Task.Run(async () =>
{
    // Take in parameter a callback called for each new tweet
    // Since we want to get the basic info of the tweet author, we add an empty array of UserOption
    await client.NextTweetStreamAsync((tweet) =>
    {
        Console.WriteLine($"From {tweet.Author.Name}: {tweet.Text} (Rules: {string.Join(',', tweet.MatchingRules.Select(x => x.Tag))})");
    }, null, Array.Empty<UserOption>());
});

// Add new high frequent rule after the stream started. No disconnection needed.
await client.AddTweetStreamAsync(new TwitterSharp.Request.StreamRequest( Expression.Author("Every3Minutes"), "Frequent"));
```

## Contributing

If you want to contribute feel free to open a pull request\
You can also see how the project is going in the [project tab](https://github.com/Xwilarg/TwitterSharp/projects/1)
