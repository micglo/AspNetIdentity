using System;
using AspNetIdentity.Domain.IdentityEntity;
using AspNetIdentity.WebApi.Utility;
using AspNetIdentity.WebApi.Utility.CustomUserValidation;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace AspNetIdentity.WebApi.IdentityConfig
{
    public class UserManager : UserManager<User, string>
    {
        public UserManager(UserStore userStore) : base(userStore)
        {
            UserValidator = new MyUserValidator(this)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            PasswordValidator = new MyPasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = false,
                RequireUppercase = false
            };

            EmailService = new EmailService();
            var dataProtectionProvider = Startup.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                UserTokenProvider = new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    TokenLifespan = TimeSpan.FromDays(1)
                };
            }
        }
    }
}