using ForgeHubConsuming.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json;

namespace ForgeHubConsuming.Controllers
{
    public class LoginController : Controller
    {
        HttpClient client;

        public LoginController()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();

            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, error) =>
            {
                return true;
            };
            client = new HttpClient(clientHandler);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult AdminDashboard()
        {
            return View(); // Add corresponding AdminDashboard.cshtml view
        }

        public IActionResult VendorDashboard()
        {
            return View(); // Add corresponding VendorDashboard.cshtml view
        }

        public IActionResult UserDashboard()
        {
            return View(); // Add corresponding UserDashboard.cshtml view
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            string url = "https://localhost:7128/api/Auth/Login";

            HttpResponseMessage res = await client.PostAsJsonAsync(url, new
            {
                userEmail = lvm.UserEmail,
                password = lvm.Password
            });

            if (!res.IsSuccessStatusCode)
            {
                lvm.ErrorMessage = "Invalid Credentials";
                return View(lvm);
            }

            var result = await res.Content.ReadFromJsonAsync<JsonElement>();

            if (result.TryGetProperty("requireGoogleAuth", out var requireGoogleAuth) && requireGoogleAuth.GetBoolean())
            {
                var userId = result.GetProperty("userId").GetInt32();
                HttpContext.Session.SetString("UserId", userId.ToString());
                return RedirectToAction("GoogleAuth");
            }

            var token = result.GetProperty("token").GetString();
            var email = result.GetProperty("email").GetString();
            var role = result.GetProperty("role").GetString();

            // ✅ Store in session
            HttpContext.Session.SetString("JWToken", token);
            HttpContext.Session.SetString("UserEmail", email);
            HttpContext.Session.SetString("UserRole", role);

            // ✅ Redirect based on role
            switch (role)
            {
                case "Admin":
                    return RedirectToAction("AdminDashboard");
                case "Vendor":
                    return RedirectToAction("VendorDashboard");
                case "User":
                    return RedirectToAction("UserDashboard");
                default:
                    return RedirectToAction("Index"); 
            }
        }


        [HttpGet]
        public async Task<IActionResult> GoogleAuth()
        {
            var userId = HttpContext.Session.GetString("UserId");

            string url = $"https://localhost:7128/api/Auth/generateQr/{userId}";

            var response = await client.GetFromJsonAsync<JsonElement>(url);
            var otpurl = response.GetProperty("otpauthUrl").GetString();

            ViewBag.OtpAuthUrl = otpurl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyCode(string code)
        {
            var userId = HttpContext.Session.GetString("UserId");

            string url = "https://localhost:7128/api/Auth/verifyCode";
            var response = await client.PostAsJsonAsync(url, new
            {
                userId = userId,
                code = code
            });

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMessage = "Invalid code. Try again.";
                return View("GoogleAuth");
            }

            return RedirectToAction("Index");
        }

        public IActionResult LostCode()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LostCode(LoginViewModel lvm)
        {
            string url = "https://localhost:7128/api/Auth/CodeLost";
            var response = await client.PostAsJsonAsync(url, new
            {
                userEmail = lvm.UserEmail,
                password = lvm.Password
            });

            if (!response.IsSuccessStatusCode)
            {
                lvm.ErrorMessage = "Invalid credentials for reset.";
                return View("Login", lvm);
            }

            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            var userId = result.GetProperty("userId").GetInt32();

            HttpContext.Session.SetString("UserId", userId.ToString());
            return RedirectToAction("GoogleAuth");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Login");
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
