using MN.Shell.Core;
using NUnit.Framework;

namespace MN.Shell.Tests.Core
{
    [TestFixture]
    public class RelayCommandTests
    {
        [Test]
        public void RelayCommandDefaultCanExecuteTest()
        {
            RelayCommand relayCommand = new RelayCommand(sender => { });

            Assert.True(relayCommand.CanExecute(new object()));
        }

        [Test]
        public void RelayCommandDelegateCanExecuteTest()
        {
            bool canExecute = false;
            RelayCommand relayCommand = new RelayCommand(sender => { }, sender => canExecute);

            Assert.False(relayCommand.CanExecute(new object()));

            canExecute = true;
            Assert.True(relayCommand.CanExecute(new object()));
        }

        [Test]
        public void RelayCommandExecuteTest()
        {
            bool hasExecuted = false;
            RelayCommand relayCommand = new RelayCommand(sender => hasExecuted = true);

            Assert.False(hasExecuted);

            relayCommand.Execute(new object());
            Assert.True(hasExecuted);
        }
    }
}
