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

            var error = new Error();

            if (!json.TryGetProperty("details", out JsonElement elem))
            {
                // elem = json;
                if (elem.ValueKind == JsonValueKind.Array)
                {
                    foreach (var el in elem.EnumerateArray())
                    {
                        // error.Details.el.ToString()
                    }

                }
                // error.Details = elem.
            }

            return new Error();
        }

        public override void Write(Utf8JsonWriter writer, Error value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
