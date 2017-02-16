using System;
using System.Configuration;
using System.Web.Http;
using AspNetIdentity.WebApi.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using JwtFormat = AspNetIdentity.WebApi.Providers.JwtFormat;

[assembly: OwinStartup(typeof(AspNetIdentity.WebApi.Startup))]
namespace AspNetIdentity.WebApi
{
    public class Startup
    {
        internal static IDataProtectionProvider DataProtectionProvider { get; private set; }
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            DataProtectionProvider = app.GetDataProtectionProvider();
            SimpleInjectorInitializer.Initialize(config);
            WebApiConfig.Register(config);
            app.UseOAuthAuthorizationServer(OAuthTokenOptions());
            app.UseJwtBearerAuthentication(OAuthTokenConsumption());
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        private OAuthAuthorizationServerOptions OAuthTokenOptions()
        {
            return new OAuthAuthorizationServerOptions
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new OAuthProvider(),
                AccessTokenFormat = new JwtFormat()
            };
        }

        private JwtBearerAuthenticationOptions OAuthTokenConsumption()
        {
            var issuer = ConfigurationManager.AppSettings["Issuer"];
            string audienceId = ConfigurationManager.AppSettings["AudienceId"];
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["AudienceSecret"]);

            return new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AllowedAudiences = new[] {audienceId},
                IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                {
                    new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                }
            };
        }
    }
}