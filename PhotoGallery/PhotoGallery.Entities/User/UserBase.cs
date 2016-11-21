using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoGallery.Entities
{
    public class UserBase
    {
        [Index(IsUnique = true)]
        [MaxLength(32)]
        [Key]
        public string UniqueUserName { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
    }
}
