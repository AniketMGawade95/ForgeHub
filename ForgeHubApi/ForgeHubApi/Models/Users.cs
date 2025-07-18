using ForgeHubProj.Models;
using System.ComponentModel.DataAnnotations;

namespace ForgeHubApi.Models
{
    public class Users
    {
        [Key]
        [Required]
        public int UserId { get; set; }

        [Required]
        public string UserEmail { get; set; }

        [Required]
        public string Password { get; set; }

        public string Role { get; set; }

        public bool RgStatus { get; set; }
        public string? GoogleAuthSecretKey { get; set; }


        // Navigation properties
        public List<RFQ> RFQs { get; set; } // For Buyers
        public List<RFQQuotation> RFQQuotations { get; set; } // For Vendors
    }
}
