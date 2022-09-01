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
            try
            {
                // parse to expression
                return Expression.ToExpression(reader.GetString());
            }
            catch (Exception e)
            {
                return new(reader.GetString(), "");
            }
        }

        public override void Write(Utf8JsonWriter writer, Expression value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
