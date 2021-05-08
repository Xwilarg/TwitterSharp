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
            return new Media
            {
                Key = reader.GetString()
            };
        }

        public override void Write(Utf8JsonWriter writer, Media value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
