using Ideaa.Data.Models;
using Ideaa.Models;
using Ideas.Data.Models;
using Ideas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ideaa.Comment;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class IdeaSupervisorController : ControllerBase
{
    private readonly AppDbContext _db;

    public IdeaSupervisorController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("AddIdeaSupervisors")]
    public async Task<IActionResult> AddIdeaSupervisor([FromBody] List<IdeaSupervisorModel> IS)
    {
        new Comment_IdeaSupervisor().ConfigureIdeaSupervisorRelationshipKey();

        if (IS == null || IS.Count == 0)
            return BadRequest("No supervisors provided.");

        foreach (var supervisor in IS)
        {
            // التحقق من وجود المستخدم
            var userExists = await _db.Users.AnyAsync(u => u.Id == supervisor.UserId);
            if (!userExists)
                return NotFound($"User with ID {supervisor.UserId} does not exist.");

            // التحقق من وجود الفكرة
            var ideaExists = await _db.Ideas.AnyAsync(i => i.IdeaId == supervisor.IdeaId);
            if (!ideaExists)
                return NotFound($"Idea with ID {supervisor.IdeaId} does not exist.");

            // التحقق إذا كان المشرف قد تم إضافته بالفعل لهذه الفكرة
            var supervisorExists = await _db.IdeaSupervisors
                .AnyAsync(s => s.IdeaId == supervisor.IdeaId && s.UserId == supervisor.UserId);

            if (supervisorExists)
            {
                return BadRequest($"User with ID {supervisor.UserId} is already assigned as a supervisor for this idea.");
            }

            // إضافة المشرف للفكرة
            var ideaSupervisor = new IdeaSupervisor
            {
                IdeaId = supervisor.IdeaId,
                UserId = supervisor.UserId,
                IsCoordinator = supervisor.IsCoordinator
            };

            _db.IdeaSupervisors.Add(ideaSupervisor);

            // إرسال إشعار للمشرف
            var notification = new Notification
            {
                IdeaId = supervisor.IdeaId,
                UserId = supervisor.UserId,
                Message = $"You have been assigned as supervisor for the idea with ID {supervisor.IdeaId}." +
                         (supervisor.IsCoordinator ? " You are the coordinator." : ""),
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            _db.Notifications.Add(notification);
        }

        await _db.SaveChangesAsync();

        return Ok(new { message = "Supervisors added successfully." });
    }



    [HttpDelete("DeleteIdeaSupervisor")]
    public async Task<IActionResult> DeleteIdeaSupervisor([FromQuery] int ideaId, [FromQuery] string userId)
    {
        new Comment_IdeaSupervisor().DeleteIdeaSupervisor();
        // العثور على السجل الذي يتوافق مع IdeaId و UserId
        var supervisor = await _db.IdeaSupervisors
            .FirstOrDefaultAsync(s => s.IdeaId == ideaId && s.UserId == userId);

        // التحقق من وجود السجل
        if (supervisor == null)
            return NotFound($"Supervisor with IdeaId {ideaId} and UserId {userId} not found.");

        // حذف السجل
        _db.IdeaSupervisors.Remove(supervisor);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Supervisor deleted successfully." });
    }


    [HttpGet("GetSupervisorsByIdeaId/{ideaId}")]
    public async Task<IActionResult> GetSupervisorsByIdeaId(int ideaId)
    {
        new Comment_IdeaSupervisor().GetSupervisorsByIdeaId();
        var supervisors = await _db.IdeaSupervisors
            .Where(s => s.IdeaId == ideaId)
            .ToListAsync();

        if (supervisors == null || supervisors.Count == 0)
            return NotFound($"No supervisors found for Idea with ID {ideaId}.");

        return Ok(supervisors);
    }

    [HttpGet("GetSupervisorsByUserId/{userId}")]
    public async Task<IActionResult> GetSupervisorsByUserId(String userId)
    {
        new Comment_IdeaSupervisor().GetSupervisorsByUserId();

        var supervisors = await _db.IdeaSupervisors
            .Where(s => s.UserId == userId)
            .ToListAsync();

        if (supervisors == null || supervisors.Count == 0)
            return NotFound($"No supervisors found for User with ID {userId}.");

        return Ok(supervisors);
    }






}
