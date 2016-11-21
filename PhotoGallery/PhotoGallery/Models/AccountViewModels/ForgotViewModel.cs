using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.Models
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}