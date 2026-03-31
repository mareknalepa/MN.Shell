using MN.Shell.MVVM.IntegrationTests.Stubs;
using System.Windows;

namespace MN.Shell.MVVM.IntegrationTests
{
    public sealed class WindowManagerTests
    {
        private readonly WindowManager _windowManager;

        public WindowManagerTests()
        {
            _windowManager = new(new ViewManager());
        }

        [StaTheory]
        [InlineData(false)]
        [InlineData(true)]
        public void ShowWindow_ShowsWindow(bool isAnotherActiveWindow)
        {
            var viewModel = new WindowViewModel();

            Window? ownerWindow = null;
            if (isAnotherActiveWindow)
            {
                ownerWindow = new Window() { Height = 1, Width = 1, WindowState = WindowState.Minimized };
                ownerWindow.Show();
            }
            _windowManager.GetActiveWindow = () => ownerWindow!;

            bool windowShown = false;
            viewModel.OnLoadedAction = window =>
            {
                windowShown = true;
                window.DataContext.ShouldBeSameAs(viewModel);
                window.Owner.ShouldBeNull();
            };

            _windowManager.ShowWindow(viewModel);

            windowShown.ShouldBeTrue();
        }

        [StaTheory]
        [InlineData(false)]
        [InlineData(true)]
        public void ShowWindow_ShowsUserControlWrappedInWindow(bool isAnotherActiveWindow)
        {
            var viewModel = new UserControlViewModel();

            Window? ownerWindow = null;
            if (isAnotherActiveWindow)
            {
                ownerWindow = new Window() { Height = 1, Width = 1, WindowState = WindowState.Minimized };
                ownerWindow.Show();
            }
            _windowManager.GetActiveWindow = () => ownerWindow!;

            bool windowShown = false;
            viewModel.OnLoadedAction = userControl =>
            {
                windowShown = true;
                userControl.DataContext.ShouldBeSameAs(viewModel);
                var parentWindow = userControl.Parent as Window;
                parentWindow.ShouldNotBeNull();
                parentWindow.DataContext.ShouldBeSameAs(viewModel);
                parentWindow.Owner.ShouldBeNull();
            };

            _windowManager.ShowWindow(viewModel);

            windowShown.ShouldBeTrue();
        }

        [StaTheory]
        [InlineData(false)]
        [InlineData(true)]
        public void ShowDialog_ShowsWindow(bool isAnotherActiveWindow)
        {
            var viewModel = new WindowViewModel();

            Window? ownerWindow = null;
            if (isAnotherActiveWindow)
            {
                ownerWindow = new Window() { Height = 1, Width = 1, WindowState = WindowState.Minimized };
                ownerWindow.Show();
            }
            _windowManager.GetActiveWindow = () => ownerWindow!;

            bool windowShown = false;
            viewModel.OnLoadedAction = window =>
            {
                windowShown = true;
                window.DataContext.ShouldBeSameAs(viewModel);
                if (isAnotherActiveWindow)
                {
                    window.Owner.ShouldBeSameAs(ownerWindow);
                }
                else
                {
                    window.Owner.ShouldBeNull();
                }
            };

            _windowManager.ShowDialog(viewModel);

            windowShown.ShouldBeTrue();
        }

        [StaTheory]
        [InlineData(false)]
        [InlineData(true)]
        public void ShowDialog_ShowsUserControlWrappedInWindow(bool isAnotherActiveWindow)
        {
            var viewModel = new UserControlViewModel();

            Window? ownerWindow = null;
            if (isAnotherActiveWindow)
            {
                ownerWindow = new Window() { Height = 1, Width = 1, WindowState = WindowState.Minimized };
                ownerWindow.Show();
            }
            _windowManager.GetActiveWindow = () => ownerWindow!;

            bool windowShown = false;
            viewModel.OnLoadedAction = userControl =>
            {
                windowShown = true;
                userControl.DataContext.ShouldBeSameAs(viewModel);
                var parentWindow = userControl.Parent as Window;
                parentWindow.ShouldNotBeNull();
                parentWindow.DataContext.ShouldBeSameAs(viewModel);
                if (isAnotherActiveWindow)
                {
                    parentWindow.Owner.ShouldBeSameAs(ownerWindow);
                }
                else
                {
                    parentWindow.Owner.ShouldBeNull();
                }
            };

            _windowManager.ShowDialog(viewModel);

            windowShown.ShouldBeTrue();
        }

        [StaFact]
        public void ShowWindow_ActivatesLifecycleAware()
        {
            var viewModel = new LifecycleTestViewModel();

            _windowManager.ShowWindow(viewModel);

            viewModel.ActivateCalledCount.ShouldBe(1);
        }

        [StaFact]
        public void ShowWindow_ClosesLifecycleAware()
        {
            var viewModel = new LifecycleTestViewModel();

            _windowManager.ShowWindow(viewModel);

            viewModel.CloseCalledCount.ShouldBe(1);
        }

        [StaTheory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void ShowWindowOrDialog_BindsWindowTitle(bool isDialog, bool isUserControl)
        {
            object viewModel;
            string actualTitle = string.Empty;
            string expectedTitle = isUserControl ? "UserControl Title" : "Window Title";

            if (isUserControl)
            {
                viewModel = new UserControlViewModel()
                {
                    OnLoadedAction = userControl => actualTitle = (userControl.Parent as Window)?.Title ?? string.Empty
                };
            }
            else
            {
                viewModel = new WindowViewModel()
                {
                    OnLoadedAction = window => actualTitle = window.Title
                };
            }

            if (isDialog)
            {
                _windowManager.ShowDialog(viewModel);
            }
            else
            {
                _windowManager.ShowWindow(viewModel);
            }

            actualTitle.ShouldBe(expectedTitle);
        }

        [StaTheory]
        [InlineData(false)]
        [InlineData(true)]
        public void ShowWindowOrDialog_OrUsesExistingWindowTitle(bool isDialog)
        {
            const string existingTitle = "Existing Window Title";
            string actualTitle = string.Empty;

            var viewModel = new WindowWithTitleViewModel()
            {
                OnLoadedAction = window => actualTitle = window.Title
            };

            if (isDialog)
            {
                _windowManager.ShowDialog(viewModel);
            }
            else
            {
                _windowManager.ShowWindow(viewModel);
            }

            actualTitle.ShouldBe(existingTitle);
        }

        [StaTheory]
        [InlineData(false)]
        [InlineData(true)]
        public void ShowWindowOrDialog_Throws_WhenViewManagerThrows(bool isDialog)
        {
            var viewModel = new object();

            if (isDialog)
            {
                Action act = () => _windowManager.ShowDialog(viewModel);
                act.ShouldThrow<InvalidOperationException>();
            }
            else
            {
                Action act = () => _windowManager.ShowWindow(viewModel);
                act.ShouldThrow<InvalidOperationException>();
            }
        }
    }
}
