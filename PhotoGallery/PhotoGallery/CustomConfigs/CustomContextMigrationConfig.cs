using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace PhotoGallery
{
    public class CustomContextMigrationConfig : DbMigrationsConfiguration<DbContext>
    {
        public CustomContextMigrationConfig()
        {
            AutomaticMigrationsEnabled = true;
        }
    }
}
