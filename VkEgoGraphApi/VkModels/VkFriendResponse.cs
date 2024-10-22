using System.Text.Json.Serialization;
using VkEgoGraphApi.Models;

namespace VkEgoGraphApi.VkModels;

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class VkFriend
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nickname")]
    public string Nickname { get; set; }

    [JsonPropertyName("track_code")]
    public string TrackCode { get; set; }

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string LastName { get; set; }

    [JsonPropertyName("can_access_closed")]
    public bool CanAccessClosed { get; set; }

    [JsonPropertyName("is_closed")]
    public bool IsClosed { get; set; }

    public User ToFriend() => new() { Id = Id, Name = $"{FirstName}-{LastName}" };
}

public class VkFriendsContainer
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("items")]
    public List<VkFriend> Items { get; set; }
}

public class VkFriendsResponse
{
    [JsonPropertyName("response")]
    public VkFriendsContainer Response { get; set; }
}