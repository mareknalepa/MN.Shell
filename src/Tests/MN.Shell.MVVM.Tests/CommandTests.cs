using NUnit.Framework;

namespace MN.Shell.MVVM.Tests
{
    [TestFixture]
    public class CommandTests
    {
        [Test]
        public void CanExecuteWithParameterTest()
        {
            bool canExecuteFired = false;
            bool canExecute = false;

            var command = new Command(o => { }, o =>
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

            var command = new Command(() => { }, () =>
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

            var command = new Command(o => executeFired = true, o => canExecute);

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

            var command = new Command(() => executeFired = true, () => canExecute);

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

            var command = new Command(o => executeFired = true);

            Assert.True(command.CanExecute(new object()));
            Assert.False(executeFired);

            command.Execute(new object());
            Assert.True(executeFired);
        }

        [Test]
        public void CanExecuteWithoutParameterWithoutDelegateIsTrueByDefaultTest()
        {
            bool executeFired = false;

            var command = new Command(() => executeFired = true);

            Assert.True(command.CanExecute(new object()));
            Assert.False(executeFired);

            command.Execute(new object());
            Assert.True(executeFired);
        }
    }
}
