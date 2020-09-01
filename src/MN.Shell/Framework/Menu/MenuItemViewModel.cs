using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MN.Shell.MVVM;

namespace MN.Shell.Framework.Menu
{
    public class MenuItemViewModel : PropertyChangedBase
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public ObservableCollection<MenuItemViewModel> SubItems { get; }
            = new ObservableCollection<MenuItemViewModel>();

        private Uri _icon;

        public Uri Icon
        {
            get => _icon;
            set => Set(ref _icon, value);
        }

        public bool IsCheckable { get; set; }

        public Action<bool> OnIsCheckedChanged { get; set; }

        private bool _isChecked;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    NotifyPropertyChanged();
                    OnIsCheckedChanged?.Invoke(value);
                }
            }
        }

        private ICommand _command;

        public ICommand Command
        {
            get => _command;
            set => Set(ref _command, value);
        }

        private bool _fitsIntoMainMenu;

        public bool FitsIntoMainMenu
        {
            get => _fitsIntoMainMenu;
            set => Set(ref _fitsIntoMainMenu, value);
        }

        public bool IsSeparator { get; set; }

        public bool IsEllipsis { get; set; }
    }
}
