using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace VkEgoGraphApi.VkAuth;

public class VkOAuthCodeExchangeContext : OAuthCodeExchangeContext
{
    public string DeviceId { get; }

    /// <summary>
    /// Initializes a new <see cref="T:Microsoft.AspNetCore.Authentication.OAuth.OAuthCodeExchangeContext" />.
    /// </summary>
    /// <param name="properties">The <see cref="T:Microsoft.AspNetCore.Authentication.AuthenticationProperties" />.</param>
    /// <param name="code">The code returned from the authorization endpoint.</param>
    /// <param name="redirectUri">The redirect uri used in the authorization request.</param>
    public VkOAuthCodeExchangeContext(AuthenticationProperties properties, string code, string redirectUri, string deviceId) : base(properties, code, redirectUri)
    {
        DeviceId = deviceId;
    }
}