using System.Data.Entity.ModelConfiguration;
using AspNetIdentity.Domain.IdentityEntity;

namespace AspNetIdentity.Dal.Config
{
    public class UserRoleCfg : EntityTypeConfiguration<UserRole>
    {
        public UserRoleCfg()
        {
            ToTable("UserRole");
            HasKey(r => new
            {
                r.UserId,
                r.RoleId
            });
        }
    }
}