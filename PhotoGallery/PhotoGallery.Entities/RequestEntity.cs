using PhotoGallery.Common.Enums;
using System;

namespace PhotoGallery.Entities
{
    public class RequestEntity
    {
        public PagingInfo PagingInfo { get; set; }

        public string AlbumName { get; set; }

        //Also owner username
        public string UniqueUserName { get; set; }

        public string PhotoName { get; set; }

        //Also fan username
        public string CurrentUserName { get; set; }

        public RequiredImgSize RequiredSize { get; set; }

        public string UserId { get; set; }

    }
}
