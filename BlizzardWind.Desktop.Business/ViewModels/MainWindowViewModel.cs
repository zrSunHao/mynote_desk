using BlizzardWind.Desktop.Business.Models;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlizzardWind.Desktop.Business.ViewModels
{
    public partial class MainWindowViewModel : MvxViewModel
    {
        public IMvxCommand MenuCommand => new MvxCommand<NavMenuModel>(OnMenuClick);

        public ObservableCollection<NavMenuModel> NavMenus { get; set; }
        public ObservableCollection<NavMenuModel> SettingMenus { get; set; }

        private string _Route;
        public string Route
        {
            get => _Route;
            set => SetProperty(ref _Route, value);
        }
    }

    public partial class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            MenuInitial();        }

        private void MenuInitial()
        {
            Route = "/Pages/ArticleListPage.xaml";

            NavMenus = new ObservableCollection<NavMenuModel>()
            {
                //new NavMenuModel(){ Name = "起始页", Icon = "\xe88a" , Route = "HomePage",Checked = true },
                new NavMenuModel(){ Name = "起始页", Icon = "\xe88a" , Route = "ArticleListPage",Checked = true},
                new NavMenuModel(){ Name = "文章类别管理页", Icon = "\xe8a7" , Route = "ArticleFamilyPage"},
                new NavMenuModel(){ Name = "阅读文章页", Icon = "\xf0c5" , Route = "MarkTextPage"},
            };

            SettingMenus = new ObservableCollection<NavMenuModel>()
            {
                new NavMenuModel(){ Name = "设置", Icon = "\xe8b9" , Route = ""},
            };

        }

        private void OnMenuClick(NavMenuModel menu)
        {
            if (menu == null || string.IsNullOrEmpty(menu.Route))
                return;
            Route = $"/Pages/{menu.Route}.xaml";
        }
    }
}
