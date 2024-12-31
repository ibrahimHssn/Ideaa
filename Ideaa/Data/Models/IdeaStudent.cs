using GraduationProjectIdeasPlatform.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ideas.Data.Models
{

    public enum Role
    {
        Leader,  // قائد
        Member,  // عضو
        Reviewer // مراجع
    }
    public class IdeaStudent
    {
        [Key]
        public int IdeaStudentId { get; set; } 

        public int IdeaId { get; set; } // معرف الفكرة
        [ForeignKey(nameof(IdeaId))]
        public Idea Idea { get; set; } // العلاقة مع جدول الأفكار


        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } // الطالب المرتبط بالفكرة

        public Role Role { get; set; } // الدور (قائد، عضو، مراجع)



    
    }
}
