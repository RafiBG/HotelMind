using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Text;
using System.Text.Json;

namespace HotelMind.Tools
{
    public class SerperSearchTool
    {
        private readonly string _apiKey;
        private readonly HttpClient _http;
        public static List<string> LatestLinks { get; private set; } = new();

        public SerperSearchTool(string apiKey)
        {
            _apiKey = apiKey;
            _http = new HttpClient();

        }

        [KernelFunction("search_hotels")]
        [Description("Searches the web for hotels, prices, and reviews in a specific location.")]
        public async Task<string> SearchAsync([Description("The search query, e.g., 'hotels in Varna under $100'")] string query)
        {
            LatestLinks.Clear(); // Clear previous links for each new search

            if (string.IsNullOrWhiteSpace(_apiKey)) return "Serper API key is missing.";

            // Use the 'search' endpoint but add 'hotels' to the query if not present
            string searchHost = "https://google.serper.dev/search";

            var payload = new { q = query, gl = "bg", hl = "en" };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, searchHost);
            request.Headers.Add("X-API-KEY", _apiKey);
            request.Content = content;

            try
            {
                var response = await _http.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"\n--- SERPER DEBUG ---");
                Console.WriteLine($"Query: {query}");
                Console.WriteLine($"Response Length: {responseBody.Length}");
                Console.WriteLine("----------------------\n");

                if (!response.IsSuccessStatusCode) return $"Error: {response.StatusCode}";

                using var doc = JsonDocument.Parse(responseBody);
                var sb = new StringBuilder();

                // Check for "Knowledge Graph" ( direct hotel info)
                if (doc.RootElement.TryGetProperty("knowledgeGraph", out var kg))
                {
                    sb.AppendLine("### Direct Match:");
                    if (kg.TryGetProperty("title", out var title)) sb.AppendLine($"Hotel: {title.GetString()}");
                    if (kg.TryGetProperty("description", out var desc)) sb.AppendLine($"Info: {desc.GetString()}");
                }

                // Extract Organic Results
                if (doc.RootElement.TryGetProperty("organic", out var organic))
                {
                    sb.AppendLine("\n### Web Results:");
                    foreach (var item in organic.EnumerateArray().Take(5)) // Take top 5 for speed
                    {
                        var title = item.GetProperty("title").GetString();
                        var snippet = item.GetProperty("snippet").GetString();
                        var link = item.GetProperty("link").GetString();

                        sb.AppendLine($"- **{title}**: {snippet} ({link})");
                        LatestLinks.Add(link);
                    }
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return $"Search failed: {ex.Message}";
            }
        }
    }
}
