using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.Models
{
    public class PhotoShortViewModel
    {
        [Display(Name = "Author")]
        public string UniqueUserName { get; set; }

        [Display(Name = "PhotoName")]
        public string PhotoName { get; set; }

        public byte[] ImageData { get; set; }

        public string ImageMimeType { get; set; }
    }
}