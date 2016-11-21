using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoGallery.AppCommonCore.Entities
{
    [Table("Albums")]
    public class Album: AlbumBase
    {
        [Key]
        public string AlbumId { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public byte[] ImageData { get; set; }

        public string ImageMimeType { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }
    }
}
