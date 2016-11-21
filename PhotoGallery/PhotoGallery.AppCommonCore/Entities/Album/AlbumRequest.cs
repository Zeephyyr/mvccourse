using PhotoGallery.Common;

namespace PhotoGallery.AppCommonCore.Entities
{
    public class AlbumRequest:AlbumBase
    {
        public PagingInfo PagingInfo { get; set; }
    }
}
