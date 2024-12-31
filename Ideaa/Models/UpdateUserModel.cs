using Ideas.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Ideaa.Models
{
    public class UpdateUserModel: RegisterModels
    {
        [Required]
        public string UserId { get; set; }
    
        [JsonIgnore] // منع إرسال كلمة المرور في JSON
        public string?Password { get; set; }

    }
}
