using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Authenticators.OAuth2;
using System.Globalization;
using VkEgoGraphApi.Models;
using VkEgoGraphApi.VkModels;

namespace VkEgoGraphApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GraphController : ControllerBase
{
    private static readonly User _me = new User() { Id = 153332386, Name = "Илья-Мышкин" };
    const int DEPTH = 2;
    const int COUNT = 0;

    private readonly CacheService _cacheService = new CacheService();

    [HttpGet("equal")]
    public IActionResult EqualLink()
    {
        Link link1 = new Link() { FirstId = 1, SecondId = 2 };
        Link link2 = new Link() { FirstId = 2, SecondId = 1 };
        List<Link> links = [link1, link2];

        var uniqLinks = links.Distinct().ToList();

        return Ok(new
        {
            Hash1 = link1.GetHashCode(), Hash2 = link2.GetHashCode(),
            HasEqual = link1.Equals(link2), UniqLinks = uniqLinks
        });
    }

    [HttpGet("friends")]
    public IActionResult Friends()
    {
        int index = 0;
        FillFriends(_me, ref index);

        List<User> users = new List<User>();
        List<Link> links = new List<Link>();

        ProcessedUserRoot(_me, ref users, ref links);

        List<GephyNode> nodes = users.ConvertAll(u => u.ToNode());
        using (var writer = new StreamWriter("Nodes_mini.csv"))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(nodes);
        }

        List<GephyEdge> edges = links.ConvertAll(u => u.ToEdge());
        using (var writer = new StreamWriter("Edges_mini.csv"))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(edges);
        }

        return Ok(new { NodeCount = nodes.Count, EdgeCount = edges.Count });
    }

    private void ProcessedUserRoot(User root, ref List<User> uniqUsers, ref List<Link> uniqLinks)
    {
        List<User> users = new List<User>();

        Processed(root);

        List<Link> links = users
            .Where(u => u.Links is not null)
            .SelectMany(u => u.Links)
            .ToList();

        uniqLinks = links.Distinct().ToList();

        uniqUsers = users.DistinctBy(x => x.Id).ToList();

        void Processed(User root)
        {
            users.Add(root);
            if (root.Friends is null) return;
            foreach (var friend in root.Friends)
            {
                Processed(friend);
            }
        }
    }

    private void FillFriends(User root, ref int currentDepth)
    {
        if (currentDepth >= DEPTH) return;

        root.Friends = GetFriends(root.Id);
        if (root.Friends is null) return;

        foreach (var f in root.Friends)
        {
            currentDepth += 1;
            FillFriends(f, ref currentDepth);
            currentDepth -= 1;
        }
    }

    private List<User> GetFriends(int id)
    {
        var cachedRes = _cacheService.Get<List<User>>(id.ToString());
        if (cachedRes != null)
            return cachedRes;

        var token = User.FindFirst("access_token")?.Value;

        var authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(
            token, "Bearer"
        );

        var options = new RestClientOptions("https://api.vk.com/method/")
        {
            Authenticator = authenticator
        };

        var client = new RestClient(options);

        var request = new RestRequest("friends.get");
        request.AddParameter("user_id", id);
        request.AddParameter("order", "hints");
        request.AddParameter("fields", "nickname");

        if (COUNT > 0)
            request.AddParameter("count", COUNT);

        request.AddParameter("v", "5.199");

        // The cancellation token comes from the caller. You can still make a call without it.
        var friends = client.ExecuteGet<VkFriendsResponse>(request);
        if (!friends.IsSuccessful) return null;

        var res = friends.Data?.Response?.Items.ConvertAll(vkf => vkf.ToFriend());
        _cacheService.Set(id.ToString(), res);
        return res;
    }
}