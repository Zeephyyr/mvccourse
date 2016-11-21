using System.Configuration;

namespace PhotoGallery.Common
{
    public static class ConfigurationElements
    {
        public static int MaxPhotoCount { get; private set; }

        public static int MaxAlbumCount { get; private set; }

        public static int DefaultPageLimit { get; private set; }

        public static int MaxFileSize { get; private set; }

        public static int ModifierForMaxSize { get; private set; }

        public static string ConnectionString { get; private set; }

        private static bool isInit = false;

        public static void InitConfig()
        {
            if(!isInit)
            {
                MaxPhotoCount = int.Parse(ConfigurationManager.AppSettings["MaxPhotoCount"]);
                MaxAlbumCount = int.Parse(ConfigurationManager.AppSettings["MaxAlbumCount"]);
                DefaultPageLimit = int.Parse(ConfigurationManager.AppSettings["DefaultPageLimit"]);

                MaxFileSize = int.Parse(ConfigurationManager.AppSettings["MaxFileSize"]);
                ModifierForMaxSize = int.Parse(ConfigurationManager.AppSettings["ModifierForMaxSize"]);
                ConnectionString = ConfigurationManager.ConnectionStrings["PhotoGalleryConnection"].ConnectionString;

                isInit = true;
            }
        }
    }
}
