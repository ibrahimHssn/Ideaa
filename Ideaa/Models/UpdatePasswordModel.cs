using System.ComponentModel.DataAnnotations;

namespace Ideas.Models
{
    public class UpdatePasswordModel
    {
        [Required]
        public string UserId { get; set; } 

        [Required]
        public string UserName { get; set; }

        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string NewPassword { get; set; }
    }
}
