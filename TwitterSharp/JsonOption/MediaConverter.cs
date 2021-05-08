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
            } catch (InvalidOperationException)
            {
                var o = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = new SnakeCaseNamingPolicy()
                };
                var media = JsonSerializer.Deserialize<Media>(ref reader, o);
                var elem = JsonSerializer.Deserialize<JsonElement>(ref reader, o);
                media.Key = elem.GetProperty("media_key").GetString();
                media.Type = elem.GetProperty("type").GetString() switch
                {
                    "video" => MediaType.Video,
                    "animated_gif" => MediaType.AnimatedGif,
                    "photo" => MediaType.Photo,
                    _ => throw new InvalidOperationException("Invalid type"),
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
