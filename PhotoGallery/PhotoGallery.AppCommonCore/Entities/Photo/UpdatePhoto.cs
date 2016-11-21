using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.AppCommonCore.Entities
{
    public class UpdatePhoto: PhotoBase
    {
        public string PhotoId { get; set; }

        public string Description { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        public string Place { get; set; }

        public string CameraModel { get; set; }

        public int LensFocus { get; set; }

        public int Diaphragm { get; set; }

        public int ShutterSpeed { get; set; }

        public string ISO { get; set; }

        public bool Flash { get; set; }
    }
}
