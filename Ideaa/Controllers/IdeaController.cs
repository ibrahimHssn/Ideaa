using GraduationProjectIdeasPlatform.Data.Models;
using Ideaa.Comment;
using Ideaa.Data.Models;
using Ideas.Data;
using Ideas.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ideaa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IdeaaController : ControllerBase
    {

        private readonly AppDbContext _db;

        public IdeaaController(AppDbContext db)
        {
            _db = db;
        }




        [HttpPost("AddIdea")]
        public async Task<IActionResult> AddIdea([FromBody] IdeaModels idea)
        {
            new CommentIdeaController().AddIdeaa();


            // الحصول على معرف المستخدم
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User is not authenticated.");

            // التحقق من البيانات المدخلة
            if (idea == null || string.IsNullOrEmpty(idea.ProjectTitle) || string.IsNullOrEmpty(idea.SummaryIdea))
                return BadRequest("Project title and summary are required.");

            // إنشاء الفكرة
            var newIdea = new Idea
            {
                ProjectTitle = idea.ProjectTitle,
                ProjectState = idea.ProjectState,
                Code = idea.Code,
                TargetParty = idea.TargetParty,
                SummaryIdea = idea.SummaryIdea,
                FutureIdeas = idea.FutureIdeas,
                Objectives = idea.Objectives,
                TechniquesUsed = idea.TechniquesUsed,
                ProjectType = idea.ProjectType,
                ProjectLink = idea.ProjectLink,
                ProjectBook = idea.ProjectBook
            };

            _db.Ideas.Add(newIdea);
            await _db.SaveChangesAsync();

            // تعيين المشرفين للفكرة
            await AssignSupervisorsToIdea(idea, newIdea.IdeaId);

            // تعيين الطلاب
            await AssignStudentsToIdea(idea, newIdea.IdeaId);

            // إرسال إشعار للمسؤول
            await SendAdminNotification(userId, newIdea);

            return Ok(new { message = "Idea created successfully.", data = await GetIdea(newIdea.IdeaId) });
        }

        private async Task AssignSupervisorsToIdea(IdeaModels idea, int ideaId)
        {
            if (idea.IdeaSupervisorsData != null && idea.IdeaSupervisorsData.Any())
            {
                foreach (var supervisor in idea.IdeaSupervisorsData)
                {
                    // التحقق من وجود المستخدم
                    var userExists = await _db.Users.AnyAsync(u => u.Id == supervisor.UserId);
                    if (!userExists)
                        throw new Exception($"User with ID {supervisor.UserId} does not exist.");

                    // إضافة المشرف للفكرة
                    var ideaSupervisor = new IdeaSupervisor
                    {
                        IdeaId = ideaId,
                        UserId = supervisor.UserId,
                        IsCoordinator = supervisor.IsCoordinator
                    };

                    _db.IdeaSupervisors.Add(ideaSupervisor);

                    // إرسال إشعار للمشرف
                    var notification = new Notification
                    {
                        IdeaId = ideaId,
                        UserId = supervisor.UserId,
                        Message = $"You have been assigned as supervisor for the idea '{idea.ProjectTitle}'." +
                                  (supervisor.IsCoordinator ? " You are the coordinator." : ""),
                        CreatedAt = DateTime.UtcNow,
                        IsRead = false
                    };

                    _db.Notifications.Add(notification);
                }
                await _db.SaveChangesAsync();
            }
        }

        private async Task AssignStudentsToIdea(IdeaModels idea, int ideaId)
        {
            new CommentIdeaController().AssignStudentsToIdea();

            if (idea.IdeaStudentsData != null && idea.IdeaStudentsData.Any())
            {
                foreach (var student in idea.IdeaStudentsData)
                {
                    // التحقق من وجود المستخدم
                    var userExists = await _db.Users.AnyAsync(u => u.Id == student.UserId);
                    if (!userExists)
                        throw new Exception($"User with ID {student.UserId} does not exist.");

                    // إضافة الطالب للفكرة
                    var ideaStudent = new IdeaStudent
                    {
                        IdeaId = ideaId,
                        UserId = student.UserId,
                        Role = student.Role
                    };

                    _db.IdeaStudents.Add(ideaStudent);

                    // إرسال إشعار للطالب
                    var notification = new Notification
                    {
                        IdeaId = ideaId,
                        UserId = student.UserId,
                        Message = $"You have been assigned to the idea '{idea.ProjectTitle}' as {student.Role}.",
                        CreatedAt = DateTime.UtcNow,
                        IsRead = false
                    };

                    _db.Notifications.Add(notification);
                }
                await _db.SaveChangesAsync();
            }
        }

        private async Task SendAdminNotification(string userId, Idea newIdea)
        {
            new CommentIdeaController().SendAdminNotification();


            var adminNotification = new Notification
            {
                IdeaId = newIdea.IdeaId,
                UserId = userId,
                Message = $"Your idea '{newIdea.ProjectTitle}' has been successfully created.",
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            _db.Notifications.Add(adminNotification);
            await _db.SaveChangesAsync();
        }






        [HttpGet("GetAllIdeas")]
        public async Task<IActionResult> GetAllIdeas()
        {
            new CommentIdeaController().GetAllIdeas();

            var ideas = await _db.Ideas
                .Select(i => new
                {
                    i.IdeaId,
                    i.ProjectTitle,
                    i.ProjectState,
                    i.Code,
                    i.TargetParty,
                    i.SummaryIdea,
                    i.FutureIdeas,
                    i.Objectives,
                    i.TechniquesUsed,
                    i.ProjectType,
                    i.ProjectLink,
                    i.ProjectBook,

                    // إضافة فكرة المشرفين
                    IdeaSupervisors = i.IdeaSupervisors.Select(s => new
                    {
                        s.User.FirstName,
                        s.User.LastName,
                        s.User.Department,
                        s.User.Validity // أي بيانات تحتاجها عن المشرف
                    }).ToList(),

                    // إضافة فكرة الطلاب
                    IdeaStudents = i.IdeaStudents.Select(s => new
                    {
                        s.User.FirstName,
                        s.User.LastName,
                        s.User.Department,
                        s.Role // أو أي بيانات إضافية تحتاجها عن الطالب
                    }).ToList()
                })
                .ToListAsync();

            return Ok(new { data = ideas });
        }




        [HttpGet("GetAllIdeasWithPagination")]
        public async Task<IActionResult> GetAllIdeasWithPagination(int page = 1, int pageSize = 10)
        {

            new CommentIdeaController().GetAllIdeasWithPagination();
            var ideas = await _db.Ideas
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(i => new
                {
                    i.IdeaId,
                    i.ProjectTitle,
                    i.ProjectState,
                    i.Code,
                    i.TargetParty,
                    i.SummaryIdea,
                    i.FutureIdeas,
                    i.Objectives,
                    i.TechniquesUsed,
                    i.ProjectType,
                    i.ProjectLink,
                    i.ProjectBook
                })
                .ToListAsync();

            return Ok(new { data = ideas });
        }


        [HttpGet("GetAllIdeasWithPaginationAndStudents")]
        public async Task<IActionResult> GetAllIdeasWithPaginationAndStudents(int page = 1, int pageSize = 10)
        {
            new CommentIdeaController().GetAllIdeasWithPaginationAndStudents();

            var ideas = await _db.Ideas
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(i => new
                {
                    i.IdeaId,
                    i.ProjectTitle,
                    i.ProjectState,
                    i.Code,
                    i.TargetParty,
                    i.SummaryIdea,
                    i.FutureIdeas,
                    i.Objectives,
                    i.TechniquesUsed,
                    i.ProjectType,
                    i.ProjectLink,
                    i.ProjectBook,

                    // إضافة فكرة المشرفين
                    IdeaSupervisors = i.IdeaSupervisors.Select(s => new
                    {
                        s.User.FirstName,
                        s.User.LastName,
                        s.User.Department,
                        s.User.Validity
                    }).ToList(),

                    // إضافة فكرة الطلاب
                    IdeaStudents = i.IdeaStudents.Select(s => new
                    {
                        s.User.FirstName,
                        s.User.LastName,
                        s.User.Department,
                        s.Role
                    }).ToList()
                })
                .ToListAsync();

            return Ok(new { data = ideas });
        }



        [HttpGet("{ideaId}")]
        public async Task<IActionResult> GetIdea([FromRoute] int ideaId)
        {
            new CommentIdeaController().GetIdea();

            var idea = await _db.Ideas
                 .Where(i => i.IdeaId == ideaId)
                 .Select(i => new
                 {
                     i.IdeaId,
                     i.ProjectTitle,
                     i.ProjectState,
                     i.Code,
                     i.TargetParty,
                     i.SummaryIdea,
                     i.FutureIdeas,
                     i.Objectives,
                     i.TechniquesUsed,
                     i.ProjectType,
                     i.ProjectLink,
                     i.ProjectBook,
                     IdeaSupervisors = i.IdeaSupervisors.Select(s => new
                     {
                         s.User.FirstName,
                         s.User.LastName,
                         s.User.Department,
                         s.User.Validity
                     }).ToList(),
                     IdeaStudents = i.IdeaStudents.Select(s => new
                     {
                         s.UserId,
                         s.Role
                     }),
                     Notifications = i.Notifications.Select(n => new
                     {
                         n.Message,
                         n.CreatedAt,
                         n.IsRead
                     })

                 }).FirstOrDefaultAsync();

            if (idea == null)
                return NotFound("Idea not found.");

            return Ok(new { data = idea });
        }


        [HttpPut]
        public async Task<IActionResult> UpdateIdea([FromBody] IdeaModels idea)
        {

            new CommentIdeaController().UpdateIdea();


            if (idea == null || string.IsNullOrEmpty(idea.ProjectTitle) || string.IsNullOrEmpty(idea.SummaryIdea))
                return BadRequest("Project title and summary are required.");

            var existingIdea = await _db.Ideas.FindAsync(idea.IdeaId);
            if (existingIdea == null)
                return NotFound("Idea not found.");

            existingIdea.ProjectTitle = idea.ProjectTitle;
            existingIdea.ProjectState = idea.ProjectState;
            existingIdea.Code = idea.Code;
            existingIdea.TargetParty = idea.TargetParty;
            existingIdea.SummaryIdea = idea.SummaryIdea;
            existingIdea.FutureIdeas = idea.FutureIdeas;
            existingIdea.Objectives = idea.Objectives;
            existingIdea.TechniquesUsed = idea.TechniquesUsed;
            existingIdea.ProjectType = idea.ProjectType;
            existingIdea.ProjectLink = idea.ProjectLink;
            existingIdea.ProjectBook = idea.ProjectBook;

            _db.Ideas.Update(existingIdea);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Idea updated successfully.", Data = existingIdea });
        }

        [HttpDelete("{ideaId}")]
        public async Task<IActionResult> DeleteIdea([FromRoute] int ideaId)
        {
            new CommentIdeaController().DeleteIdea();

            var idea = await _db.Ideas.FindAsync(ideaId);
            if (idea == null)
                return NotFound("Idea not found.");

            _db.Ideas.Remove(idea);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Idea deleted successfully." });
        }

    }
}
