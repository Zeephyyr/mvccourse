using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.Models
{
    public class AlbumShortViewModel
    {
        [Display(Name = "Author")]
        public string UniqueUserName { get; set; }

        [Display(Name = "Album name")]
        public string AlbumName { get; set; }

        public byte[] ImageData { get; set; }

        public string ImageMimeType { get; set; }
    }
}