namespace PhotoGallery.AppCommonCore.Entities
{
    public class UpdateAlbum : AlbumBase
    {
        public string AlbumId { get; set; }

        public string Description { get; set; }

        public byte[] ImageData { get; set; }

        public string ImageMimeType { get; set; }
    }
}
