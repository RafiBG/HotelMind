namespace HotelMind.Services
{
    public class EnvConfig
    {
        public string MODEL { get; private set; }
        public string LOCAL_HOST { get; private set; }
        public string API_KEY { get; private set; }
        public string SERPER_API_KEY { get; private set; }
        public string WEATHER_API_KEY { get; private set; }

        public EnvConfig()
        {
            // Load .env file
            DotNetEnv.Env.Load();

            // Assign values with fallbacks
            MODEL = Environment.GetEnvironmentVariable("AI_MODEL") ?? "llama3";
            LOCAL_HOST = Environment.GetEnvironmentVariable("AI_ENDPOINT") ?? "http://localhost:11434/v1";
            API_KEY = Environment.GetEnvironmentVariable("AI_API_KEY") ?? "unused";
            SERPER_API_KEY = Environment.GetEnvironmentVariable("SERPER_API_KEY") ?? "";
            WEATHER_API_KEY = Environment.GetEnvironmentVariable("WEATHER_API_KEY") ?? "";
        }
    }
}
