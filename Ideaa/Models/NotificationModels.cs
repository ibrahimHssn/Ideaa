using GraduationProjectIdeasPlatform.Data.Models;
using Ideas.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ideaa.Models
{
    public class NotificationModels
    {
        

        public String UserId { get; set; } // معرف المستخدم
       
        public int IdeaId { get; set; } // معرف الفكرة

        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false; // حالة قراءة التنبيه


    }
}
