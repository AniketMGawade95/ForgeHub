using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ForgeHubConsuming.Models
{
    public class RFQ
    {
        [Key]
        public int RFQId { get; set; }
        public string RFQNo { get; set; }
        public string IndentNo { get; set; }
        public string? RFQLineNo { get; set; }
        public string? ItemNo { get; set; }
        public string? ItemName { get; set; }
        public int? ReqQty { get; set; }
        public string? Description { get; set; }
        public string? DeliveryLocation { get; set; }
        public string? UOM { get; set; }
        public DateTime? ReqDeliveryDate { get; set; }
        public string? FactoryCode { get; set; }
        public DateTime? BidDate { get; set; } = DateTime.Now;
        public DateTime? ExpiryDateofBid { get; set; }
        [Required]

        //[ForeignKey("Users")]
        public int BuyerId { get; set; }
        //public User Users { get; set; }
        public string? Mobile { get; set; }
        public string? ContactPerson { get; set; }
        [Required]
        public string? Status { get; set; } = "Open";
        //public List<RFQQuotation> RFQQuotations { get; set; }
        //public FinalizedQuotation FinalizedQuotation { get; set; }
    }
}
