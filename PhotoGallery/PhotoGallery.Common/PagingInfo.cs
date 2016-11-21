using System;

namespace PhotoGallery.Common
{
    public class PagingInfo
    {
        public int TotalObjects { get; set; }

        public int ObjectsPerPage { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((decimal)TotalObjects / ObjectsPerPage);
            }
        }
    }
}
