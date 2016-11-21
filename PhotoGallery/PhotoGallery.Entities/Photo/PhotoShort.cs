namespace PhotoGallery.Entities
{
    public class PhotoShort:PhotoBase
    {
        public byte[] ImageData { get; set; }

        public string ImageMimeType { get; set; }
    }
}
