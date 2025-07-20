using ForgeHubApi.Data;
using ForgeHubApi.DTO;
using ForgeHubProj.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForgeHubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyerController : ControllerBase
    {


        private readonly ApplicationDbContext db;

        public BuyerController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // CREATE
        [HttpPost("CreateRFQ")]
        public async Task<ActionResult<RFQ>> CreateRFQ(RFQCreateDto dto)
        {
            var rfq = new RFQ
            {
                RFQNo = dto.RFQNo,
                IndentNo = dto.IndentNo,
                RFQLineNo = dto.RFQLineNo,
                ItemNo = dto.ItemNo,
                ItemName = dto.ItemName,
                ReqQty = dto.ReqQty,
                Description = dto.Description,
                DeliveryLocation = dto.DeliveryLocation,
                UOM = dto.UOM,
                ReqDeliveryDate = dto.ReqDeliveryDate,
                FactoryCode = dto.FactoryCode,
                BidDate = dto.BidDate ?? DateTime.Now,
                ExpiryDateofBid = dto.ExpiryDateofBid,
                BuyerId = dto.BuyerId,
                Mobile = dto.Mobile,
                ContactPerson = dto.ContactPerson,
                Status = dto.Status ?? "Open"
            };

            db.RFQs.Add(rfq);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRFQById), new { id = rfq.RFQId }, rfq);
        }



        // READ ALL
        [HttpGet("ListRFQ")]
        public async Task<ActionResult<IEnumerable<RFQDto>>> GetRFQs()
        {
            var rfqs = await db.RFQs
                .Include(r => r.Buyer)
                .Select(r => new RFQDto
                {
                    RFQId = r.RFQId,
                    RFQNo = r.RFQNo,
                    IndentNo = r.IndentNo,
                    ItemName = r.ItemName,
                    ReqQty = r.ReqQty,
                    ReqDeliveryDate = r.ReqDeliveryDate,
                    Status = r.Status,
                    BuyerEmail = r.Buyer.UserEmail
                }).ToListAsync();

            return Ok(rfqs);
        }




        // DELETE
        [HttpDelete("DeleteRFQ/{id}")]
        public async Task<IActionResult> DeleteRFQ(int id)
        {
            var rfq = await db.RFQs.FindAsync(id);
            if (rfq == null) return NotFound();

            db.RFQs.Remove(rfq);
            await db.SaveChangesAsync();

            return NoContent();
        }



        // READ SINGLE
        [HttpGet("FindRFQ/{id}")]
        public async Task<ActionResult<RFQ>> GetRFQById(int id)
        {
            var rfq = await db.RFQs.Include(r => r.Buyer).FirstOrDefaultAsync(r => r.RFQId == id);
            if (rfq == null) return NotFound();
            return Ok(rfq);
        }




        // UPDATE
        [HttpPut("UpdateRFQ/{id}")]
        public async Task<IActionResult> UpdateRFQ(int id, RFQUpdateDto dto)
        {
            if (id != dto.RFQId) return BadRequest("Mismatched RFQ ID");

            var rfq = await db.RFQs.FindAsync(id);
            if (rfq == null) return NotFound();

            rfq.RFQNo = dto.RFQNo;
            rfq.IndentNo = dto.IndentNo;
            rfq.RFQLineNo = dto.RFQLineNo;
            rfq.ItemNo = dto.ItemNo;
            rfq.ItemName = dto.ItemName;
            rfq.ReqQty = dto.ReqQty;
            rfq.Description = dto.Description;
            rfq.DeliveryLocation = dto.DeliveryLocation;
            rfq.UOM = dto.UOM;
            rfq.ReqDeliveryDate = dto.ReqDeliveryDate;
            rfq.FactoryCode = dto.FactoryCode;
            rfq.BidDate = dto.BidDate ?? rfq.BidDate;
            rfq.ExpiryDateofBid = dto.ExpiryDateofBid;
            rfq.BuyerId = dto.BuyerId;
            rfq.Mobile = dto.Mobile;
            rfq.ContactPerson = dto.ContactPerson;
            rfq.Status = dto.Status;

            await db.SaveChangesAsync();
            return NoContent();
        }










        [HttpGet("buyer/{buyerId}/received-quotations")]
        public async Task<ActionResult<IEnumerable<ReceivedQuotationDto>>> GetReceivedQuotations(int buyerId)
        {
            var quotations = await db.RFQQuotations
                .Include(q => q.Vendor)
                .Include(q => q.RFQ)
                .Where(q => q.RFQ.BuyerId == buyerId && q.Status == "Pending")
                .Select(q => new ReceivedQuotationDto
                {
                    QuotationId = q.QuotationId,
                    BidNo = q.BidNo,
                    QuotedAmount = q.QuotedAmount,
                    DeliveryDate = q.DeliveryDate,
                    PaymentTerms = q.PaymentTerms,
                    Remarks = q.Remarks,
                    SubmittedDate = q.SubmittedDate,
                    VendorId = q.VendorId,
                    VendorEmail = q.Vendor.UserEmail,
                    RFQId = q.RFQId,
                    RFQNo = q.RFQ.RFQNo,
                    ItemName = q.RFQ.ItemName
                })
                .ToListAsync();

            return Ok(quotations);
        }





        [HttpPost("approve/{quotationId}")]
        public async Task<IActionResult> ApproveQuotation(int quotationId)
        {
            var quotation = await db.RFQQuotations.Include(q => q.RFQ).FirstOrDefaultAsync(q => q.QuotationId == quotationId);

            if (quotation == null) return NotFound();

            // Reject others for this RFQ
            var others = db.RFQQuotations.Where(q => q.RFQId == quotation.RFQId && q.QuotationId != quotationId);
            foreach (var q in others)
                q.Status = "Rejected";

            quotation.Status = "Accepted";

            // Save finalized entry
            var finalized = new FinalizedQuotation
            {
                RFQId = quotation.RFQId,
                QuotationId = quotation.QuotationId,
                FinalizedDate = DateTime.Now
            };

            db.FinalizedQuotations.Add(finalized);
            await db.SaveChangesAsync();

            return Ok("Quotation Approved & Others Rejected.");
        }

        [HttpPost("reject/{quotationId}")]
        public async Task<IActionResult> RejectQuotation(int quotationId)
        {
            var quotation = await db.RFQQuotations.FindAsync(quotationId);
            if (quotation == null) return NotFound();

            quotation.Status = "Rejected";
            await db.SaveChangesAsync();

            return Ok("Quotation Rejected.");
        }



        [HttpGet("buyer/{buyerId}/finalized-quotations")]
        public async Task<ActionResult<IEnumerable<FinalizedQuotationDto>>> GetFinalizedQuotations(int buyerId)
        {
            var finalized = await db.FinalizedQuotations
                .Include(f => f.RFQ)
                .Include(f => f.RFQQuotation)
                    .ThenInclude(q => q.Vendor)
                .Where(f => f.RFQ.BuyerId == buyerId)
                .Select(f => new FinalizedQuotationDto
                {
                    FinalId = f.FinalId,
                    FinalizedDate = f.FinalizedDate,
                    QuotationId = f.QuotationId,
                    QuotedAmount = f.RFQQuotation.QuotedAmount,
                    PaymentTerms = f.RFQQuotation.PaymentTerms,
                    DeliveryDate = f.RFQQuotation.DeliveryDate,
                    VendorId = f.RFQQuotation.VendorId,
                    VendorEmail = f.RFQQuotation.Vendor.UserEmail,
                    RFQId = f.RFQId,
                    RFQNo = f.RFQ.RFQNo,
                    ItemName = f.RFQ.ItemName
                })
                .ToListAsync();

            return Ok(finalized);
        }





    }
}
