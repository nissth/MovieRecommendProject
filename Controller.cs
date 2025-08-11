using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRecommendProject
{
    internal class Controller
    {
        private readonly IOpenRouterService _openRouterService;
        private readonly ITmdbService _tmdbService;
        private readonly View _view;

        public Controller(IOpenRouterService openRouterService, ITmdbService tmdbService, View view)
        {
            _openRouterService = openRouterService ?? throw new ArgumentNullException(nameof(openRouterService));
            _tmdbService = tmdbService ?? throw new ArgumentNullException(nameof(tmdbService));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public async Task Run()
        {
            _view.DisplayStart();

            var likedMovies = _view.GetLikedMoviesInput(); 

            if (string.IsNullOrWhiteSpace(likedMovies))
            {
                _view.DisplayErrorMessage("No movies entered. Exiting.");
                return;
            }

            _view.DisplayLoadingMessage();

            var recommendedTitles = await _openRouterService.GetRecommendations(likedMovies);

            if (recommendedTitles == null || recommendedTitles.Count == 0) 
            {
                _view.DisplayErrorMessage("Could not get recommendations from AI or no recommendations were found.");
                return;
            }

            _view.FetchingDetailsForRecommendedMoviesText();

            _view.DisplayRecommendedForYouText();

            foreach (var title in recommendedTitles)
            {
                var movie = await _tmdbService.SearchMovie(title);
                if (movie != null)
                {
                    _view.DisplayMovie(movie);
                    if (!string.IsNullOrEmpty(movie.PosterUrl) && _view.AskToOpenPoster(movie.Title))
                    {
                        // Open the poster URL in the default web browser
                        try
                        {
                            Console.WriteLine("Opening poster in browser...");
                            Process.Start(new ProcessStartInfo(movie.PosterUrl) { UseShellExecute = true }); 
                        }
                        catch (Exception ex)
                        {
                            _view.DisplayErrorMessage($"Could not open poster URL: {ex.Message}");
                        }
                    }
                }
                else
                {
                    _view.DisplayErrorMessage($"Could not find details for '{title}'.");
                }
                Console.WriteLine(); 
                await Task.Delay(500);
            }

            _view.PressAnyKeyToExit();
        }
    }
}
