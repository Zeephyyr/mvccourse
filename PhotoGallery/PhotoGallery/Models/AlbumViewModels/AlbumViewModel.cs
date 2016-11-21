using PhotoGallery.Common;
using System.Collections.Generic;

namespace PhotoGallery.Models
{
    public class AlbumViewModel
    {
        public AlbumInfoViewModel AlbumInfo { get; set; }

        public List<PhotoShortViewModel> Photos { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}