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
                //  Check if the tool is registered
                var functions = _aiService.Kernel.Plugins.GetFunctionsMetadata();
                Console.WriteLine($"Available Functions: {functions.Count}");

                OpenAIPromptExecutionSettings settings = new()
                {
                    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                    Temperature = 0.1 // Lower temperature helps the AI focus on tool calls
                };

                var chatHistory = new ChatHistory("You are a helpful hotel assistant. To find current information, you MUST use the search tools available to you.");
                chatHistory.AddUserMessage(query);

                var result = await _aiService.ChatService.GetChatMessageContentAsync(
                    chatHistory,
                    settings,
                    _aiService.Kernel);

                // Check for empty content
                if (string.IsNullOrEmpty(result.Content))
                {
                    ViewBag.Response = "DEBUG: The AI triggered a search but failed to generate a text summary.";
                }
                else
                {
                    ViewBag.Response = result.Content;
                    Console.WriteLine("\n--- AI Response ---");
                    Console.WriteLine($"{result.Content}");
                    Console.WriteLine("---------------------\n");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Response = $"❌ ERROR DURING SEARCH: {ex.Message} \n\n Inner Exception: {ex.InnerException?.Message}";
                System.Diagnostics.Debug.WriteLine($"AI Error: {ex}");
            }

            return View("Results");
        }
    }
}