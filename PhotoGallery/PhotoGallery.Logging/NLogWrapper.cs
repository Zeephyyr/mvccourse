using NLog;
using System;

namespace PhotoGallery.Logging
{
    public class NLogWrapper : ICustomLogger
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public void Exception(Exception ex)
        {
            _logger.Error(ex);
        }

        public void Exception(Exception ex,string msg)
        {
            _logger.Error(ex,msg);
        }

        public void Exception(Exception ex, string msg,params string[] list)
        {
            Exception(ex, string.Format(msg,list));
        }

        public void Error(string msg)
        {
            _logger.Error(msg);
        }

        public void Error(string msg, params string[] list)
        {
            Error(string.Format(msg,list));
        }

        public void DebugInfo(string msg)
        {
            _logger.Debug(msg);
        }

        public void DebugInfo(string msg, params string[] list)
        {
            DebugInfo(string.Format(msg,list));
        }

        public void Info(string msg)
        {
            _logger.Info(msg);
        }

        public void Info(string msg, params string[] list)
        {
            Info(string.Format(msg, list));
        }
    }
}
