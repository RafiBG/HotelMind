using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using HotelMind.Services;

namespace HotelMind.Controllers
{
    public class SearchController : Controller
    {
        private readonly AIConnectionService _aiService;

        public SearchController(AIConnectionService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return RedirectToAction("Index");

            ViewBag.UserQuery = query;

            try
            {
                OpenAIPromptExecutionSettings settings = new()
                {
                    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                    Temperature = 0.1,
                    MaxTokens = 800
                };

                // STRONGER PROMPT: Explicitly tells the AI to use the tools and summarize findings.
                var chatHistory = new ChatHistory(@"You are a Hotel Booking Expert. 
            Search for hotels based on the user's budget and location.
            Even if you find only a few results, SUMMARIZE them. 
            Do not say there was an issue if the search tool returned links.
            Provide names, estimated prices, and why they fit the user's request.");

                chatHistory.AddUserMessage(query);
                Tools.SerperSearchTool.LatestLinks.Clear();

                var result = await _aiService.ChatService.GetChatMessageContentAsync(
                    chatHistory,
                    settings,
                    _aiService.Kernel);

                string finalResponse = result.Content ?? "I searched for options but couldn't generate a summary. Please see the sources below.";

                var links = Tools.SerperSearchTool.LatestLinks;
                if (links != null && links.Any())
                {
                    var uniqueLinks = links.Distinct().Take(5);
                    finalResponse += "\n\n---\n**Sources Found:**\n" + string.Join("\n", uniqueLinks.Select(l => $"* {l}"));
                }

                ViewBag.Response = finalResponse;
            }
            catch (Exception ex)
            {
                ViewBag.Response = $"❌ AI Search Error: {ex.Message}";
            }

            return View("Results");
        }

        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Message)) return BadRequest();
            try
            {
                OpenAIPromptExecutionSettings settings = new() { ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions, Temperature = 0.5 };
                var chatHistory = new ChatHistory("You are a helpful hotel assistant. Describe the vibe and answer follow-ups.");
                chatHistory.AddUserMessage(request.Message);

                var result = await _aiService.ChatService.GetChatMessageContentAsync(chatHistory, settings, _aiService.Kernel);
                return Ok(new { response = result.Content });
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        [HttpGet]
        public async Task<IActionResult> GetPackingAdvice(string city, string weather, double highTemp, double lowTemp)
        {
            // A more detailed prompt that considers the temperature range
            var prompt = $"The user is traveling to {city}. The forecast shows {weather} with highs of {highTemp}°C and lows of {lowTemp}°C. " +
                         "Suggest essential clothing items or accessories in short. You can add emojes  " +
                         "Return ONLY a comma-separated list. No conversational text.";

            try
            {
                var result = await _aiService.Kernel.InvokePromptAsync(prompt);
                return Ok(new { advice = result.ToString().Trim() });
            }
            catch { return Ok(new { advice = "👟 Walking Shoes, 🧥 Light Jacket, 🕶️ Sunglasses, 🔋 Power Bank" }); }
        }

        [HttpGet]
        public async Task<IActionResult> GetCityVibe(string city)
        {
            var prompt = $"Give a 2-sentence summary of the 'vibe' and 'top thing to do' in {city}. " +
                         "Be descriptive but very brief. No intro.";

            try
            {
                var result = await _aiService.Kernel.InvokePromptAsync(prompt);
                return Ok(new { vibe = result.ToString().Trim() });
            }
            catch { return Ok(new { vibe = $"Explore the local culture and hidden gems of {city}." }); }
        }

        public class ChatRequest { public string Message { get; set; } }
    }
}