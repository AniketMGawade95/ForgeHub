using System.Text;
using ForgeHubConsuming.DTO;
using ForgeHubConsuming.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ForgeHubConsuming.Controllers
{
    public class BidbController : Controller
    {
        HttpClient client;
        public BidbController()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            client = new HttpClient(clientHandler);
        }

        public IActionResult Index()
        {
            List<RFQDTO> data = new List<RFQDTO>();
            string url = "https://localhost:7128/api/RFQr/Fetch";

            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<List<RFQDTO>>(json);
                if (obj != null)
                {
                    data = obj;
                }
            }
            // ViewBag.managers = new SelectList(data, "mid", "mname");
            return View(data);
        }
        public IActionResult AddRFq()
        {
            List<RFQDTO> data = new List<RFQDTO>();
            string url = "https://localhost:7128/api/RFQr/Fetch";

            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<List<RFQDTO>>(json);
                if (obj != null)
                {
                    data = obj;
                }
            }
            //ViewBag.managers = new SelectList(data, "mid", "mname");
            return View();
        }

        [HttpPost]
        public IActionResult AddRFq(RFQDTO em)
        {
            string url = "https://localhost:7128/api/RFQr/Add";
            var json = JsonConvert.SerializeObject(em);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult Delete(int id)
        {
            string url = $"https://localhost:7128/api/RFQr/Delete/{id}";
            HttpResponseMessage response = client.DeleteAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }


            return RedirectToAction("Index");
        }

        //public IActionResult Edit(int id)
        //{
        //    RFQDTO data = new RFQDTO();
        //    string url = "https://localhost:7128/api/RFQr/Fetch/{id}";

        //    HttpResponseMessage response = client.GetAsync(url).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var json = response.Content.ReadAsStringAsync().Result;
        //        var obj = JsonConvert.DeserializeObject<RFQDTO>(json);
        //        if (obj != null)
        //        {
        //            data = obj;
        //        }
        //    }
        //    // ViewBag.managers = new SelectList(data, "mid", "mname");
        //    return View(data);
        //}
        //[HttpPost]
        //public IActionResult Edit(RFQDTO em)
        //{
        //    string url = "https://localhost:7128/api/RFQr/Edit/{em.RFQId}";
        //    var json = JsonConvert.SerializeObject(em);
        //    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        //    HttpResponseMessage response = client.PostAsync(url, content).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}
        [HttpGet]
        public IActionResult Edit(int id)
        {
            RFQDTO data = new RFQDTO();
            string url = "https://localhost:7128/api/RFQr/Edit/";

            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var jason = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<RFQDTO>(jason);
                if (obj != null)
                {
                    data = obj;
                }
            }
            return View(data);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(RFQDTO m)
        {

            string url = $"https://localhost:7128/api/RFQr/Edit/{m.RFQId}";
            var jason = JsonConvert.SerializeObject(m);
            StringContent content = new StringContent(jason, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult View(int id)
        {
            RFQDTO data = new RFQDTO();
            string url = "https://localhost:7128/api/RFQr/Edit/";

            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var jason = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<RFQDTO>(jason);
                if (obj != null)
                {
                    data = obj;
                }
            }
            return View(data);
        }
    }
}
