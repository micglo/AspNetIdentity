using AspNetIdentity.Dal;
using AspNetIdentity.Domain.IdentityEntity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNetIdentity.WebApi.IdentityConfig
{
    public class UserStore : UserStore<User, Role, string, UserLogin, UserRole, UserClaim>
    {
        public UserStore(AspNetIdentityDbContext context) : base(context)
        {
        }
    }
}