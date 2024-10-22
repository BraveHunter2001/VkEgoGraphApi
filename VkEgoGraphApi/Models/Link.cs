namespace VkEgoGraphApi.Models;

public class Link : IEquatable<Link>
{
    public int FirstId { get; set; }
    public int SecondId { get; set; }

    public bool IsLoop => FirstId == SecondId;

    public GephyEdge ToEdge() => new GephyEdge()
    {
        Source = FirstId,
        Target = SecondId,
    };

    public override int GetHashCode() => FirstId.GetHashCode() ^ SecondId.GetHashCode();

    public bool Equals(Link? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return FirstId == other.FirstId && SecondId == other.SecondId || FirstId == other.SecondId && SecondId == other.FirstId;
    }

    public override bool Equals(object? obj) => Equals(obj as Link);
}