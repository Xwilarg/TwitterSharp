using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TwitterSharp.Response;

namespace TwitterSharp.JsonOption
{
    public class ReferencedTweetConverter : JsonConverter<ReferencedTweet>
    {
        public override ReferencedTweet Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var t = new ReferencedTweet();
            switch (reader.GetString())
            {
                case "replied_to":
                    t.Type = ReferenceType.RepliedTo;
                    break;

                case "quoted":
                    t.Type = ReferenceType.Quoted;
                    break;

                default:
                    throw new InvalidOperationException("Invalid type");
            }
            t.Id = reader.GetString();
            return t;
        }

        public override void Write(Utf8JsonWriter writer, ReferencedTweet value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
