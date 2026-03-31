namespace MN.Shell.MVVM.UnitTests
{
    public sealed class CommandTests
    {
        [Fact]
        public void CanExecute_WithParameter_ReturnsCorrectResult()
        {
            bool canExecuteFired = false;
            bool canExecute = false;

            var command = new Command(o => { }, o =>
            {
                canExecuteFired = true;
                return canExecute;
            });

            canExecuteFired.ShouldBeFalse();
            command.CanExecute(new()).ShouldBeFalse();
            canExecuteFired.ShouldBeTrue();

            canExecuteFired = false;
            canExecute = true;

            command.CanExecute(new()).ShouldBeTrue();
            canExecuteFired.ShouldBeTrue();
        }

        [Fact]
        public void CanExecute_WithoutParameter_ReturnsCorrectResult()
        {
            bool canExecuteFired = false;
            bool canExecute = false;

            var command = new Command(() => { }, () =>
            {
                canExecuteFired = true;
                return canExecute;
            });

            canExecuteFired.ShouldBeFalse();
            command.CanExecute(new()).ShouldBeFalse();
            canExecuteFired.ShouldBeTrue();

            canExecuteFired = false;
            canExecute = true;

            command.CanExecute(new()).ShouldBeTrue();
            canExecuteFired.ShouldBeTrue();
        }

        [Fact]
        public void Execute_WithParameter_CallsDelegateOnlyWhenCanExecuteAllows()
        {
            bool executeFired = false;
            bool canExecute = false;

            var command = new Command(o => executeFired = true, o => canExecute);

            executeFired.ShouldBeFalse();

            command.Execute(new());
            executeFired.ShouldBeFalse();

            canExecute = true;

            command.Execute(new());
            executeFired.ShouldBeTrue();
        }

        [Fact]
        public void Execute_WithoutParameter_CallsDelegateOnlyWhenCanExecuteAllows()
        {
            bool executeFired = false;
            bool canExecute = false;

            var command = new Command(() => executeFired = true, () => canExecute);

            executeFired.ShouldBeFalse();

            command.Execute(new());
            executeFired.ShouldBeFalse();

            canExecute = true;

            command.Execute(new());
            executeFired.ShouldBeTrue();
        }

        [Fact]
        public void CanExecute_WithParameter_WithoutDelegateIsTrueByDefault()
        {
            bool executeFired = false;

            var command = new Command(o => executeFired = true);

            command.CanExecute(new()).ShouldBeTrue();
            executeFired.ShouldBeFalse();

            command.Execute(new());
            executeFired.ShouldBeTrue();
        }

        [Fact]
        public void CanExecute_WithoutParameter_WithoutDelegateIsTrueByDefault()
        {
            bool executeFired = false;

            var command = new Command(() => executeFired = true);

            command.CanExecute(new()).ShouldBeTrue();
            executeFired.ShouldBeFalse();

            command.Execute(new());
            executeFired.ShouldBeTrue();
        }
    }
}
