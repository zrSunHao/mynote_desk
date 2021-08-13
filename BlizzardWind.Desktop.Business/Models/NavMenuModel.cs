using MvvmCross.ViewModels;

namespace BlizzardWind.Desktop.Business.Models
{
    public class NavMenuModel : MvxViewModel
    {
        private string _Name;
        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }

        private string _Icon;
        public string Icon
        {
            get => _Icon;
            set => SetProperty(ref _Icon, value);
        }

        private bool _Checked;
        public bool Checked
        {
            get => _Checked;
            set => SetProperty(ref _Checked, value);
        }

        private string _Route;
        public string Route
        {
            get => _Route;
            set => SetProperty(ref _Route, value);
        }
    }
}
