using System;
using System.Linq;
using System.Security.Claims;
using AspNetIdentity.Domain.IdentityEntity;

namespace AspNetIdentity.WebApi.Providers
{
    public static class SuperAdminRoleProvider
    {
        public static Claim ValidateSuperAdmin(User user)
        {
            var daysInWork = (DateTime.Now.Date - user.JoinDate).TotalDays;
            var isAbleToBeSuperAdmin =
                user.Claims.Any(c => c.ClaimType.Equals("SuperAdmin") && c.ClaimValue.Equals("true"));

            if (daysInWork >= 90 || isAbleToBeSuperAdmin)
                return new Claim(ClaimTypes.Role, "SuperAdmin");

            return new Claim(ClaimTypes.Role, "User");
        }
    }
}