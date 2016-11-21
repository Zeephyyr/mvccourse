using System;
using System.Web.Mvc;

namespace PhotoGallery.HtmlHelpers
{
    public static class DisplayImageHelper
    {
        public static string GetImageSource(this HtmlHelper html,byte[] imageData,string imageMimeType)
        {
            var base64 = Convert.ToBase64String(imageData);
            var imgSrc = String.Format("data:{0};base64,{1}", imageMimeType, base64);
            return imgSrc;
        }
    }
}