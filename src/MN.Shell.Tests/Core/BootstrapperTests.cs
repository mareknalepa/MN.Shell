using MN.Shell.Core;
using NUnit.Framework;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace MN.Shell.Tests.Core
{
    [TestFixture]
    public class BootstrapperTests
    {
        private Thread _fakeUiThread;

        [Test]
        [Timeout(10000)]
        public void BootstrapperInitTest()
        {
            ManualResetEventSlim manualResetEventSlim = new ManualResetEventSlim();
            Application app = null;

            _fakeUiThread = new Thread(() =>
            {
                app = new Application();
                Bootstrapper bootstrapper = new Bootstrapper();

                app.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    manualResetEventSlim.Set();
                }));

                app.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Background);

                app.Run();
            });

            _fakeUiThread.SetApartmentState(ApartmentState.STA);
            _fakeUiThread.Start();

            manualResetEventSlim.Wait();

            Assert.NotNull(app);
            Assert.NotNull(app.Dispatcher);

            app.Dispatcher.Invoke(() => app.Shutdown());
        }

        [TearDown]
        public void TearDown()
        {
            if (!_fakeUiThread?.Join(500) ?? false)
                _fakeUiThread?.Abort();
        }
    }
}
