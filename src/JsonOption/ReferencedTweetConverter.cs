using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TwitterSharp.Response.RTweet;

namespace TwitterSharp.JsonOption
{
    internal class ReferencedTweetConverter : JsonConverter<ReferencedTweet>
    {
        public override ReferencedTweet Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var json = JsonSerializer.Deserialize<JsonElement>(ref reader, options);
            var t = new ReferencedTweet();
            switch (json.GetProperty("type").GetString())
            {
                case "replied_to":
                    t.Type = ReferenceType.RepliedTo;
                    break;

                case "quoted":
                    t.Type = ReferenceType.Quoted;
                    break;

                case "retweeted":
                    t.Type = ReferenceType.Retweeted;
                    break;

                default:
                    throw new InvalidOperationException("Invalid type " + json.GetProperty("type").GetString());
            }
            t.Id = json.GetProperty("id").GetString();
            return t;
        }

        public override void Write(Utf8JsonWriter writer, ReferencedTweet value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
