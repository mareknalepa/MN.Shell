using Caliburn.Micro;
using MN.Shell.Controls;
using System.Windows;

namespace MN.Shell.Core
{
    public class WindowManager : Caliburn.Micro.WindowManager
    {
        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            if (!(view is Window window))
            {
                window = new ShellWindow()
                {
                    Content = view,
                };

                window.SetValue(View.IsGeneratedProperty, true);

                var owner = InferOwnerOf(window);
                if (owner != null)
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    window.Owner = owner;
                }
                else
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
            }
            else
            {
                var owner = InferOwnerOf(window);
                if (owner != null && isDialog)
                {
                    window.Owner = owner;
                }
            }

            if (isDialog)
            {
                window.SizeToContent = SizeToContent.WidthAndHeight;
                window.ResizeMode = ResizeMode.NoResize;
            }

            return window;
        }
    }
}
