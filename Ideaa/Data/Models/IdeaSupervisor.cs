using GraduationProjectIdeasPlatform.Data.Models;
using Ideas.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ideaa.Data.Models
{
    public class IdeaSupervisor
    {

        public int IdeaId { get; set; }
        [ForeignKey(nameof(IdeaId))]
        public Idea Idea { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } // المشرف أو المنسق

        public bool IsCoordinator { get; set; } // لتحديد ما إذا كان المستخدم منسقًا
    }
}
