using Ideaa.Data.Models;
using Ideas.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProjectIdeasPlatform.Data.Models
{
    public class Idea
    {
        [Key]
        public int IdeaId { get; set; } 

        [Required]
        [MaxLength(255)]
        public string ProjectTitle { get; set; } // عنوان المشروع

        public bool ProjectState { get; set; } = false; // حالة المشروع (مقترح/مقبول)

        public int Code { get; set; } //الكود الخاص بالفكرة	)

        [MaxLength(255)]
        public string TargetParty { get; set; } // الجهة المستفيدة

        [Required]
        public string SummaryIdea { get; set; } // ملخص الفكرة

        public string FutureIdeas { get; set; } // الأفكار المستقبلية

        public string Objectives { get; set; } // الأهداف

        public string TechniquesUsed { get; set; } // التقنيات المستخدمة

        public string ProjectType { get; set; } // نوع المشروع

        // الحقل الجديد لرابط المشروع
        public string ProjectLink { get; set; } // رابط المشروع

        // الحقل الجديد لرفع الكتاب
        public string ProjectBook { get; set; } // رابط الكتاب


        public ICollection<IdeaSupervisor> IdeaSupervisors { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<IdeaStudent> IdeaStudents { get; set; }


    }
}
