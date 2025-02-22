using Microsoft.Extensions.DependencyInjection;
using MovieVault.UI.Configuration;
using MovieVault.UI.Forms;
using System.Diagnostics;

namespace MovieVault.UI
{
    internal static class Program
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            if (Debugger.IsAttached)
            {
                AllocConsole();
            }

            var serviceProvider = DependencyInjection.ConfigureServices();

            ApplicationConfiguration.Initialize();
            var mainForm = serviceProvider.GetRequiredService<MainForm>();
            Application.Run(mainForm);
        }
    }
}