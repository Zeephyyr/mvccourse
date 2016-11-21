using PhotoGallery.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.Models
{
    public class UpdatePhotoViewModel
    {
        [Required]
        public string PhotoId { get; set; }

        [Required]
        public string UniqueUserName { get; set; }

        [Required]
        [Display(Name = "Name")]
        [MaxLength(32)]
        public string PhotoName { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string Place { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Camera model")]
        public string CameraModel { get; set; }

        [Required]
        [Display(Name = "Lens focus")]
        public int LensFocus { get; set; }

        public DiaphragmType Diaphragm { get; set; }

        [Required]
        [Display(Name = "Shutter speed")]
        public int ShutterSpeed { get; set; }

        [MaxLength(20)]
        public string ISO { get; set; }

        [Required]
        public bool Flash { get; set; }
    }
}