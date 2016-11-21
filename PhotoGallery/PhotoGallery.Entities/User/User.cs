using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoGallery.Entities
{
    [Table("UserInfos")]
    public class User:UserBase
    {
        [Index(IsUnique = true)]
        [MaxLength(64)]
        public string UserId { get; set; }

        [MaxLength(500)]
        public string About { get; set; }

        //Yes I know, membership roles, stuff like that.
        //And also here should be one-to-many with roleid and userid
        public string Role { get; set; }

        public virtual List<Photo> LikesTo { get; set; }

        public virtual List<Photo> Photos { get; set; }

        public virtual List<Album> Albums { get; set; }
    }
}
