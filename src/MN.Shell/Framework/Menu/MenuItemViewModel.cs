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

        private bool _isSelectable;

        public bool IsSelectable
        {
            get => _isSelectable;
            set { _isSelectable = value; NotifyOfPropertyChange(); }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; NotifyOfPropertyChange(); }
        }

        private ICommand _command;

        public ICommand Command
        {
            get => _command;
            set { _command = value; NotifyOfPropertyChange(); }
        }
    }
}
