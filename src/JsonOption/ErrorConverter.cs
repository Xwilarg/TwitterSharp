using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TwitterSharp.Response;

namespace TwitterSharp.JsonOption
{
    internal class ErrorConverter : JsonConverter<Error>
    {
        public override Error Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var json = JsonSerializer.Deserialize<JsonElement>(ref reader, options);

            if (!json.TryGetProperty("details", out JsonElement elem))
            {
                elem = json;
            }

            return new Error();
        }

        public override void Write(Utf8JsonWriter writer, Error value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
