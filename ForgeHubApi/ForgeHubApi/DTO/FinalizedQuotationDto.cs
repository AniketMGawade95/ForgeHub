namespace ForgeHubApi.DTO
{
    public class FinalizedQuotationDto
    {
        public int FinalId { get; set; }
        public DateTime? FinalizedDate { get; set; }

        // Quotation Info
        public int QuotationId { get; set; }
        public decimal? QuotedAmount { get; set; }
        public string PaymentTerms { get; set; }
        public DateTime DeliveryDate { get; set; }

        // Vendor Info
        public int VendorId { get; set; }
        public string VendorEmail { get; set; }

        // RFQ Info
        public int RFQId { get; set; }
        public string RFQNo { get; set; }
        public string ItemName { get; set; }
    }
}
