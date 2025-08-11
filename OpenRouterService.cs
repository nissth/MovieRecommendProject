using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static MovieRecommendProject.Model;

namespace MovieRecommendProject
{
    internal class OpenRouterService : IOpenRouterService
    {
        private readonly HttpClient _httpClient;
        private readonly string _openRouterApiKey;
        private readonly string _modelName;

        public OpenRouterService(string openRouterApiKey, string modelName)
        {
            _openRouterApiKey = openRouterApiKey ?? throw new ArgumentNullException(nameof(openRouterApiKey));
            _modelName = modelName ?? throw new ArgumentNullException(nameof(modelName));

            _httpClient = new HttpClient 
            {
                BaseAddress = new Uri("https://openrouter.ai/api/v1/")
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openRouterApiKey);
            _httpClient.DefaultRequestHeaders.Add("HTTP-Referer", "http://localhost"); 
            _httpClient.DefaultRequestHeaders.Add("X-Title", "AI Movie Recommender"); 
        }
        public async Task<List<string>> GetRecommendations(string likedMovies) 
        {
            var request = new OpenRouterRequest
            {
                Model = _modelName,
                Messages = new List<OpenRouterMessage>
                {
                    new OpenRouterMessage { Role = "user", 
                        Content = $"I like these movies: {likedMovies}. Please recommend 3 other movies similar to these. Only list the movie titles, separated by commas. Do not include any other text or explanations." }
                }
            };

            try
            {
                var jsonRequest = JsonSerializer.Serialize(request); // Serialize the request object to JSON
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json"); // Set the content type to application/json

                var response = await _httpClient.PostAsync("chat/completions", content); // POST
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync(); // Read the response content as a string
                var openRouterResponse = JsonSerializer.Deserialize<OpenRouterResponse>(jsonResponse); // Deserialize the JSON response to OpenRouterResponse object

                if (openRouterResponse?.Choices != null && openRouterResponse.Choices.Count > 0) 
                {
                    // Get the content of the first choice's message
                    var recommendedText = openRouterResponse.Choices[0].Message.Content;

                    // Clean up the response to get just movie titles
                    var movieTitles = new List<string>();
                    foreach (var title in recommendedText.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) // Split the titles by comma
                    {
                        // Trim whitespace from each title
                        movieTitles.Add(title.Trim());
                    }
                    return movieTitles;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"\nError communicating with OpenRouter API: {e.Message}");
            }
            catch (JsonException e)
            {
                Console.WriteLine($"\nError parsing OpenRouter API response: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nAn unexpected error occurred in OpenRouterService: {e.Message}");
            }
            return null;
        }
    }
}
