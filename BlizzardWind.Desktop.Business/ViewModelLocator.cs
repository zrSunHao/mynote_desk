﻿using BlizzardWind.Desktop.Business.Interfaces;
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

            services.AddTransient<MarkTextPageViewModel>();
            services.AddTransient<HomePageViewModel>();
            services.AddTransient<ArticleListPageViewModel>();
            services.AddTransient<ArticleFamilyPageViewModel>();

            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<EditorWindowViewModel>();
        }

        public HomePageViewModel? HomePageViewModel => _serviceProvider.GetService<HomePageViewModel>();
        public MarkTextPageViewModel? MarkTextPageViewModel => _serviceProvider.GetService<MarkTextPageViewModel>();
        public ArticleListPageViewModel? ArticleListPageViewModel => _serviceProvider.GetService<ArticleListPageViewModel>();
        public ArticleFamilyPageViewModel? ArticleFamilyPageViewModel => _serviceProvider.GetService<ArticleFamilyPageViewModel>();

        public EditorWindowViewModel? EditorWindowViewModel => _serviceProvider.GetService<EditorWindowViewModel>();
        public MainWindowViewModel? MainWindowViewModel => _serviceProvider.GetService<MainWindowViewModel>();
    }
}
