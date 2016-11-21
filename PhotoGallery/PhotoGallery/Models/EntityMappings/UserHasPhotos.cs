using PhotoGallery.AppCommonCore.Entities;
using System.Data.Entity;

namespace PhotoGallery.Models.EntityMappings
{
    public static class UserHasPhotos
    {
        public static void Map(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany<Photo>(u => u.Photos)
                .WithRequired(p => p.User)
                .HasForeignKey(p => p.UniqueUserName);
        }
    }
}