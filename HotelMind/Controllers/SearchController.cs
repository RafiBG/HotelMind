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
                // Clear previous search results to ensure fresh data for each query
                Tools.SerperSearchTool.LatestLinks.Clear();

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
                    // Debug in console
                    Console.WriteLine("\n--- AI Response ---");
                    Console.WriteLine($"{result.Content}");
                    Console.WriteLine("---------------------\n");

                    var links = Tools.SerperSearchTool.LatestLinks;
                    string finalResponse = result.Content;

                    if (links != null && links.Any())
                    {
                        // Format the links nicely
                        var uniqueLinks = links.Distinct().Take(5); // Remove duplicates and limit to 5
                        finalResponse += "\n\n---\n**Sources:**\n" + string.Join("\n", uniqueLinks.Select(l => $"* {l}"));
                    }

                    ViewBag.Response = finalResponse;
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