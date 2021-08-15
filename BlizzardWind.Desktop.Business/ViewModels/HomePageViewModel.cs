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
    public partial class HomePageViewModel : MvxViewModel
    {
        public IMvxCommand AppClickCommand => new MvxCommand<HomeAppModel>(OnAppClick);

        public ObservableCollection<HomeAppModel> AppCollection { get; set; }

        public Action<HomeAppModel> OnAppAction { get; set; }
    }

    public partial class HomePageViewModel
    {
        public HomePageViewModel()
        {
            AppCollection = new ObservableCollection<HomeAppModel>()
            {
                new HomeAppModel(){ Name = "文本编辑器",Icon = "\xe22b",Type=1},
                new HomeAppModel(){ Name = "文本编辑器",Icon = "\xe22b",Type=1},
                new HomeAppModel(){ Name = "文本编辑器",Icon = "\xe22b",Type=1},
                new HomeAppModel(){ Name = "文本编辑器",Icon = "\xe22b",Type=1},
                new HomeAppModel(){ Name = "文本编辑器",Icon = "\xe22b",Type=1},
                new HomeAppModel(){ Name = "文本编辑器",Icon = "\xe22b",Type=1},
                new HomeAppModel(){ Name = "文本编辑器",Icon = "\xe22b",Type=1},
                new HomeAppModel(){ Name = "文本编辑器",Icon = "\xe22b",Type=1},
            };
        }

        private void OnAppClick(HomeAppModel app)
        {
            if (OnAppAction != null)
            {
                OnAppAction.Invoke(app);
            }
        }
    }
}
