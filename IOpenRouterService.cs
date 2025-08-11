using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRecommendProject
{
    internal interface IOpenRouterService
    {
        Task<List<string>> GetRecommendations(string likedMovies); 
    }
}