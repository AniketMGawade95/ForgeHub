namespace ForgeHubApi.DTO
{
    public class RFQDto
    {
        public int RFQId { get; set; }
        public string RFQNo { get; set; }
        public string IndentNo { get; set; }
        public string? ItemName { get; set; }
        public int? ReqQty { get; set; }
        public DateTime? ReqDeliveryDate { get; set; }
        public string? Status { get; set; }
        public string BuyerEmail { get; set; } // From Users
    }
}
