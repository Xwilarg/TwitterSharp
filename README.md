# TwitterSharp
C# wrapper around Twitter API V2

| CI | Code Quality | Coverage |
| -- | ------------ | -------- |
| [![.NET](https://github.com/Xwilarg/TwitterSharp/actions/workflows/ci.yml/badge.svg)](https://github.com/Xwilarg/TwitterSharp/actions/workflows/ci.yml) | [![Codacy Badge](https://app.codacy.com/project/badge/Grade/726fd5c6287644d48807fcf03a18d868)](https://www.codacy.com/gh/Xwilarg/TwitterSharp/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Xwilarg/TwitterSharp&amp;utm_campaign=Badge_Grade) | [![Codacy Badge](https://app.codacy.com/project/badge/Coverage/726fd5c6287644d48807fcf03a18d868)](https://www.codacy.com/gh/Xwilarg/TwitterSharp/dashboard?utm_source=github.com&utm_medium=referral&utm_content=Xwilarg/TwitterSharp&utm_campaign=Badge_Coverage) |

## Examples (WIP)

To begin with, please go to the [Twitter Developer Portail](https://developer.twitter.com/) and create a new application

### Get a tweet from its ID
```cs
var client = new TwitterSharp.Client.TwitterClient(bearerToken);
var answer = await client.GetTweetsByIdsAsync("1389189291582967809");
Console.WriteLine(answer[0].Text); // たのしみ！！\uD83D\uDC93 https://t.co/DgBYVYr9lN
```

### Get a tweet from its username
```cs
var client = new TwitterSharp.Client.TwitterClient(bearerToken);
var answer = await client.GetUsersAsync("theindra5"); // 1022468464513089536
Console.WriteLine(answer[0].Id); // TheIndra
```

### Get a tweet from an user id
```cs
var client = new TwitterSharp.Client.TwitterClient(bearerToken);
var answer = await client.GetTweetsFromUserIdAsync("1109748792721432577"); // You can get the id using GetUsersAsync
for (int i = 0; i < answer.Length; i++)
{
    Console.WriteLine($"Tweet n°{i}:\n{answer[i].Text}\n\n");
}
```
