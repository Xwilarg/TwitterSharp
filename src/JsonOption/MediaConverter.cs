using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TwitterSharp.Response.RMedia;

namespace TwitterSharp.JsonOption
{
    internal class MediaConverter : JsonConverter<Media>
    {
        private JsonElement? TryGetProperty(JsonElement json, string key)
        {
            if (json.TryGetProperty(key, out JsonElement elem))
            {
                return elem;
            }
            return null;
        }

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
                    },
                    Height = TryGetProperty(elem, "height")?.GetInt32(),
                    Width = TryGetProperty(elem, "width")?.GetInt32(),
                    DurationMs = TryGetProperty(elem, "duration_ms")?.GetInt32(),
                    PreviewImageUrl = TryGetProperty(elem, "preview_image_url")?.GetString(),
                    Url = TryGetProperty(elem, "url")?.GetString()
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
