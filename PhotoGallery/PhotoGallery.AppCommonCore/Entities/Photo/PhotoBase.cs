using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoGallery.AppCommonCore.Entities
{ 
    public class PhotoBase
    {
        [MaxLength(32)]
        [Index("PhotoIndex", IsUnique = true, Order = 1)]
        public string PhotoName { get; set; }

        [MaxLength(32)]
        [Index("PhotoIndex", IsUnique = true, Order = 2)]
        public string UniqueUserName { get; set; }
    }
}
