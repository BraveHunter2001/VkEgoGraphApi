namespace VkEgoGraphApi.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<User> Friends { get; set; }

    public List<Link> Links => Friends?.ConvertAll(friend => new Link() { FirstId = Id, SecondId = friend.Id });

    public GephyNode ToNode() => new GephyNode() { Id = Id, Label = Name };
}