using PhotoGallery.AppCommonCore.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PhotoGallery.DataAccess.DataContexts
{
    public class CustomContext: DbContext
    {
        public CustomContext()
            :base("PhotoGalleryConnection")
        {
            Database.CreateIfNotExists();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany<Photo>(u => u.Photos)
                .WithRequired(p => p.User)
                .HasForeignKey(p => p.UniqueUserName);

            modelBuilder.Entity<User>()
                .HasMany<Album>(u => u.Albums)
                .WithRequired(a => a.User)
                .HasForeignKey(a => a.UniqueUserName);

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Album>()
                .HasMany(a => a.Photos)
                .WithMany(p => p.Albums)
                .Map(m =>
                {
                    m.ToTable("PhotoInAlbum");
                    m.MapLeftKey("PhotoId");
                    m.MapRightKey("AlbumId");
                });

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
