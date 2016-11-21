﻿using System;

namespace PhotoGallery.Entities
{
    public class AlbumInfo : AlbumBase
    {
        public string Description { get; set; }

        public byte[] ImageData { get; set; }

        public string ImageMimeType { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
