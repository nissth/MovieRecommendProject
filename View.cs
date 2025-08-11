using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MovieRecommendProject.Model;

namespace MovieRecommendProject
{
    internal class View
    {
        public void DisplayStart() {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(@"
            ╔══════════════════════════════════════════════════════════════╗
            ║                    MOVIE RECOMMENDATION                      ║
            ║                          SYSTEM                              ║
            ╚══════════════════════════════════════════════════════════════╝
                    ");
            Console.ResetColor();
        }


        public string GetLikedMoviesInput()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Please enter the movie titles with using commas.");
            Console.WriteLine("Ex: \"Dead Poets Society, Inception\"\n");
            Console.Write("Enter title/s : ");
            Console.ResetColor();
            return Console.ReadLine();
        }

        public void DisplayErrorMessage(string message)
        {
            Console.WriteLine($"[ERROR] {message}");
        }

        public void FetchingDetailsForRecommendedMoviesText()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n--- Fetching details for recommended movies ---");
            Console.ResetColor();
        }

        public void DisplayRecommendedForYouText() {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"\n            ╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"            ║                   RECOMMENDED FOR YOU                        ║");
            Console.WriteLine($"            ╚══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        public void DisplayMovie(Movie movie)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"\n--- Recommended Movie: {movie.Title} ---\n");
            Console.WriteLine($"Overview: {movie.Overview}");
            Console.WriteLine("------------------------------------");
            Console.ResetColor();
        }

        public void DisplayLoadingMessage()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nGenerating recommendations and fetching movie details... Please wait.");
            Console.ResetColor();
        }
        public bool AskToOpenPoster(string movieTitle)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"Open the poster for '{movieTitle}' in your browser? (y/n): ");
            Console.ResetColor();
            var response = Console.ReadLine()?.Trim().ToLower();
            return response == "y" || response == "yes";
        }


        public void PressAnyKeyToExit()
        {
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }

    }
}
