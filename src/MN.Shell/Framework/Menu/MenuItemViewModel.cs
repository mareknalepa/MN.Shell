using Caliburn.Micro;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MN.Shell.Framework.Menu
{
    public class MenuItemViewModel : PropertyChangedBase
    {
        private string _name;

        public string Name
        {
            get => _name;
            set { _name = value; NotifyOfPropertyChange(); }
        }

        public ObservableCollection<MenuItemViewModel> SubItems { get; }
            = new ObservableCollection<MenuItemViewModel>();

        private Uri _icon;

        public Uri Icon
        {
            get => _icon;
            set { _icon = value; NotifyOfPropertyChange(); }
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
                    NotifyOfPropertyChange();
                    OnIsCheckedChanged?.Invoke(value);
                }
            }
        }

        private ICommand _command;

        public ICommand Command
        {
            get => _command;
            set { _command = value; NotifyOfPropertyChange(); }
        }

        private bool _fitsIntoMainMenu;

        public bool FitsIntoMainMenu
        {
            get => _fitsIntoMainMenu;
            set { _fitsIntoMainMenu = value; NotifyOfPropertyChange(); }
        }

        public bool IsSeparator { get; set; }

        public bool IsEllipsis { get; set; }
    }
}
