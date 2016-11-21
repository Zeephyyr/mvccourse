using PhotoGallery.AppCommonCore.Entities;
using System.Data.Entity;

namespace PhotoGallery.Models.EntityMappings
{
    public static class AlbumsToPhotos
    {
        public static void Map(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>()
                .HasMany(a => a.Photos)
                .WithMany(p => p.Albums)
                .Map(m =>
                {
                    m.ToTable("PhotoInAlbum");
                    m.MapLeftKey("PhotoId");
                    m.MapRightKey("AlbumId");
                });
        }
    }
}