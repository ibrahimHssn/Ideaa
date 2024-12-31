

using Ideaa.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Ideas.Data.Models
{
    public enum Validity
    {
        Admin,      // مسؤول
        Supervisor, // مشرف
        Student,    // طالب
    }

    // الكلاس User يرث من IdentityUser
    public class User : IdentityUser
    {




        [Required, MaxLength(255)]
        public string FirstName { get; set; } // الاسم الأول

        [Required, MaxLength(255)]
        public string LastName { get; set; } // الاسم الأخير

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // تاريخ إنشاء الحساب (يتم تعيينه تلقائيًا عند الإنشاء)

        public DateTime? LastLogin { get; set; } // تاريخ آخر تسجيل دخول (اختياري)

        [Required, MaxLength(255)]
        public required string Department { get; set; } // القسم

        [Required,]
        public Validity Validity { get; set; } // الصلاحية (طالب/مشرف/منسق)

        public int? Capacity { get; set; } // القدرة الاستيعابية (فقط للمشرفين)

        public int? UnitsNo { get; set; } // عدد الوحدات المسجلة (للطالب فقط)



        public ICollection<IdeaSupervisor> IdeaSupervisors { get; set; }

        public virtual ICollection<Notification>? Notifications { get; set; } // العلاقة مع جدول Notifications
        

    }
}
