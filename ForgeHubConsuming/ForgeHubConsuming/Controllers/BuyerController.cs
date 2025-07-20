using ForgeHubConsuming.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ForgeHubConsuming.Controllers
{
    public class BuyerController : Controller
    {


        HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:7128/api"; // change as needed

        public BuyerController()
        {
            
            HttpClientHandler clientHandler = new HttpClientHandler();

            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, error) =>
            {
                return true;
            };
            _httpClient = new HttpClient(clientHandler);
        }




        public IActionResult Index()
        {
            return View();
        }






        // GET: View Received Quotations
        public async Task<IActionResult> ReceivedQuotations(int buyerId)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/RFQQuotations/buyer/{buyerId}/received-quotations");

            if (!response.IsSuccessStatusCode) return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<ReceivedQuotationDto>>(json);

            return View(list);
        }

        // POST: Approve Quotation
        [HttpPost]
        public async Task<IActionResult> ApproveQuotation(int quotationId)
        {
            var response = await _httpClient.PutAsync($"{_apiBaseUrl}/RFQQuotations/{quotationId}/approve", null);
            return RedirectToAction("ReceivedQuotations", new { buyerId = 1 }); // Replace with actual buyerId
        }

        // POST: Reject Quotation
        [HttpPost]
        public async Task<IActionResult> RejectQuotation(int quotationId)
        {
            var response = await _httpClient.PutAsync($"{_apiBaseUrl}/RFQQuotations/{quotationId}/reject", null);
            return RedirectToAction("ReceivedQuotations", new { buyerId = 1 });
        }

        // GET: View Finalized Quotations
        public async Task<IActionResult> FinalizedQuotations(int buyerId)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/RFQQuotations/buyer/{buyerId}/finalized-quotations");

            if (!response.IsSuccessStatusCode) return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<FinalizedQuotationDto>>(json);

            return View(list);
        }




    }
}
