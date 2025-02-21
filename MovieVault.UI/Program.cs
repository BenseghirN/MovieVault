using Microsoft.Extensions.DependencyInjection;
using MovieVault.Core.TMDB;
using MovieVault.UI.Configuration;

namespace MovieVault.UI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            var serviceProvider = DependencyInjection.ConfigureServices();

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1(serviceProvider.GetRequiredService<ITmdbService>()));
        }
    }
}