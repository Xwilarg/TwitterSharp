using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TwitterSharp.Rule;

namespace TwitterSharp.JsonOption
{
    internal class ExpressionConverter : JsonConverter<Expression>
    {
        public override Expression Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Nach Expression parsen
            return new(reader.GetString(), "", ExpressionType.None);
        }

        public override void Write(Utf8JsonWriter writer, Expression value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
