using PhotoGallery.Common;
using System.Collections.Generic;

namespace PhotoGallery.AppCommonCore.Entities
{
    public class AlbumResult
    {
        public AlbumInfo AlbumInfo { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public List<PhotoShort> Photos { get; set; }
    }
}
