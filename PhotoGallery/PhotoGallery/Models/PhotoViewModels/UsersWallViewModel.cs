using PhotoGallery.Common;
using System.Collections.Generic;

namespace PhotoGallery.Models
{
    public class UsersWallViewModel
    {
        public List<PhotoShortViewModel> Photos { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}