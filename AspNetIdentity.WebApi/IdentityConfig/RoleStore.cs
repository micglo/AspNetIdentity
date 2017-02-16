using System.Data.Entity;
using AspNetIdentity.Domain.IdentityEntity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNetIdentity.WebApi.IdentityConfig
{
    public class RoleStore : RoleStore<Role, string, UserRole>
    {
        public RoleStore(DbContext context) : base(context)
        {
        }
    }
}