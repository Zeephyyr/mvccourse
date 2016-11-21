using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoGallery.Entities
{
    [Table("Photos")]
    public class Photo: PhotoBase
    {
        [Key]
        public string PhotoId { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UploadDate { get; set; }

        [MaxLength(50)]
        public string Place { get; set; }

        [MaxLength(500)]
        public string CameraModel { get; set; }

        public int LensFocus { get; set; }

        public int Diaphragm { get; set; }

        public int ShutterSpeed { get; set; }

        [MaxLength(20)]
        public string ISO { get; set; }

        public bool Flash { get; set; }

        public byte[] SourceImageData { get; set; }

        public byte[] MiniatureImageData { get; set; }

        public byte[] MidSizeImageData { get; set; }

        public string ImageMimeType { get; set; }

        public int Rating { get; set; }

        public virtual User User {get;set;}

        public virtual ICollection<User> LikesFrom { get; set; }

        public virtual ICollection<Album> Albums { get; set; }


        [NotMapped]
        public bool IsRatedByCurrent { get; set; }
    }
}
