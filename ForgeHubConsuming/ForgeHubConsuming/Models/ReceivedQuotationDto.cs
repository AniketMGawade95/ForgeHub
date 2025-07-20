namespace ForgeHubConsuming.Models
{
    public class ReceivedQuotationDto
    {
        public int QuotationId { get; set; }
        public string BidNo { get; set; }
        public decimal? QuotedAmount { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string PaymentTerms { get; set; }
        public string Remarks { get; set; }
        public DateTime? SubmittedDate { get; set; }

        public int VendorId { get; set; }
        public string VendorEmail { get; set; }

        public int RFQId { get; set; }
        public string RFQNo { get; set; }
        public string ItemName { get; set; }
    }
}
