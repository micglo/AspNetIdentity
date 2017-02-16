using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetIdentity.Domain.CommonEntity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNetIdentity.Domain.IdentityEntity
{
    public sealed class User : IdentityUser<string, UserLogin, UserRole, UserClaim>, IEntityBase
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime JoinDate { get; set; }
        public Gender Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool IsBanned { get; set; }
        public bool IsActive { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, string> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            // Add custom user claims here
            return userIdentity;
        }
    }
}