using Ideas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Ideas.Data.Models;
using Microsoft.EntityFrameworkCore;
using Ideaa.HubS;
using Ideaa.Models;
using Microsoft.AspNetCore.Authorization;

namespace Ideas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationController(AppDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }


        // إحضار جميع التنبيهات الخاصة بمستخدم معين
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationsByUserId(string userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            if (notifications == null || !notifications.Any())
            {
                return NotFound("No notifications found.");
            }

            return Ok(notifications);
        }

        // تحديث حالة قراءة التنبيه
        [HttpPut("{notificationId}")]
        [Authorize]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null)
            {
                return NotFound("Notification not found.");
            }

            notification.IsRead = true;
            _context.Entry(notification).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Notification>> CreateNotification(NotificationModels notificationModel)
        {
            // تحويل NotificationModels إلى Notification
            var notification = new Notification
            {
                UserId = notificationModel.UserId,
                IdeaId = notificationModel.IdeaId,
                Message = notificationModel.Message,
                CreatedAt = notificationModel.CreatedAt,
                IsRead = notificationModel.IsRead
            };

            // إضافة التنبيه إلى قاعدة البيانات
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // إرسال التنبيه عبر SignalR
            await _hubContext.Clients.User(notification.UserId).SendAsync("ReceiveNotification", notification.Message);

            return CreatedAtAction(nameof(GetNotificationsByUserId), new { userId = notification.UserId }, notification);
        }

        // حذف تنبيه معين
        [HttpDelete("{notificationId}")]
        [Authorize]

        public async Task<IActionResult> DeleteNotification(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null)
            {
                return NotFound("Notification not found.");
            }

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

            // إرسال إشعار عبر SignalR بأنه تم حذف التنبيه
            await _hubContext.Clients.User(notification.UserId).SendAsync("NotificationDeleted", notificationId);

            return NoContent();
        }
    }
}
