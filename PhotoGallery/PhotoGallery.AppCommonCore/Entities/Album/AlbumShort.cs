namespace PhotoGallery.AppCommonCore.Entities
{
    public class AlbumShort:AlbumBase
    {
        public byte[] ImageData { get; set; }

        public string ImageMimeType { get; set; }
    }
}
