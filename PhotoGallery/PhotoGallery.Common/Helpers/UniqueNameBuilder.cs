namespace PhotoGallery.Common.Helpers
{
    public static class UniqueNameBuilder
    {
        public static string GetUniqueName(string ownerName,string objectName)
        {
            return ownerName + "-" + objectName;
        }
    }
}
