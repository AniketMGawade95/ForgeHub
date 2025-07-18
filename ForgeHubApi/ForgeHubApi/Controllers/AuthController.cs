using ForgeHubApi.Data;
using ForgeHubApi.DTO;
using ForgeHubApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OtpNet;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ForgeHubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        private readonly IConfiguration config;

        public AuthController(ApplicationDbContext db, IConfiguration config)
        {
            this.config = config;
            this.db = db;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginDTO log)
        {
            var user = db.Users.FirstOrDefault(u => u.UserEmail == log.UserEmail && u.Password == log.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "User not found" });
            }

            if (!user.RgStatus)
            {
                return Ok(new
                {
                    message = "Google Authenticator setup required",
                    requireGoogleAuth = true,
                    userId = user.UserId
                });
            }

            var token = GenJwtToken(user);
            return Ok(new { token });
        }

        [HttpPost]
        [Route("Register")]

        public IActionResult Register(RegisterDto dto)
        {
            var result = new Users()
            {
                UserEmail = dto.UserEmail,
                Password = dto.Password,
                Role = dto.Role
            };
            db.Users.Add(result);
            db.SaveChanges();
            return Ok("success");
        }

        private string GenJwtToken(Users users)
        {
            var jwtKey = config["Jwt:Key"];
            var jwtIssuer = config["Jwt:Issuer"];

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, users.UserEmail),
                new Claim("role", users.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtIssuer,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet]
        [Route("generateQr/{userId}")]
        public IActionResult GenerateQr(int userId)
        {
            var user = db.Users.Find(userId);
            if (user == null)
            {
                return NotFound("User Not Found");
            }

            if (string.IsNullOrEmpty(user.GoogleAuthSecretKey))
            {
                var key = KeyGeneration.GenerateRandomKey(20);
                var base32Secret = Base32Encoding.ToString(key);
                user.GoogleAuthSecretKey = base32Secret;
                db.SaveChanges();
            }

            var issuer = "ForgeHub";
            var label = user.UserEmail;

            var otpauthUrl = $"otpauth://totp/{Uri.EscapeDataString(issuer)}:{Uri.EscapeDataString(label)}?secret={user.GoogleAuthSecretKey}&issuer={Uri.EscapeDataString(issuer)}&digits=6";

            return Ok(new { otpauthUrl });
        }

        [HttpPost]
        [Route("verifyCode")]
        public IActionResult VerifyCode(VerifyCodeDTO verify)
        {
            var user = db.Users.Find(verify.UserId);
            if (user == null)
                return NotFound("User not found");

            if (string.IsNullOrEmpty(user.GoogleAuthSecretKey))
            {
                return BadRequest("Google authenticator is not setup yet");
            }

            var totp = new Totp(Base32Encoding.ToBytes(user.GoogleAuthSecretKey));
            var isValid = totp.VerifyTotp(verify.code, out long timeWindowUser, VerificationWindow.RfcSpecifiedNetworkDelay);

            if (!isValid) return BadRequest("Invalid code");

            user.RgStatus = true;
            db.SaveChanges();

            return Ok("Google Authenticator setup complete");
        }

        [HttpPost]
        [Route("CodeLost")]
        public IActionResult LostCode(LoginDTO log)
        {
            var user = db.Users.FirstOrDefault(u => u.UserEmail == log.UserEmail && u.Password == log.Password);

            if (user == null) return NotFound("User not found");

            user.RgStatus = false;
            user.GoogleAuthSecretKey = null;

            db.SaveChanges();

            GenerateQr(user.UserId);
            return Ok(new
            {
                message = "Google Authenticator reset. Please scan the new QR code.",
                userId = user.UserId
            });
        }


        [HttpGet("me")]
        [Authorize] // Requires JWT token
        public IActionResult GetCurrentUserProfile()
        {
            // Retrieve email from JWT claims
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? User.FindFirst(ClaimTypes.Email)?.Value
                          ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userEmail == null)
                return Unauthorized("Email not found in token.");

            var user = db.Users
                .Where(u => u.UserEmail == userEmail)
                .Select(u => new UserprofileDto
                {
                    UserEmail = u.UserEmail,
                    Role = u.Role
                })
                .FirstOrDefault();

            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }
    }
}
   
