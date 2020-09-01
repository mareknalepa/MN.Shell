using NUnit.Framework;

namespace MN.Shell.MVVM.Tests
{
    [TestFixture]
    public class RelayCommandTests
    {
        [Test]
        public void CanExecuteWithParameterTest()
        {
            bool canExecuteFired = false;
            bool canExecute = false;

            var command = new RelayCommand(o => { }, o =>
            {
                canExecuteFired = true;
                return canExecute;
            });

            Assert.False(canExecuteFired);

            Assert.False(command.CanExecute(new object()));
            Assert.True(canExecuteFired);

            canExecuteFired = false;
            canExecute = true;

            Assert.True(command.CanExecute(new object()));
            Assert.True(canExecuteFired);
        }

        [Test]
        public void CanExecuteWithoutParameterTest()
        {
            bool canExecuteFired = false;
            bool canExecute = false;

            var command = new RelayCommand(() => { }, () =>
            {
                canExecuteFired = true;
                return canExecute;
            });

            Assert.False(canExecuteFired);

            Assert.False(command.CanExecute(new object()));
            Assert.True(canExecuteFired);

            canExecuteFired = false;
            canExecute = true;

            Assert.True(command.CanExecute(new object()));
            Assert.True(canExecuteFired);
        }

        [Test]
        public void ExecuteWithParameterTest()
        {
            bool executeFired = false;
            bool canExecute = false;

            var command = new RelayCommand(o => executeFired = true, o => canExecute);

            Assert.False(executeFired);

            command.Execute(new object());
            Assert.False(executeFired);

            canExecute = true;

            command.Execute(new object());
            Assert.True(executeFired);
        }

        [Test]
        public void ExecuteWithoutParameterTest()
        {
            bool executeFired = false;
            bool canExecute = false;

            var command = new RelayCommand(() => executeFired = true, () => canExecute);

            Assert.False(executeFired);

            command.Execute(new object());
            Assert.False(executeFired);

            canExecute = true;

            command.Execute(new object());
            Assert.True(executeFired);
        }

        [Test]
        public void CanExecuteWithParameterWithoutDelegateIsTrueByDefaultTest()
        {
            bool executeFired = false;

            var command = new RelayCommand(o => executeFired = true);

            Assert.True(command.CanExecute(new object()));
            Assert.False(executeFired);

            command.Execute(new object());
            Assert.True(executeFired);
        }

        [Test]
        public void CanExecuteWithoutParameterWithoutDelegateIsTrueByDefaultTest()
        {
            bool executeFired = false;

            var command = new RelayCommand(() => executeFired = true);

            Assert.True(command.CanExecute(new object()));
            Assert.False(executeFired);

            command.Execute(new object());
            Assert.True(executeFired);
        }
    }
}
