using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static MovieRecommendProject.Model;

namespace MovieRecommendProject
{
    internal class TmdbService : ITmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _tmdbApiKey;
        private const string TmdbBaseImageUrl = "https://image.tmdb.org/t/p/w500"; 

        public TmdbService(string tmdbApiKey)
        {
            _tmdbApiKey = tmdbApiKey ?? throw new ArgumentNullException(nameof(tmdbApiKey));
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.themoviedb.org/3/")
            };
        }

        public async Task<Movie> SearchMovie(string movieTitle)
        {
            try
            {
                var encodedTitle = Uri.EscapeDataString(movieTitle); // Encode the movie title to ensure it is safe for use in a URL
                var url = $"search/movie?api_key={_tmdbApiKey}&query={encodedTitle}"; 

                var response = await _httpClient.GetAsync(url); // GET
                response.EnsureSuccessStatusCode(); 

                var jsonResponse = await response.Content.ReadAsStringAsync(); 
                var tmdbResponse = JsonSerializer.Deserialize<TmdbMovieSearchResponse>(jsonResponse);  

                if (tmdbResponse?.Results != null && tmdbResponse.Results.Count > 0) 
                {
                    // Take the first result as the most relevant
                    var firstResult = tmdbResponse.Results[0];
                    return new Movie
                    {
                        Title = firstResult.Title,
                        Overview = firstResult.Overview,
                        PosterUrl = !string.IsNullOrEmpty(firstResult.PosterPath) ? $"{TmdbBaseImageUrl}{firstResult.PosterPath}" : null
                    };
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"\nError communicating with TMDB API for '{movieTitle}': {e.Message}");
            }
            catch (JsonException e)
            {
                Console.WriteLine($"\nError parsing TMDB API response for '{movieTitle}': {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nAn unexpected error occurred in TmdbService for '{movieTitle}': {e.Message}");
            }
            return null;
        }
    }
}
