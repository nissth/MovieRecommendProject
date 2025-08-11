using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieRecommendProject
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string openRouterApiKey = config.AppSettings.Settings["OpenRouter_ApiKey"].Value;
            string tmdbApiKey = config.AppSettings.Settings["TMDB_ApiKey"].Value;
            string modelName = config.AppSettings.Settings["ModelName"].Value;
            
            var openRouterService = new OpenRouterService(openRouterApiKey, modelName); 
            var tmdbService = new TmdbService(tmdbApiKey);
            var view = new View();
            var controller = new Controller(openRouterService, tmdbService, view);
            await controller.Run(); 
        }
    }
}
