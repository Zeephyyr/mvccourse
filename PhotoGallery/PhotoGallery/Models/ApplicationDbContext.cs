using Microsoft.AspNet.Identity.EntityFramework;
using PhotoGallery.Models.EntityMappings;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PhotoGallery.Models
{
    public class ApplicationUsersDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationUsersDbContext()
            : base("PhotoGalleryConnection", throwIfV1Schema: false)
        {
            Database.CreateIfNotExists();
        }

        public static ApplicationUsersDbContext Create()
        {
            return new ApplicationUsersDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            UserHasPhotos.Map(modelBuilder);
            UserHasAlbums.Map(modelBuilder);

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            AlbumsToPhotos.Map(modelBuilder);
            UsersToPhotos.Map(modelBuilder);
        }
    }
}