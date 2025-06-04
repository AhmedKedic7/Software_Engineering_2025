using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoeStoreAPI.Controllers
{
    [ApiController]
    [Route("api/recommendations")]
    public class RecommendationsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public RecommendationsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> GetRecommendations([FromBody] RecommendationRequest request)
        {
            var groqApiKey = "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"; // Store API key in configuration
            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", groqApiKey);

            var response = await client.PostAsJsonAsync("https://api.groq.com/recommendations", request);
            if (response.IsSuccessStatusCode)
            {
                var recommendations = await response.Content.ReadFromJsonAsync<RecommendationResponse>();
                return Ok(recommendations);
            }

            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }

    public record RecommendationRequest(string UserId, int Limit);
    public record RecommendationResponse(List<RecommendationItem> Recommendations);

    public record RecommendationItem(string Name, string Image, decimal Price);

}
