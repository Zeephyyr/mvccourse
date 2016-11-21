using System.Collections.Generic;

namespace PhotoGallery.AppCommonCore.Entities
{
    public class SearchResult
    {
        public List<PhotoShort> Photos { get; set; }

        public List<AlbumShort> Albums { get; set; }

        public List<UserShort> Users { get; set; }
    }
}
