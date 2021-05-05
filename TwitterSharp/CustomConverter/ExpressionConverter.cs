using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TwitterSharp.Rule;

namespace TwitterSharp.CustomConverter
{
    public class ExpressionConverter : JsonConverter<Expression>
    {
        public override Expression Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException("TODO");
        }

        public override void Write(Utf8JsonWriter writer, Expression value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
