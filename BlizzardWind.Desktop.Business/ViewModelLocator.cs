using BlizzardWind.Desktop.Business.Interfaces;
using BlizzardWind.Desktop.Business.Services;
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
            services.AddSingleton<IDatabaseService, DatabaseService>();
            services.AddSingleton<IFileResourceService, FileResourceService>();
            services.AddSingleton<IArticleService, ArticleService>();
            services.AddSingleton<IFolderService, FolderService>();
            services.AddSingleton<IFamilyService, FamilyService>();

            services.AddSingleton<ViewModelMediator>();

            services.AddTransient<ReaderWindowViewModel>();
            services.AddTransient<NoteListPageViewModel>();
            services.AddTransient<NoteFamilyPageViewModel>();

            services.AddTransient<LaunchWindowViewModel>();
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<EditorPageViewModel>();
        }

        public ReaderWindowViewModel? ReaderWindowViewModel => _serviceProvider.GetService<ReaderWindowViewModel>();
        public NoteListPageViewModel? NoteListPageViewModel => _serviceProvider.GetService<NoteListPageViewModel>();
        public NoteFamilyPageViewModel? NoteFamilyPageViewModel => _serviceProvider.GetService<NoteFamilyPageViewModel>();

        public LaunchWindowViewModel? LaunchWindowViewModel => _serviceProvider.GetService<LaunchWindowViewModel>();
        public EditorPageViewModel? EditorPageViewModel => _serviceProvider.GetService<EditorPageViewModel>();
        public MainWindowViewModel? MainWindowViewModel => _serviceProvider.GetService<MainWindowViewModel>();
    }
}
