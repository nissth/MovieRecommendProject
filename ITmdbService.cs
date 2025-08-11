using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MovieRecommendProject.Model;

namespace MovieRecommendProject
{
    internal interface ITmdbService
    {
        Task<Movie> SearchMovie(string movieTitle); 
    }
}
