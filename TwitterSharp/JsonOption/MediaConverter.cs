using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TwitterSharp.Response.RMedia;

namespace TwitterSharp.JsonOption
{
    public class MediaConverter : JsonConverter<Media>
    {
        public override Media Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                return new Media
                {
                    Key = reader.GetString()
                };
            }
            catch (InvalidOperationException)
            {
                var elem = JsonSerializer.Deserialize<JsonElement>(ref reader, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = new SnakeCaseNamingPolicy()
                });
                var media = new Media
                {
                    Key = elem.GetProperty("media_key").GetString(),
                    Type = elem.GetProperty("type").GetString() switch
                    {
                        "video" => MediaType.Video,
                        "animated_gif" => MediaType.AnimatedGif,
                        "photo" => MediaType.Photo,
                        _ => throw new InvalidOperationException("Invalid type"),
                    }
                };
                return media;
            }
        }

        public override void Write(Utf8JsonWriter writer, Media value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
