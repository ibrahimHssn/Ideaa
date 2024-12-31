using GraduationProjectIdeasPlatform.Data.Models;
using Ideaa.Data.Models;

namespace Ideas.Data.Models
{
    public class IdeaModels
    {
        public int IdeaId { get; set; }
        public string ProjectTitle { get; set; }
        public bool ProjectState { get; set; }
        public int Code { get; set; }
        public string TargetParty { get; set; }
        public string SummaryIdea { get; set; }
        public string FutureIdeas { get; set; }
        public string Objectives { get; set; }
        public string TechniquesUsed { get; set; }
        public string ProjectType { get; set; }
        public string ProjectLink { get; set; }
        public string ProjectBook { get; set; }

        public List<IdeaSupervisorsResponse> IdeaSupervisorsData { get; set; }

        public List<IdeaStudentResponse> IdeaStudentsData { get; set; }
    }

    public class IdeaStudentResponse
    {
        public string UserId { get; set; }
        public Role Role { get; set; }
    }

    public class IdeaSupervisorsResponse
    {
        public string UserId { get; set; }
        public bool IsCoordinator { get; set; }  // لتحديد ما إذا كان المستخدم منسقًا
    }
}
