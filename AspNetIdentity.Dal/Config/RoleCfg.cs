using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using AspNetIdentity.Domain.IdentityEntity;

namespace AspNetIdentity.Dal.Config
{
    public class RoleCfg : EntityTypeConfiguration<Role>
    {
        public RoleCfg()
        {
            ToTable("Role");
            Property(r => r.Name).IsRequired().HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("RoleNameIndex")
                {
                    IsUnique = true
                }));

            HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);
        }
    }
}