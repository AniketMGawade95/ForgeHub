using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ForgeHubProj.Models
{
    public class FinalizedQuotation
    {
        [Key]
        public int FinalId { get; set; }

        [Required]
        public int RFQId { get; set; }

        [Required]
        public int QuotationId { get; set; }

        public DateTime? FinalizedDate { get; set; } = DateTime.Now;

        // Navigation
        [ForeignKey("RFQId")]
        public RFQ RFQ { get; set; }

        [ForeignKey("QuotationId")]
        public RFQQuotation RFQQuotation { get; set; }
    }

}
