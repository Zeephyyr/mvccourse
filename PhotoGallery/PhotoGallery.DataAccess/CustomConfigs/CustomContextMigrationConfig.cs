using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace PhotoGallery.AppCommonCore
{
    public class CustomContextMigrationConfig : DbMigrationsConfiguration<DbContext>
    {
        public CustomContextMigrationConfig()
        {
            AutomaticMigrationsEnabled = true;
        }
    }
}
