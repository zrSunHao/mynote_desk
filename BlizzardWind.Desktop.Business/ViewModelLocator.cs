using BlizzardWind.Desktop.Business.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace BlizzardWind.Desktop.Business
{
    public partial class ViewModelLocator
    {
        private readonly IServiceCollection ServiceCollection;
        private readonly ServiceProvider _serviceProvider;
        public ViewModelLocator()
        {
            ServiceCollection = new ServiceCollection();
            ConfigureServices(ServiceCollection);
            _serviceProvider = ServiceCollection.BuildServiceProvider();
        }
    }

    public partial class ViewModelLocator
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<EditorWindowViewModel>();

            services.AddSingleton<MarkTextPageViewModel>();

            services.AddSingleton<MainWindowViewModel>();
        }

        public EditorWindowViewModel? EditorWindowViewModel => _serviceProvider.GetService<EditorWindowViewModel>();
        public MarkTextPageViewModel? MarkTextPageViewModel => _serviceProvider.GetService<MarkTextPageViewModel>();
        public MainWindowViewModel? MainWindowViewModel => _serviceProvider.GetService<MainWindowViewModel>();
    }
}
