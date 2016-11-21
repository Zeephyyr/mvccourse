using PhotoGallery.Common.Enums;

namespace PhotoGallery.Models
{
    public class ExtendedPhotoSearchViewModel
    {
        public string PhotoName { get; set; }

        public string UniqueUserName { get; set; }

        public string Description { get; set; }

        public string Place { get; set; }

        public string CameraModel { get; set; }

        public int? LensFocus { get; set; }

        public SearchDiaphragmType Diaphragm { get; set; }

        public int? ShutterSpeed { get; set; }

        public string ISO { get; set; }

        public bool? Flash { get; set; }

        public string FlashStub { get; set; }
    }
}