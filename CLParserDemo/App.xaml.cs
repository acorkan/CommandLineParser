using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace CLParserDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;
        private IConfiguration _configuration;


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Create the configuration from appsettings.json
            var configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).
             AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _configuration = configurationBuilder.Build();

            // Set up the DI service collection.
            var serviceCollection = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();

            var clParser = _serviceProvider.GetRequiredService<CommandLineParser.CLParser>();

            clParser.Add(new Option<string>("--name") { Description = "Your name" });

            Option<bool> headlessOption = new Option<bool>("--headless") { Description = "Run the application in headless mode without GUI." };
            clParser.Add(headlessOption);

            if(clParser.HasErrors())
            {
                MessageBox.Show("Error parsing command line arguments: " + string.Join(", ", clParser.GetErrors()));
                return;
            }

            bool b1 = clParser.GetValueForOption<bool>("--headless");
            bool b2 = clParser.GetValueForOption<bool>(headlessOption);
            var name = clParser.GetValueForOption<string>("--name");

            if (clParser.GetValueForOption<bool>("--headless"))
            {
                Trace.WriteLine("Running in headless mode.");
            }
            else
            {
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                MainWindow.Show();
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<MainWindow>();
            services.AddTransient<MainViewModel>();

            services.AddSingleton<IConfiguration>(_configuration);
            services.AddSingleton<CommandLineParser.CLParser>();
        }
    }
}
