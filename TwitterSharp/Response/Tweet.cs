using System;
using System.Text.Json.Serialization;
using TwitterSharp.Model;

namespace TwitterSharp.Response
{
    public class Tweet : IEquatable<Tweet>, IHaveAuthor
    {
        public string Id { init; get; }
        public string Text { init; get; }
        [JsonPropertyName("author_id")]
        public string AuthorId { init; get; }

        [JsonIgnore]
        public User Author { internal set; get; }

        // Interface

        [JsonIgnore]
        User IHaveAuthor.Author { set => Author = value; }

        [JsonIgnore]
        string IHaveAuthor.AuthorId => AuthorId;

        // Comparison

        public override bool Equals(object obj)
            => obj is Tweet t && t?.Id == Id;

        public bool Equals(Tweet other)
            => other?.Id == Id;

        public override int GetHashCode()
            => Id.GetHashCode();

        public static bool operator ==(Tweet left, Tweet right)
            => left?.Id == right?.Id;

        public static bool operator !=(Tweet left, Tweet right)
            => left?.Id != right?.Id;
    }
}
