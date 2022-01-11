using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TwitterSharp.Response.RTweet;

namespace TwitterSharp.JsonOption
{
    public class ReplySettingsConverter : JsonConverter<ReplySettings>
    {
        public override ReplySettings Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.GetString())
            {
                case "everyone":
                    return ReplySettings.Everyone;

                case "mentionned_users":
                    return ReplySettings.MentionnedUsers;

                case "followers":
                    return ReplySettings.Followers;

                default:
                    throw new InvalidOperationException("Invalid type");
            }
        }

        public override void Write(Utf8JsonWriter writer, ReplySettings value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
