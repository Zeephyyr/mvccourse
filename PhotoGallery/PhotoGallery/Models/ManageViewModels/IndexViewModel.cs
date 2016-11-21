using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace PhotoGallery.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public bool BrowserRemembered { get; set; }
    }
}
