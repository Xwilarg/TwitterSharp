using System.Linq;
using System.Text;
using System.Text.Json;

namespace TwitterSharp.JsonOption
{
    internal class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
            StringBuilder str = new();
            str.Append(char.ToLower(name[0]));
            foreach (char c in name.Skip(1))
            {
                if (char.IsUpper(c))
                {
                    str.Append("_" + char.ToLower(c));
                }
                else
                {
                    str.Append(c);
                }
            }
            return str.ToString();
        }
    }
}
