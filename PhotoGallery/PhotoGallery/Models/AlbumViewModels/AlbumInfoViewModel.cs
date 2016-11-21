using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.Models
{
    public class AlbumInfoViewModel
    {
        public string UniqueUserName { get; set; }

        [Display(Name = "Album name")]
        public string AlbumName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public byte[] ImageData { get; set; }

        public string ImageMimeType { get; set; }

        [Display(Name = "Creation date")]
        public DateTime CreationDate { get; set; }
    }
}