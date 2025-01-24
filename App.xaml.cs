using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using PDAB.ViewModels;
using PDAB.Repository;
using PDAB.Models;
using PDAB.Services;

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
            services.AddDbContext<PdabDbContext>();

            # region repositories
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            # endregion
            
            # region services
            services.AddScoped<DialogService, DialogService>();
            services.AddSingleton<PasswordService, PasswordService>();
            services.AddSingleton<EmailService, EmailService>();
            services.AddSingleton<InvoiceService, InvoiceService>();
            #endregion
            
            # region viewmodels
            services.AddTransient<MainWindowViewModel>();
            # endregion
            
            services.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindow.Show();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
            base.OnExit(e);
        }
    }
}