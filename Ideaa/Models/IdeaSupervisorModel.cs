using GraduationProjectIdeasPlatform.Data.Models;
using Ideas.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ideaa.Models
{
    public class IdeaSupervisorModel
    {
       

        public int IdeaId { get; set; }

        public string UserId { get; set; }

        public bool IsCoordinator { get; set; } // لتحديد ما إذا كان المستخدم منسقًا
    }
}
