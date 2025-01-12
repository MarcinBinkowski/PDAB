using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using PDAB.ViewModels;
using PDAB.Repository;
using PDAB.Models;

namespace PDAB
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register DbContext
            services.AddDbContext<PdabDbContext>();

            // Register repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Register ViewModels
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<AllCategoriesViewModel>();

            // Register MainWindow
            services.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindow.Show();
        }
    }
}