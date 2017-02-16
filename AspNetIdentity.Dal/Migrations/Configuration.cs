using System.Data.Entity.Migrations;

namespace AspNetIdentity.Dal.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AspNetIdentityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AspNetIdentityDbContext context)
        {
        }
    }
}
