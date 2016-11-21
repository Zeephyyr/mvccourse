using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.Models
{
    public class ChangeUserInfoViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(500)]
        public string About { get; set; }

        public string UserId { get; set; }
    }
}