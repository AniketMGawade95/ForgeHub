namespace ForgeHubConsuming.Models
{
    public class FinalizedQuotationDto
    {
        public int FinalId { get; set; }
        public DateTime? FinalizedDate { get; set; }

        public int QuotationId { get; set; }
        public decimal? QuotedAmount { get; set; }
        public string PaymentTerms { get; set; }
        public DateTime DeliveryDate { get; set; }

        public int VendorId { get; set; }
        public string VendorEmail { get; set; }

        public int RFQId { get; set; }
        public string RFQNo { get; set; }
        public string ItemName { get; set; }
    }
}
