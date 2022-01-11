using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TwitterSharp.Response.Entity;

namespace TwitterSharp.JsonOption
{
    internal class EntitiesConverter : JsonConverter<Entities>
    {
        public override Entities Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var json = JsonSerializer.Deserialize<JsonElement>(ref reader, options);
            JsonElement elem;
            if (json.TryGetProperty("description", out elem))
            { }
            else
            {
                elem = json;
            }
            var entities = JsonSerializer.Deserialize<Entities>(elem.ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            entities.Urls ??= Array.Empty<EntityUrl>();
            entities.Hashtags ??= Array.Empty<EntityTag>();
            entities.Cashtags ??= Array.Empty<EntityTag>();
            entities.Mentions ??= Array.Empty<EntityTag>();
            return entities;
        }

        public override void Write(Utf8JsonWriter writer, Entities value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
