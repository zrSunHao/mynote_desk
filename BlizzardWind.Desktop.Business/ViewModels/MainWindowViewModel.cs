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

        public IMvxCommand NavTabCommand => new MvxCommand<NavMenuModel>(OnNavTabClick);

        public ObservableCollection<NavMenuModel> NavTabCollection { get; set; }

        private string _Route;
        public string Route
        {
            get => _Route;
            set => SetProperty(ref _Route, value);
        }

        private bool _HasNotification;
        public bool HasNotification
        {
            get => _HasNotification;
            set => SetProperty(ref _HasNotification, value);
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
            Route = $"/Pages/{PageNameConsts.NoteListPage}.xaml";

            NavTabCollection = new ObservableCollection<NavMenuModel>()
            {
                new NavMenuModel(){
                    Name = "首页", Route = PageNameConsts.NoteListPage,Checked = true,IsEnable = true
                },
                new NavMenuModel(){
                    Name = "类别", Route = PageNameConsts.NoteFamilyPage,Checked = false,IsEnable = true
                },
                new NavMenuModel(){
                    Name = "编辑", Route = PageNameConsts.EditorPage,Checked = false,IsEnable = false
                },
            };
        }

        private void RouteRedirectAction(string pageName)
        {
            var menu = NavTabCollection.FirstOrDefault(x => x.Route == pageName);
            if (menu != null)
            {
                foreach (var item in NavTabCollection)
                {
                    if (item.Route != menu.Route)
                        item.Checked = false;
                    else
                        item.Checked = true;
                }
                OnNavTabClick(menu);
            }
        }

        private void OnNavTabClick(NavMenuModel menu)
        {
            if (menu == null || string.IsNullOrEmpty(menu.Route))
                return;
            Route = $"/Pages/{menu.Route}.xaml";
        }
    }
}
