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

            if (!(view is Window window))
            {
                window = new ShellWindow()
                {
                    DataContext = view.DataContext,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    Content = view,
                };
            }

            return window;
        }
    }
}
