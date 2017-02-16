using System.Security.Claims;
using System.Threading.Tasks;
using AspNetIdentity.Dal;
using AspNetIdentity.Domain.IdentityEntity;
using AspNetIdentity.WebApi.IdentityConfig;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace AspNetIdentity.WebApi.Providers
{
    public class OAuthProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = "*";
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var userManager = new UserManager(new UserStore(new AspNetIdentityDbContext()));
            User user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            if (!user.EmailConfirmed)
            {
                context.SetError("invalid_grant", "The user email is not confirmed.");
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, "JWT");
            oAuthIdentity.AddClaim(SuperAdminRoleProvider.ValidateSuperAdmin(user));
            var ticket = new AuthenticationTicket(oAuthIdentity, null);

            context.Validated(ticket);
        }
    }
}