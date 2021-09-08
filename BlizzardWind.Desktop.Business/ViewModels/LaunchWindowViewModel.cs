using BlizzardWind.Desktop.Business.Interfaces;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlizzardWind.Desktop.Business.ViewModels
{
    public partial class LaunchWindowViewModel : MvxViewModel
    {
        private readonly IDatabaseService _DatabaseService1;
        private readonly ViewModelMediator _Mediator;
    }

    public partial class LaunchWindowViewModel
    {
        public LaunchWindowViewModel(IDatabaseService databaseService, ViewModelMediator mediator)
        {
            _DatabaseService1 = databaseService;
            _Mediator = mediator;
        }
    }
}
