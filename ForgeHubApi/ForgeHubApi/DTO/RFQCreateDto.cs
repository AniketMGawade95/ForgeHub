namespace ForgeHubApi.DTO
{
    public class RFQCreateDto
    {
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
        public DateTime? BidDate { get; set; }
        public DateTime? ExpiryDateofBid { get; set; }
        public int BuyerId { get; set; }
        public string? Mobile { get; set; }
        public string? ContactPerson { get; set; }
        public string? Status { get; set; } = "Open";
    }
}
