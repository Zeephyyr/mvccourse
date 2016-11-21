using System.Collections.Generic;

namespace PhotoGallery.Models
{
    public class SearchResultViewModel
    {
        public List<PhotoShortViewModel> Photos { get; set; }

        public List<AlbumShortViewModel> Albums { get; set; }

        public List<UserShortViewModel> Users { get; set; }
    }
}