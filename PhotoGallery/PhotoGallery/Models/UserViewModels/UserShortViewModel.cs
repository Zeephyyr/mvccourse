using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.Models
{
    public class UserShortViewModel
    {
        [Display(Name = "Nickname")]
        public string UniqueUserName { get; set; }

        public string Name { get; set; }
    }
}