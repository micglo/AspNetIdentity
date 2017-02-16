using System.Data.Entity.ModelConfiguration;
using AspNetIdentity.Domain.IdentityEntity;

namespace AspNetIdentity.Dal.Config
{
    public class UserLoginCfg : EntityTypeConfiguration<UserLogin>
    {
        public UserLoginCfg()
        {
            ToTable("UserLogin");
            HasKey(l => new
            {
                l.LoginProvider,
                l.ProviderKey,
                l.UserId
            });
        }
    }
}