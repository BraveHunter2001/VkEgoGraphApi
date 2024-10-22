using VkEgoGraphApi.VkAuth;

namespace VkEgoGraphApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var vkConfig = builder.Configuration.GetSection("VkConfig").Get<VkConfig>()!;
            builder.Services.AddControllers();

            builder.Services
                .AddAuthentication("auth-cookie")
                .AddCookie("auth-cookie", o =>
                {
                    o.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                })
                .AddVk("vk-oauth", "vk", o =>
                {
                    o.SignInScheme = "auth-cookie";
                    o.ClientSecret = vkConfig.ClientId;
                    o.ClientId = vkConfig.ClientId;
                    o.CodeVerifier = vkConfig.CodeVerifier;
                    o.CodeChallengeMethod = vkConfig.CodeChallengeMethod;

                    o.AuthorizationEndpoint = "https://id.vk.com/authorize";
                    o.TokenEndpoint = "https://id.vk.com/oauth2/auth";
                    o.UserInformationEndpoint = "https://id.vk.com/oauth2/user_info";

                    o.Scope.Add("vkid.personal_info");
                    o.Scope.Add("friends");

                    o.CallbackPath = "/api/auth/cb";
                    o.UsePkce = true;
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}