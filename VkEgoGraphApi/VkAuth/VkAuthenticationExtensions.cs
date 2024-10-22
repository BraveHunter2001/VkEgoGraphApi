using Microsoft.AspNetCore.Authentication;

namespace VkEgoGraphApi.VkAuth;

public static class VkAuthenticationExtensions
{
    public static AuthenticationBuilder AddVk(this AuthenticationBuilder builder, string scheme, string caption, Action<VkAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<VkAuthenticationOptions, VkAuthenticationHandler>(scheme, caption, configuration);
    }
}
