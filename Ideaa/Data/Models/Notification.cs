    using GraduationProjectIdeasPlatform.Data.Models;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json; // إضافة المكتبة

namespace Ideas.Data.Models
    {
        public class Notification
        {
        [Key]
        public int NotificationId { get; set; }



        public String UserId { get; set; } // معرف المستخدم
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } // العلاقة مع جدول المستخدمين



        public int IdeaId { get; set; } // معرف الفكرة
        [ForeignKey(nameof(IdeaId))]
        public Idea Idea { get; set; } // العلاقة مع جدول الأفكار

        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = true ; // حالة قراءة التنبيه


        }



    }


