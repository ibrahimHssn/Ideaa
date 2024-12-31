using Ideaa.Models;
using Ideas.Data.Models;
using Ideas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ideas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public UserController(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModels RM)
        {
            if (string.IsNullOrWhiteSpace(RM.Password))
            {
                ModelState.AddModelError("Password", "Password is required.");
                return BadRequest(ModelState);
            }

            // التحقق من وجود المستخدم أولاً
            var existingUser = await _userManager.FindByNameAsync(RM.UserName);
            if (existingUser != null)
            {
                return Conflict("Username already exists.");
            }

            User newUser = new User
            {
                UserName = RM.UserName,
                Email = RM.Email,
                FirstName = RM.FirstName,
                LastName = RM.LastName,
                PhoneNumber = RM.PhoneNumber,
                Department = RM.Department,
                Validity = RM.Validity
            };

            var result = await _userManager.CreateAsync(newUser, RM.Password);

            if (result.Succeeded)
            {
                return Ok("User registered successfully.");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Password", error.Description);
                }
                return BadRequest(ModelState);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModels LM)
        {
            if (ModelState.IsValid)
            {
                User _user = await _userManager.FindByNameAsync(LM.UserName);
                if (_user == null)
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return Unauthorized(ModelState);
                }

                bool isPasswordValid = await _userManager.CheckPasswordAsync(_user, LM.Password);
                if (!isPasswordValid)
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return Unauthorized(ModelState);
                }

                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, _user.UserName),
                    new(ClaimTypes.NameIdentifier, _user.Id),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var roles = await _userManager.GetRolesAsync(_user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
                var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    claims: claims,
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddDays(90),
                    signingCredentials: signingCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new { token = "Bearer " + tokenString, expiration = token.ValidTo });
            }

            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserModel UM)
        {
            if (string.IsNullOrWhiteSpace(UM.UserId))
            {
                ModelState.AddModelError("UserId", "UserId is required.");
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(UM.UserId);
            if (user == null)
            {
                ModelState.AddModelError("UserId", "User not found.");
                return NotFound(ModelState);
            }

            // تحديث بيانات المستخدم هنا
            user.FirstName = UM.FirstName ?? user.FirstName;
            user.LastName = UM.LastName ?? user.LastName;
            user.PhoneNumber = UM.PhoneNumber ?? user.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok("User updated successfully.");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("UpdateError", error.Description);
                }
                return BadRequest(ModelState);
            }
        }

 
        [Authorize]
        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordModel model)
        {
            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                ModelState.AddModelError("UserName", "UserName is required.");
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(model.CurrentPassword))
            {
                ModelState.AddModelError("CurrentPassword", "Current password is required.");
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(model.NewPassword))
            {
                ModelState.AddModelError("NewPassword", "New password is required.");
                return BadRequest(ModelState);
            }

            // البحث عن المستخدم باستخدام اسم المستخدم
            User _user = await _userManager.FindByNameAsync(model.UserName);
            if (_user == null)
            {
                return NotFound("User not found.");
            }

            // التحقق من صحة كلمة المرور الحالية
            var passwordValid = await _userManager.CheckPasswordAsync(_user, model.CurrentPassword);
            if (!passwordValid)
            {
                ModelState.AddModelError("CurrentPassword", "Current password is incorrect.");
                return BadRequest(ModelState);
            }

            // تغيير كلمة المرور
            var result = await _userManager.ChangePasswordAsync(_user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok("Password updated successfully.");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Password", error.Description);
                }
                return BadRequest(ModelState);
            }
        }


        [Authorize]
        [HttpGet("GetUserById/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                ModelState.AddModelError("UserId", "UserId is required.");
                return BadRequest(ModelState);
            }

            // البحث عن المستخدم باستخدام UserId مع تحميل الـ Notifications المرتبطة
            User _user = await _userManager.Users
                .Include(u => u.Notifications)  // تضمين البيانات المرتبطة بـ Notifications
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (_user == null)
            {
                return NotFound("User not found.");
            }

            // التحقق إذا كانت Notifications فارغة أو null
            var notifications = _user.Notifications != null
                ? _user.Notifications.Select(n => new Notification
                {
                    NotificationId = n.NotificationId,
                    Message = n.Message,
                    CreatedAt = n.CreatedAt,
                    IsRead = n.IsRead
                }).ToList()
                : new List<Notification>(); // إرجاع قائمة فارغة من نوع NotificationDto إذا كانت null

            // يمكن إرجاع المستخدم مع بياناته
            var userDto = new
            {
                _user.Id,
                _user.FirstName,
                _user.LastName,
                _user.UserName,
                _user.Email,
                _user.PhoneNumber,
                _user.Department,
                _user.Validity,
                _user.CreatedAt,
                _user.UnitsNo,
                _user.Capacity,
                Notifications = notifications // استخدام المتغير الذي تم التأكد من قيمته
            };

            return Ok(new { data = userDto });
        }


        [Authorize]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            // جلب جميع المستخدمين مع الإشعارات المرتبطة بهم
            var users = await _userManager.Users
                                           .Include(u => u.Notifications)  // تضمين الإشعارات المرتبطة بالمستخدم
                                           .ToListAsync();

            if (users == null || !users.Any())
            {
                return NotFound("No users found.");
            }

            // تحويل البيانات إلى صيغة مناسبة (DTO) لإرسالها إلى الواجهة
            var usersDto = users.Select(user => new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.UserName,
                user.Email,
                user.PhoneNumber,
                user.Department,
                user.Validity,
                user.CreatedAt,
                user.UnitsNo,
                user.Capacity,
                //Notifications = user.Notifications.Select(n => new
                //{
                //    n.NotificationId,
                //    n.Message,
                //    n.CreatedAt,
                //    n.IsRead  
                //}).ToList()
            }).ToList();

            return Ok(new { data = usersDto });
        }

   


    }

}



