using Ideas.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Ideas.Models
{
    public class RegisterModels
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required()]
        public string PhoneNumber { get; set; }

        public string Department { get; set; }

        public Validity Validity { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; } // تأكد من وجود هذا الحقل بنفس الاسم تأكد من وجود هذا الحقل بنفس الاسم
    }
}
