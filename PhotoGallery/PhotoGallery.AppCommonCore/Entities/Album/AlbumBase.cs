using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoGallery.AppCommonCore.Entities
{
    public class AlbumBase
    {
        [MaxLength(32)]
        [Index("AlbumIndex", IsUnique = true, Order = 1)]
        public string AlbumName { get; set; }

        [MaxLength(32)]
        [Index("AlbumIndex", IsUnique = true, Order = 2)]
        public string UniqueUserName { get; set; }
    }
}
