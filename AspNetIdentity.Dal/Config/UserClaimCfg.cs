using System.Data.Entity.ModelConfiguration;
using AspNetIdentity.Domain.IdentityEntity;

namespace AspNetIdentity.Dal.Config
{
    public class UserClaimCfg : EntityTypeConfiguration<UserClaim>
    {
        public UserClaimCfg()
        {
            ToTable("UserClaim");
        }
    }
}