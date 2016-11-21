using PhotoGallery.AppCommonCore.Entities;
using System.Data.Entity;

namespace PhotoGallery.Models.EntityMappings
{
    public static class UsersToPhotos
    {
        public static void Map(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(a => a.LikesTo)
                .WithMany(p => p.LikesFrom)
                .Map(m =>
                {
                    m.ToTable("UsersLikePhotos");
                    m.MapLeftKey("PhotoId");
                    m.MapRightKey("AlbumId");
                });
        }
    }
}