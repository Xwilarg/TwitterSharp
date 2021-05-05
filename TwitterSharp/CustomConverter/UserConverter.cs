using System;
using System.Buffers;
using System.Text.Json;
using System.Text.Json.Serialization;
using TwitterSharp.Response;

namespace TwitterSharp.CustomConverter
{
    public class UserConverter : JsonConverter<User>
    {
        public override User Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var json = JsonSerializer.Deserialize<JsonElement>(ref reader, options);
            var elem = json.GetProperty("includes")[0];

            // https://stackoverflow.com/a/59047063/6663248
            var bufferWriter = new ArrayBufferWriter<byte>();
            using var writer = new Utf8JsonWriter(bufferWriter);
            elem.WriteTo(writer);
            return JsonSerializer.Deserialize<User>(bufferWriter.WrittenSpan, options);
        }

        public override void Write(Utf8JsonWriter writer, User value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value);
        }
    }
}
