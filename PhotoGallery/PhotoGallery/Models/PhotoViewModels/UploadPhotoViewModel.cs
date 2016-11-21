using PhotoGallery.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.Models
{
    public class UploadPhotoViewModel
    {
        public string UniqueUserName { get; set; }

        [Required]
        [Display(Name = "Name")]
        [MaxLength(32)]
        public string PhotoName { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate
        {
            get
            {
                if (_internalCreationDate == new DateTime())
                    _internalCreationDate = new DateTime(1826, 1, 1);
                return _internalCreationDate;
            }
            set
            {
                _internalCreationDate = value;
            }
        }

        private DateTime _internalCreationDate;

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

        [Required]
        public DiaphragmType Diaphragm { get; set; }

        [Required]
        [Display(Name = "Shutter speed")]
        public int ShutterSpeed { get; set; }

        [MaxLength(20)]
        public string ISO { get; set; }

        [Required]
        public bool Flash { get; set; }

        public DateTime UploadDate
        {
            get
            {
                return DateTime.Now;
            }
        }

        public byte[] SourceImageData { get; set; }

        public byte[] MiniatureImageData { get; set; }

        public byte[] MidSizeImageData { get; set; }

        public string ImageMimeType { get; set; }
    }
}