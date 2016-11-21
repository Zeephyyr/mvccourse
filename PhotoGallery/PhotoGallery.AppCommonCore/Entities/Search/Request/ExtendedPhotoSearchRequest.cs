using PhotoGallery.Common.Enums;

namespace PhotoGallery.AppCommonCore.Entities
{
    public class ExtendedPhotoSearchRequest:PhotoBase
    {
        public string Description { get; set; }

        public string Place { get; set; }

        public string CameraModel { get; set; }

        public int? LensFocus { get; set; }

        public DiaphragmType? Diaphragm { get; set; }

        public int? ShutterSpeed { get; set; }

        public string ISO { get; set; }

        public bool? Flash { get; set; }

        public string FlashStub { get; set; }
    }
}
