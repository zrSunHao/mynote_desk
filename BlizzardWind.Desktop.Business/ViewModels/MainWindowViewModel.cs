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
        private readonly ViewModelMediator _Mediator;

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
        public MainWindowViewModel(ViewModelMediator mediator)
        {
            _Mediator = mediator;
            _Mediator.RouteRedirectAction += RouteRedirectAction;
            MenuInitial();
        }

        private void MenuInitial()
        {
            Route = "/Pages/ArticleListPage.xaml";

            NavMenus = new ObservableCollection<NavMenuModel>()
            {
                new NavMenuModel(){
                    Name = "起始页", Icon = "\xe871" , Route = PageNameConsts.ArticleListPage,Checked = true,IsEnable = true
                },
                new NavMenuModel(){
                    Name = "类别管理", Icon = "\xf1c8" , Route = PageNameConsts.ArticleFamilyPage,Checked = false,IsEnable = true
                },
                new NavMenuModel(){
                    Name = "编辑文章", Icon = "\xe3c9" , Route = PageNameConsts.EditorPage,Checked = false,IsEnable = false
                },
            };

            SettingMenus = new ObservableCollection<NavMenuModel>()
            {
                new NavMenuModel(){ Name = "设置", Icon = "\xe8b9" , Route = "",Checked = false,IsEnable = false},
            };

        }

        private void RouteRedirectAction(string pageName)
        {
            var menu = NavMenus.FirstOrDefault(x => x.Route == pageName);
            if (menu != null)
            {
                foreach (var item in NavMenus)
                {
                    if (item.Route != menu.Route)
                        item.Checked = false;
                    else
                        item.Checked = true;
                }
                OnMenuClick(menu);
            }
        }

        private void OnMenuClick(NavMenuModel menu)
        {
            if (menu == null || string.IsNullOrEmpty(menu.Route))
                return;
            Route = $"/Pages/{menu.Route}.xaml";
        }
    }
}
