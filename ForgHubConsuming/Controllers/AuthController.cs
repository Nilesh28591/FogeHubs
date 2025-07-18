using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using ForgHubConsuming.Dto;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace ForgHubConsuming.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public AuthController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View("Login");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegistorDto registorDto)
        {
            try
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                var client = new HttpClient(handler);
                var apiUrl = "https://localhost:7149/api/Authentication/Registration";

                // Fix for CS1501 and CS0815: Use System.Text.Json.JsonSerializer.Serialize with correct overload
                string json = System.Text.Json.JsonSerializer.Serialize(registorDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Registration successful!" });
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = $"API Error: {error}" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Exception: {ex.Message}" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> SubmitLogin([FromBody] LoginDto login)
        {
            try
            {
                // Allow untrusted SSL for development
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                var client = new HttpClient(handler);
                var apiUrl = "https://localhost:7149/api/Authentication/login";

                var requestContent = new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        email = login.Email,
                        password = login.Password
                    }), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    return Ok(token);
                }

                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }
            catch (Exception ex)
            {
                return BadRequest("Exception: " + ex.Message);
            }
        }
    }
}
