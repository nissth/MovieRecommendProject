using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MovieRecommendProject
{
    internal class Model
    {
        public class Movie {
            public string Title { get; set; }
            public string Overview { get; set; }
            public string PosterUrl { get; set; }
        }

        public class OpenRouterRequest 
        {
            [JsonPropertyName("model")] 
            public string Model { get; set; } = ""; 

            [JsonPropertyName("messages")] 
            public List<OpenRouterMessage> Messages { get; set; } = new List<OpenRouterMessage>();
        }

        public class OpenRouterMessage 
        {
            [JsonPropertyName("role")]
            public string Role { get; set; } 

            [JsonPropertyName("content")] 
            public string Content { get; set; } 
        }

        public class OpenRouterResponse
        {
            [JsonPropertyName("choices")]
            // List of choices returned by the AI model
            public List<OpenRouterChoice> Choices { get; set; } = new List<OpenRouterChoice>(); 
        }

        public class OpenRouterChoice
        {
            [JsonPropertyName("message")]
            // The message containing the AI's response
            public OpenRouterMessage Message { get; set; } 
        }

        public class TmdbMovieSearchResponse
        {
            [JsonPropertyName("results")]
            // List of movie results returned by TMDB API
            public List<TmdbMovieResult> Results { get; set; } = new List<TmdbMovieResult>(); 
        }

        public class TmdbMovieResult
        {
            [JsonPropertyName("title")]
            public string Title { get; set; }

            [JsonPropertyName("overview")]
            public string Overview { get; set; }

            [JsonPropertyName("poster_path")]
            public string PosterPath { get; set; }
        }
    }
}
