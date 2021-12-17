using MN.Shell.Controls;
using MN.Shell.MVVM;
using System;
using System.Windows;

namespace MN.Shell.Core
{
    public class ShellWindowManager : WindowManager
    {
        public ShellWindowManager(IViewManager viewManager) : base(viewManager) { }

        protected override Window EnsureWindow(FrameworkElement view, bool isDialog)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            if (view is not Window window)
            {
                window = new ShellWindow()
                {
                    DataContext = view.DataContext,
                    Content = view,
                    SizeToContent = isDialog ? SizeToContent.WidthAndHeight : SizeToContent.Manual,
                    ResizeMode = isDialog ? ResizeMode.NoResize : ResizeMode.CanResize,
                };
            }

            return window;
        }
    }
}
