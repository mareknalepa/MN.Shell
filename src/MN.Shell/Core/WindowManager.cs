using MN.Shell.Controls;
using MN.Shell.MVVM;
using System;
using System.Windows;

namespace MN.Shell.Core
{
    public class WindowManager : MVVM.WindowManager
    {
        public WindowManager(IViewManager viewManager) : base(viewManager) { }

        protected override Window EnsureWindow(FrameworkElement view, bool isDialog)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            if (!(view is Window window))
            {
                window = new ShellWindow()
                {
                    Content = view,
                };
            }

            if (isDialog)
            {
                var activeWindow = GetActiveWindow?.Invoke();
                if (ReferenceEquals(activeWindow, window))
                    activeWindow = null;

                if (activeWindow != null)
                {
                    try
                    {
                        window.Owner = activeWindow;
                    }
                    catch (InvalidOperationException) { }
                }
            }

            if (window.Owner != null)
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            else
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            if (isDialog)
            {
                window.SizeToContent = SizeToContent.WidthAndHeight;
            }

            return window;
        }
    }
}
