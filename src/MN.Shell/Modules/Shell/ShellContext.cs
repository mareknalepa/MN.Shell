using System;

namespace MN.Shell.Modules.Shell
{
    public class ShellContext
    {
        #region "Application Title"

        private string _applicationTitle = string.Empty;

        public event EventHandler<string> ApplicationTitleChanged;

        public string ApplicationTitle
        {
            get => _applicationTitle;
            set
            {
                if (_applicationTitle != value)
                {
                    _applicationTitle = value;
                    ApplicationTitleChanged?.Invoke(this, _applicationTitle);
                }
            }
        }

        #endregion

        #region "Exit handling"

        public event EventHandler ApplicationExitRequested;

        public void RequestApplicationExit() => ApplicationExitRequested?.Invoke(this, EventArgs.Empty);

        #endregion
    }
}
