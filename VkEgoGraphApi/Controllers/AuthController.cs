using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VkEgoGraphApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpGet("vk")]
    public ChallengeResult VkAuth() => Challenge("vk-oauth");

    [HttpGet("Token")]
    [Authorize]
    public IActionResult Token()
    {
        string token = User.FindFirst("access_token")?.Value;

        return Ok(token);
    }
}
