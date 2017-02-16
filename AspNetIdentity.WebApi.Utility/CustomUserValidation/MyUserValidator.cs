using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetIdentity.Domain.IdentityEntity;
using Microsoft.AspNet.Identity;

namespace AspNetIdentity.WebApi.Utility.CustomUserValidation
{
    public class MyUserValidator : UserValidator<User>
    {
        private readonly List<string> _allowedEmailDomains = new List<string> { "outlook.com", "hotmail.com", "gmail.com", "yahoo.com" };
        public MyUserValidator(UserManager<User, string> manager) : base(manager)
        {
        }

        public override async Task<IdentityResult> ValidateAsync(User user)
        {
            IdentityResult result = await base.ValidateAsync(user);

            var emailDomain = user.Email.Split('@')[1];

            if (!_allowedEmailDomains.Contains(emailDomain.ToLower()))
            {
                var errors = result.Errors.ToList();

                errors.Add($"Email domain '{emailDomain}' is not allowed");

                result = new IdentityResult(errors);
            }

            return result;
        }
    }
}