using ADOCRUD.Models;
using ChatGpt.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace ADOCRUD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;


        public HomeController(ILogger<HomeController> logger, IConfiguration  configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ChatBotInput(string prompt)
        {
            string text = string.Empty;
            try
            {
                var requestBody = new
                {
                    contents = new[]
               {
                    new
                    {
                        parts = new[]
                        {
                            new { text = $"{prompt}" }
                        }
                    }
                }
                };

                string jsonBody = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                using (var client = new HttpClient())
                {
                    var url = _configuration["Gemini:URL"];
                    var APIKEY = _configuration["Gemini:APIKEY"];
                    var response = await client.PostAsync(url + APIKEY, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        RootObject responseObject = JsonConvert.DeserializeObject<RootObject>(responseBody);
                        if (responseObject != null && responseObject.Candidates.Count > 0)
                        {
                            text = responseObject.Candidates[0].Content.Parts[0].Text;
                        }
                        return Json(text);
                    }
                    else
                    {
                        throw new Exception($"Error calling Gemini API: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
