using PhotoGallery.Common;
using System.Collections.Generic;

namespace PhotoGallery.AppCommonCore.Entities
{
    public class UsersWall
    {
        public List<PhotoShort> Photos { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
