using AspNetIdentity.Domain.IdentityEntity;
using Microsoft.AspNet.Identity;

namespace AspNetIdentity.WebApi.IdentityConfig
{
    public class RoleManager : RoleManager<Role, string>
    {
        public RoleManager(RoleStore store) : base(store)
        {
        }
    }
}