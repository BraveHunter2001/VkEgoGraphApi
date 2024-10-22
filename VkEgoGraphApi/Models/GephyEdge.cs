namespace VkEgoGraphApi.Models;

public class GephyEdge
{
    public int Source { get; set; }
    public int Target { get; set; }
    public string Type => "Undirected";
    public int? Id = null;
    public string Label { get; set; }
    public int Weight => 1;
}