using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using AspNetIdentity.Domain.IdentityEntity;

namespace AspNetIdentity.Dal.Config
{
    public class UserCfg : EntityTypeConfiguration<User>
    {
        public UserCfg()
        {
            ToTable("User");
            Property(u => u.FirstName).IsRequired().HasMaxLength(100);
            Property(u => u.LastName).IsRequired().HasMaxLength(100);
            Property(u => u.JoinDate).IsRequired();
            Property(u => u.Gender).IsRequired();
            Property(u => u.BirthDate).IsOptional();
            Property(u => u.IsBanned).IsRequired();

            Property(u => u.UserName).IsRequired().HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex")
                {
                    IsUnique = true
                }));
            Property(u => u.Email).IsRequired().HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("EmailIndex")
                {
                    IsUnique = true
                }));

            HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
        }
    }
}