using System.ComponentModel.DataAnnotations;

namespace Ideaa.Models
{
    public class LoginModels
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; } // تأكد من وجود هذا الحقل بنفس الاسم
    }
}
