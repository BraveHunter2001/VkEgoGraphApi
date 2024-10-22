using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace VkEgoGraphApi.VkAuth;

public class VkAuthenticationOptions : OAuthOptions
{
    public string? CodeVerifier { get; set; }
    public string? CodeChallengeMethod { get; set; }

    public VkAuthenticationOptions()
    {
        Events.OnRemoteFailure = (RemoteFailureContext context) =>
        {
            context.HandleResponse();
            return Task.CompletedTask;
        };

        Events.OnCreatingTicket = async (OAuthCreatingTicketContext context) => { 
            context.Identity.AddClaim(new("access_token", context.AccessToken));
            context.Properties.RedirectUri = $"/";
        };
    }
}