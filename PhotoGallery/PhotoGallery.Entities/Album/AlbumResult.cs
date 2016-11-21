using System.Collections.Generic;

namespace PhotoGallery.Entities
{
    public class AlbumResult
    {
        public AlbumInfo AlbumInfo { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public List<PhotoShort> Photos { get; set; }
    }
}
