using PhotoGallery.AppCommonCore.Entities;
using System.Data.Entity;

namespace PhotoGallery.Models.EntityMappings
{
    public static class UserHasAlbums
    {
        public static void Map(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany<Album>(u => u.Albums)
                .WithRequired(a => a.User)
                .HasForeignKey(a => a.UniqueUserName);
        }
    }
}