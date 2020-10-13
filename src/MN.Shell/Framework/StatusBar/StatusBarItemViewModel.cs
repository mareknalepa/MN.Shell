using MN.Shell.MVVM;
using System.Windows.Input;

namespace MN.Shell.Framework.StatusBar
{
    public class StatusBarItemViewModel : PropertyChangedBase
    {
        private string _content;

        public string Content
        {
            get => _content;
            set => Set(ref _content, value);
        }

        public bool IsRightSide { get; set; }

        public double MinWidth { get; set; } = 100;

        public ICommand Command { get; set; }
    }
}
