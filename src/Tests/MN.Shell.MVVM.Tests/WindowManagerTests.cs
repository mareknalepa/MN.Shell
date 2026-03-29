using MN.Shell.MVVM.Tests.Mocks;
using NSubstitute.ClearExtensions;
using NSubstitute.ExceptionExtensions;
using System.Windows;

namespace MN.Shell.MVVM.Tests
{
    public sealed class WindowManagerTests
    {
        private readonly IViewManager _viewManager;
        private readonly WindowManager _windowManager;

        public WindowManagerTests()
        {
            _viewManager = Substitute.For<IViewManager>();
            _windowManager = new(_viewManager);
        }

        [StaTheory]
        [InlineData(false)]
        [InlineData(true)]
        public void ShowWindow_ShowsWindow(bool isAnotherActiveWindow)
        {
            var viewModel = new object();
            var view = new MockWindowView() { DataContext = viewModel };

            Window? ownerWindow = null;
            if (isAnotherActiveWindow)
            {
                ownerWindow = new Window() { Height = 1, Width = 1, WindowState = WindowState.Minimized };
                ownerWindow.Show();
            }
            _windowManager.GetActiveWindow = () => ownerWindow!;

            bool windowShown = false;
            view.OnLoadedAction = window =>
            {
                windowShown = true;
                window.DataContext.ShouldBeSameAs(viewModel);
                window.Owner.ShouldBeNull();
            };

            _viewManager.GetViewFor(viewModel).Returns(view);

            _windowManager.ShowWindow(viewModel);

            _viewManager.Received(1).GetViewFor(viewModel);
            windowShown.ShouldBeTrue();
        }

        [StaTheory]
        [InlineData(false)]
        [InlineData(true)]
        public void ShowWindow_ShowsUserControlWrappedInWindow(bool isAnotherActiveWindow)
        {
            var viewModel = new object();
            var view = new MockUserControlView() { DataContext = viewModel };

            Window? ownerWindow = null;
            if (isAnotherActiveWindow)
            {
                ownerWindow = new Window() { Height = 1, Width = 1, WindowState = WindowState.Minimized };
                ownerWindow.Show();
            }
            _windowManager.GetActiveWindow = () => ownerWindow!;

            bool windowShown = false;
            view.OnLoadedAction = userControl =>
            {
                windowShown = true;
                userControl.DataContext.ShouldBeSameAs(viewModel);
                var parentWindow = userControl.Parent as Window;
                parentWindow.ShouldNotBeNull();
                parentWindow.DataContext.ShouldBeSameAs(viewModel);
                parentWindow.Owner.ShouldBeNull();
            };

            _viewManager.GetViewFor(viewModel).Returns(view);

            _windowManager.ShowWindow(viewModel);

            _viewManager.Received(1).GetViewFor(viewModel);
            windowShown.ShouldBeTrue();
        }

        [StaTheory]
        [InlineData(false)]
        [InlineData(true)]
        public void ShowDialog_ShowsWindow(bool isAnotherActiveWindow)
        {
            var viewModel = new object();
            var view = new MockWindowView() { DataContext = viewModel };

            Window? ownerWindow = null;
            if (isAnotherActiveWindow)
            {
                ownerWindow = new Window() { Height = 1, Width = 1, WindowState = WindowState.Minimized };
                ownerWindow.Show();
            }
            _windowManager.GetActiveWindow = () => ownerWindow!;

            bool windowShown = false;
            view.OnLoadedAction = window =>
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

            _viewManager.GetViewFor(viewModel).Returns(view);

            _windowManager.ShowDialog(viewModel);

            _viewManager.Received(1).GetViewFor(viewModel);
            windowShown.ShouldBeTrue();
        }

        [StaTheory]
        [InlineData(false)]
        [InlineData(true)]
        public void ShowDialog_ShowsUserControlWrappedInWindow(bool isAnotherActiveWindow)
        {
            var viewModel = new object();
            var view = new MockUserControlView() { DataContext = viewModel };

            Window? ownerWindow = null;
            if (isAnotherActiveWindow)
            {
                ownerWindow = new Window() { Height = 1, Width = 1, WindowState = WindowState.Minimized };
                ownerWindow.Show();
            }
            _windowManager.GetActiveWindow = () => ownerWindow!;

            bool windowShown = false;
            view.OnLoadedAction = userControl =>
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

            _viewManager.GetViewFor(viewModel).Returns(view);

            _windowManager.ShowDialog(viewModel);

            _viewManager.Received(1).GetViewFor(viewModel);
            windowShown.ShouldBeTrue();
        }

        [StaTheory]
        [InlineData(false)]
        [InlineData(true)]
        public void ShowWindowOrDialog_ActivatesLifecycleAware(bool isDialog)
        {
            var viewModel = Substitute.For<ILifecycleAware>();
            var view = new MockWindowView() { DataContext = viewModel };

            _viewManager.GetViewFor(viewModel).Returns(view);

            if (isDialog)
            {
                _windowManager.ShowDialog(viewModel);
            }
            else
            {
                _windowManager.ShowWindow(viewModel);
            }

            viewModel.Received(1).Activate();
        }

        [StaTheory]
        [InlineData(false)]
        [InlineData(true)]
        public void ShowWindowOrDialog_ClosesLifecycleAware(bool isDialog)
        {
            var viewModel = Substitute.For<ILifecycleAware>();
            var view = new MockWindowView() { DataContext = viewModel };

            _viewManager.GetViewFor(viewModel).Returns(view);

            if (isDialog)
            {
                _windowManager.ShowDialog(viewModel);
            }
            else
            {
                _windowManager.ShowWindow(viewModel);
            }

            viewModel.Received(1).Close();
        }

        [StaTheory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void ShowWindowOrDialog_BindsWindowTitle(bool isDialog, bool isUserControl)
        {
            const string title = "Window Title";

            var viewModel = Substitute.For<IHaveTitle>();
            viewModel.Title.Returns(title);

            string actualTitle = string.Empty;

            FrameworkElement? view = null;
            if (isUserControl)
            {
                var userControlView = new MockUserControlView
                {
                    DataContext = viewModel,
                    OnLoadedAction = userControl => actualTitle = (userControl.Parent as Window)?.Title ?? string.Empty
                };
                view = userControlView;
            }
            else
            {
                var windowView = new MockWindowView
                {
                    DataContext = viewModel,
                    OnLoadedAction = window => actualTitle = window.Title
                };
                windowView.Title.ShouldBeNullOrEmpty();
                view = windowView;
            }

            _viewManager.GetViewFor(viewModel).Returns(view);

            if (isDialog)
            {
                _windowManager.ShowDialog(viewModel);
            }
            else
            {
                _windowManager.ShowWindow(viewModel);
            }

            actualTitle.ShouldBe(title);
        }

        [StaTheory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void ShowWindowOrDialog_BindWindowTitleOrUsesExistingOne(bool isDialog, bool isUserControl)
        {
            const string title = "Window Title";
            const string existingTitle = "Existing Window Title";

            var viewModel = Substitute.For<IHaveTitle>();
            viewModel.Title.Returns(title);

            string actualTitle = string.Empty;

            FrameworkElement? view = null;
            if (isUserControl)
            {
                var userControlView = new MockUserControlView
                {
                    DataContext = viewModel,
                    OnLoadedAction = userControl => actualTitle = (userControl.Parent as Window)?.Title ?? string.Empty
                };
                view = userControlView;
            }
            else
            {
                var windowView = new MockWindowView
                {
                    DataContext = viewModel,
                    Title = existingTitle,
                    OnLoadedAction = window => actualTitle = window.Title
                };
                windowView.Title.ShouldBe(existingTitle);
                view = windowView;
            }

            _viewManager.GetViewFor(viewModel).Returns(view);

            if (isDialog)
            {
                _windowManager.ShowDialog(viewModel);
            }
            else
            {
                _windowManager.ShowWindow(viewModel);
            }

            if (isUserControl)
            {
                actualTitle.ShouldBe(title);
            }
            else
            {
                actualTitle.ShouldBe(existingTitle);
            }
        }

        [StaTheory]
        [InlineData(false)]
        [InlineData(true)]
        public void ShowWindowOrDialog_Throws_WhenViewManagerThrows(bool isDialog)
        {
            var viewModel = new object();

            _viewManager.GetViewFor(viewModel).Throws<ArgumentNullException>();

            if (isDialog)
            {
                Action act = () => _windowManager.ShowDialog(viewModel);
                act.ShouldThrow<ArgumentNullException>();
            }
            else
            {
                Action act = () => _windowManager.ShowWindow(viewModel);
                act.ShouldThrow<ArgumentNullException>();
            }

            _viewManager.ClearSubstitute();
            _viewManager.GetViewFor(viewModel).Throws<InvalidOperationException>();

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
