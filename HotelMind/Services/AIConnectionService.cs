using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using HotelMind.Tools;

namespace HotelMind.Services
{
    public class AIConnectionService
    {
        private readonly Kernel _kernel;

        public AIConnectionService(EnvConfig config)
        {
            var builder = Kernel.CreateBuilder();

            // Using the standard OpenAI connector configured for Ollama
            builder.AddOpenAIChatCompletion(
                modelId: config.MODEL,
                apiKey: config.API_KEY, // Ollama doesnt care, but the connector needs a string
                endpoint: new Uri(config.LOCAL_HOST) // e.g., "http://localhost:11434/v1"
            );

            // Register your Serper Tool
            var searchTool = new SerperSearchTool(config.SERPER_API_KEY);
            builder.Plugins.AddFromObject(searchTool, "HotelSearch");

            _kernel = builder.Build();
        }

        public Kernel Kernel => _kernel;
        public IChatCompletionService ChatService => _kernel.GetRequiredService<IChatCompletionService>();
    }
}