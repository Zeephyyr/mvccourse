using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.Models
{
    public class UpdateAlbumViewModel
    {
        [Required]
        public string AlbumId { get; set; }

        [Required]
        public string UniqueUserName { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 2)]
        public string AlbumName { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        public byte[] ImageData { get; set; }

        public string ImageMimeType { get; set; }
    }
}