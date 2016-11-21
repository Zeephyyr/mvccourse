using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.Models
{
    public class AddAlbumViewModel
    {
        public string UniqueUserName { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 2)]
        [Display(Name = "Name")]
        public string AlbumName { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        public byte[] ImageData { get; set; }

        public string ImageMimeType { get; set; }
    }
}