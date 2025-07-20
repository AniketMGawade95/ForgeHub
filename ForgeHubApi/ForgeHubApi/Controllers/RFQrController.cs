using ForgeHubApi.Data;
using ForgeHubApi.DTO;
using ForgeHubApi.Models;
using ForgeHubProj.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForgeHubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RFQrController : ControllerBase
    {
        ApplicationDbContext db;
        public RFQrController(ApplicationDbContext db)
        {
            this.db = db;
        }
        [HttpPost]
        [Route("Add")]
        public IActionResult Add(RFQDTO r)
        {
            var t = new RFQ()
            {
                RFQNo = r.RFQNo,
                IndentNo = r.IndentNo,
                RFQLineNo = r.RFQLineNo,
                ItemNo = r.ItemNo,
                ItemName = r.ItemName,
                ReqQty = r.ReqQty,
                Description = r.Description,
                DeliveryLocation = r.DeliveryLocation,
                UOM = r.UOM,
                ReqDeliveryDate = r.ReqDeliveryDate,
                FactoryCode = r.FactoryCode,
                BidDate = r.BidDate,
                ExpiryDateofBid = r.ExpiryDateofBid,
                BuyerId = r.BuyerId,
                Mobile = r.Mobile,
                ContactPerson = r.ContactPerson,
                Status = r.Status,
            };
            db.RFQs.Add(t);
            db.SaveChanges();
            return Ok("Added Successfull");
        }
        [HttpGet]
        [Route("Fetch")]
        public IActionResult fetch()
        {
            //var r = db.RFQs.ToList();  
            var t = db.RFQs.Include(x => x.Buyer).Select(x => new
            {
                x.RFQNo,
                x.IndentNo,
                x.RFQLineNo,
                x.ItemNo,
                x.ItemName,
                x.ReqQty,
                x.Description,
                x.DeliveryLocation,
                x.UOM,
                x.ReqDeliveryDate,
                x.FactoryCode,
                x.BidDate,
                x.ExpiryDateofBid,
                x.BuyerId,
                x.Mobile,
                x.ContactPerson,
                x.Status,
                buyer = x.Buyer.UserEmail,
            }).ToList();
            return Ok(t);
        }
        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var t = db.RFQs.Find(id);
            if (t != null)
            {
                db.RFQs.Remove(t);
                db.SaveChanges();
            }
            return Ok("Deleted Successful");
        }
        [HttpGet]
        [Route("Edit/{id}")]
        public IActionResult edit(int id)
        {
            var t = db.RFQs.Find(id);
            return Ok(t);
        }
        [HttpPut]
        [Route("Edit/{id}")]
        public IActionResult edit(int id, RFQDTO r)
        {

            //var t = db.RFQs.FirstOrDefault(i);
            var t = db.RFQs.Find(id);

            t.RFQNo = r.RFQNo;
            t.IndentNo = r.IndentNo;
            t.RFQLineNo = r.RFQLineNo;
            t.ItemNo = r.ItemNo;
            t.ItemName = r.ItemName;
            t.ReqQty = r.ReqQty;
            t.Description = r.Description;
            t.DeliveryLocation = r.DeliveryLocation;
            t.UOM = r.UOM;
            t.ReqDeliveryDate = r.ReqDeliveryDate;
            t.FactoryCode = r.FactoryCode;
            t.BidDate = r.BidDate;
            t.ExpiryDateofBid = r.ExpiryDateofBid;
            t.BuyerId = r.BuyerId;
            t.Mobile = r.Mobile;
            t.ContactPerson = r.ContactPerson;
            t.Status = r.Status;

            db.RFQs.Update(t);
            db.SaveChanges();
            return Ok("Edit Successfull");
        }
    }
}
