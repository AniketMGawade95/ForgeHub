using ForgeHubApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForgeHubProj.Models
{
    public class RFQQuotation
    {
        [Key]
        public int QuotationId { get; set; }

        [Required]
        public int RFQId { get; set; }

        [Required]
        public int VendorId { get; set; }

        public string? BidNo { get; set; }

        [Required]
        public decimal? QuotedAmount { get; set; }

        public DateTime DeliveryDate { get; set; }

        [Required]
        [MaxLength(200)]
        public string? PaymentTerms { get; set; }

        public string? Remarks { get; set; }

        [Required]
        public string? Status { get; set; } // Accepted, Rejected, Pending

        public DateTime? SubmittedDate { get; set; } = DateTime.Now;

        // Navigation
        [ForeignKey("RFQId")]
        public RFQ RFQ { get; set; }

        [ForeignKey("VendorId")]
        public Users Vendor { get; set; }

        public FinalizedQuotation FinalizedQuotation { get; set; }
    }

}
