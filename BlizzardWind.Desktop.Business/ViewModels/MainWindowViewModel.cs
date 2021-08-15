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
            Route = "/Pages/HomePage.xaml";

            NavMenus = new ObservableCollection<NavMenuModel>()
            {
                new NavMenuModel(){ Name = "起始页", Icon = "\xe88a" , Route = "HomePage",Checked = true },
                new NavMenuModel(){ Name = "笔记", Icon = "\xea0e" , Route = "MarkTextPage",Checked = false},
                new NavMenuModel(){ Name = "TODO", Icon = "\xf0c5" , Route = ""},
                new NavMenuModel(){ Name = "图片集", Icon = "\xe8a7" , Route = ""},
                new NavMenuModel(){ Name = "视频集", Icon = "\xe04a" , Route = ""},
                new NavMenuModel(){ Name = "个人云盘", Icon = "\xe2bd" , Route = ""},
                new NavMenuModel(){ Name = "上传下载", Icon = "\xe2c0" , Route = ""},
            };

            SettingMenus = new ObservableCollection<NavMenuModel>()
            {
                new NavMenuModel(){ Name = "账号", Icon = "\xe853" , Route = ""},
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
