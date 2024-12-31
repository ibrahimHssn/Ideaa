using Ideaa.Comment;
using Ideaa.Models;
using Ideas.Data;
using Ideas.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class IdeaStudentController : ControllerBase
{
    private readonly AppDbContext _db;

    public IdeaStudentController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("AddIdeaStudents")]
    public async Task<IActionResult> AddIdeaStudents([FromBody] List<IdeaStudentModel> ideaStudents)
    {

        new Comment_IdeaStudent().AddIdeaStudents();
        if (ideaStudents == null || ideaStudents.Count == 0)
            return BadRequest("No students provided.");

        foreach (var student in ideaStudents)
        {
            // التحقق من وجود المستخدم
            var userExists = await _db.Users.AnyAsync(u => u.Id == student.UserId);
            if (!userExists)
                return NotFound($"User with ID {student.UserId} does not exist.");

            // التحقق من وجود الفكرة
            var ideaExists = await _db.Ideas.AnyAsync(i => i.IdeaId == student.IdeaId);
            if (!ideaExists)
                return NotFound($"Idea with ID {student.IdeaId} does not exist.");

            // التحقق إذا كان الطالب قد تم إضافته بالفعل لهذه الفكرة
            var studentExists = await _db.IdeaStudents
                .AnyAsync(s => s.IdeaId == student.IdeaId && s.UserId == student.UserId);

            if (studentExists)
            {
                return BadRequest($"User with ID {student.UserId} is already assigned to this idea.");
            }

            // تحويل IdeaStudentModel إلى IdeaStudent
            var ideaStudent = new IdeaStudent
            {
                IdeaId = student.IdeaId,
                UserId = student.UserId
            };

            // إضافة الطالب للفكرة
            _db.IdeaStudents.Add(ideaStudent);
        }

        await _db.SaveChangesAsync();

        return Ok(new { message = "Students added successfully.", });
    }

    [HttpDelete("DeleteIdeaStudent")]
    public async Task<IActionResult> DeleteIdeaStudent([FromQuery] int ideaId, [FromQuery] string userId)
    {

        new Comment_IdeaStudent().DeleteIdeaStudent();
        // العثور على السجل الذي يتوافق مع IdeaId و UserId
        var student = await _db.IdeaStudents
            .FirstOrDefaultAsync(s => s.IdeaId == ideaId && s.UserId == userId);

        // التحقق من وجود السجل
        if (student == null)
            return NotFound($"Student with IdeaId {ideaId} and UserId {userId} not found.");

        // حذف السجل
        _db.IdeaStudents.Remove(student);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Student removed successfully." });
    }

    [HttpGet("GetStudentsByIdeaId/{ideaId}")]
    public async Task<IActionResult> GetStudentsByIdeaId(int ideaId)
    {
        new Comment_IdeaStudent().GetStudentsByIdeaId();
        var students = await _db.IdeaStudents
            .Where(s => s.IdeaId == ideaId)
            .ToListAsync();

        if (students == null || students.Count == 0)
            return NotFound($"No students found for Idea with ID {ideaId}.");

        return Ok(students);
    }

    [HttpGet("GetStudentsByUserId/{userId}")]
    public async Task<IActionResult> GetStudentsByUserId(string userId)
    {
        new Comment_IdeaStudent().GetStudentsByUserId();
        var students = await _db.IdeaStudents
            .Where(s => s.UserId == userId)
            .ToListAsync();

        if (students == null || students.Count == 0)
            return NotFound($"No ideas found for User with ID {userId}.");

        return Ok(students);
    }
}
