using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.Models
{
    public class FillAdditionalDataViewModel
    {
        public string UserId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(500)]
        public string About { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "UserName")]
        public string UniqueUserName { get; set; }

        public string Role { get; set; }
    }
}