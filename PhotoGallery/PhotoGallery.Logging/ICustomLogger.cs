using System;

namespace PhotoGallery.Logging
{
    public interface ICustomLogger
    {
        void Exception(Exception ex);

        void Exception(Exception ex, string msg);

        void Exception(Exception ex, string msg, params string[] list);

        void Error(string msg);

        void Error(string msg, params string[] list);

        void Info(string msg);

        void Info(string msg, params string[] list);

        void DebugInfo(string msg);

        void DebugInfo(string msg, params string[] list);
    }
}
