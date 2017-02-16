using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using AspNetIdentity.Dal.Config;
using AspNetIdentity.Domain.IdentityEntity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNetIdentity.Dal
{
    public class AspNetIdentityDbContext : IdentityDbContext<User, Role, string, UserLogin, UserRole, UserClaim>
    {
        public AspNetIdentityDbContext()
            : base("AspNetIdentityConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new UserCfg());
            modelBuilder.Configurations.Add(new RoleCfg());
            modelBuilder.Configurations.Add(new UserRoleCfg());
            modelBuilder.Configurations.Add(new UserClaimCfg());
            modelBuilder.Configurations.Add(new UserLoginCfg());
        }
    }
}