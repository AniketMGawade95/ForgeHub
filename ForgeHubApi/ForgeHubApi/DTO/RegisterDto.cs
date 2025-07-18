using System.ComponentModel.DataAnnotations;

namespace ForgeHubApi.DTO
{
    public class RegisterDto
    {
        public string UserEmail { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}
