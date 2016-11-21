using PhotoGallery.Common.Enums;
using System;

namespace PhotoGallery.Models
{
    public class PhotoViewModel
    {
        public string PhotoId { get; set; }

        public string UniqueUserName { get; set; }

        public string PhotoName { get; set; }

        public string Description { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UploadDate { get; set; }

        public string Place { get; set; }

        public string CameraModel { get; set; }

        public int LensFocus { get; set; }

        public DiaphragmType Diaphragm { get; set; }

        public int ShutterSpeed { get; set; }

        public string ISO { get; set; }

        public bool Flash { get; set; }

        public int Rating { get; set; }

        public byte[] MidSizeImageData { get; set; }

        public string ImageMimeType { get; set; }

        public bool IsRatedByCurrent { get; set; }
    }
}