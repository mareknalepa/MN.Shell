namespace MN.Shell.MVVM.Tests
{
    public sealed class AsyncCommandTests
    {
        [Fact]
        public void CanExecute_WithParameter_ReturnsCorrectResult()
        {
            bool canExecuteFired = false;
            bool canExecute = false;

            var command = new AsyncCommand(o => Task.CompletedTask, o =>
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

            var command = new AsyncCommand(() => Task.CompletedTask, () =>
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

            var command = new AsyncCommand(o =>
            {
                executeFired = true;
                return Task.CompletedTask;
            }, o => canExecute);

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

            var command = new AsyncCommand(() =>
            {
                executeFired = true;
                return Task.CompletedTask;
            }, () => canExecute);

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

            var command = new AsyncCommand(o =>
            {
                executeFired = true;
                return Task.CompletedTask;
            });

            command.CanExecute(new()).ShouldBeTrue();
            executeFired.ShouldBeFalse();

            command.Execute(new());
            executeFired.ShouldBeTrue();
        }

        [Fact]
        public void CanExecute_WithoutParameter_WithoutDelegateIsTrueByDefault()
        {
            bool executeFired = false;

            var command = new AsyncCommand(() =>
            {
                executeFired = true;
                return Task.CompletedTask;
            });

            command.CanExecute(new()).ShouldBeTrue();
            executeFired.ShouldBeFalse();

            command.Execute(new());
            executeFired.ShouldBeTrue();
        }

        [Fact]
        public async Task CanExecute_WithParameter_ReturnsFalse_WhileCommandIsAlreadyRunning()
        {
            using var runningSemaphore = new SemaphoreSlim(0);
            using var completionSemaphore = new SemaphoreSlim(0);

            var command = new AsyncCommand(async o =>
            {
                runningSemaphore.Release();
                await completionSemaphore.WaitAsync().ConfigureAwait(false);
            });

            command.CanExecute(new()).ShouldBeTrue();

            command.Execute(new());
            runningSemaphore.Wait(TestContext.Current.CancellationToken);

            command.CanExecute(new()).ShouldBeFalse();

            completionSemaphore.Release();
            await command.Execution!.TaskCompleted;

            command.CanExecute(new()).ShouldBeTrue();
        }

        [Fact]
        public async Task CanExecute_WithoutParameter_ReturnsFalse_WhileCommandIsAlreadyRunning()
        {
            using var runningSemaphore = new SemaphoreSlim(0);
            using var completionSemaphore = new SemaphoreSlim(0);

            var command = new AsyncCommand(async () =>
            {
                runningSemaphore.Release();
                await completionSemaphore.WaitAsync().ConfigureAwait(false);
            });

            command.CanExecute(new()).ShouldBeTrue();

            command.Execute(new());
            runningSemaphore.Wait(TestContext.Current.CancellationToken);

            command.CanExecute(new()).ShouldBeFalse();

            completionSemaphore.Release();
            await command.Execution!.TaskCompleted;

            command.CanExecute(new()).ShouldBeTrue();
        }

        [Fact]
        public async Task Execute_WithParameter_TransitionsToCorrectStateOnSuccess()
        {
            using var runningSemaphore = new SemaphoreSlim(0);
            using var completionSemaphore = new SemaphoreSlim(0);

            var command = new AsyncCommand(async o =>
            {
                runningSemaphore.Release();
                await completionSemaphore.WaitAsync().ConfigureAwait(false);
            });

            command.IsExecuting.ShouldBeFalse();
            command.Execution.ShouldBeNull();

            command.Execute(new());
            runningSemaphore.Wait(TestContext.Current.CancellationToken);

            command.IsExecuting.ShouldBeTrue();
            command.Execution.ShouldNotBeNull();

            command.Execution?.IsCompleted.ShouldBeFalse();
            command.Execution?.IsNotCompleted.ShouldBeTrue();
            command.Execution?.IsCompletedSuccessfully.ShouldBeFalse();
            command.Execution?.IsCanceled.ShouldBeFalse();
            command.Execution?.IsFaulted.ShouldBeFalse();

            completionSemaphore.Release();
            await command.Execution!.TaskCompleted;

            command.IsExecuting.ShouldBeFalse();
            command.Execution.ShouldNotBeNull();

            command.Execution?.IsCompleted.ShouldBeTrue();
            command.Execution?.IsNotCompleted.ShouldBeFalse();
            command.Execution?.IsCompletedSuccessfully.ShouldBeTrue();
            command.Execution?.IsCanceled.ShouldBeFalse();
            command.Execution?.IsFaulted.ShouldBeFalse();
        }

        [Fact]
        public async Task Execute_WithoutParameter_TransitionsToCorrectStateOnSuccess()
        {
            using var runningSemaphore = new SemaphoreSlim(0);
            using var completionSemaphore = new SemaphoreSlim(0);

            var command = new AsyncCommand(async () =>
            {
                runningSemaphore.Release();
                await completionSemaphore.WaitAsync().ConfigureAwait(false);
            });

            command.IsExecuting.ShouldBeFalse();
            command.Execution.ShouldBeNull();

            command.Execute(new());
            runningSemaphore.Wait(TestContext.Current.CancellationToken);

            command.IsExecuting.ShouldBeTrue();
            command.Execution.ShouldNotBeNull();

            command.Execution?.IsCompleted.ShouldBeFalse();
            command.Execution?.IsNotCompleted.ShouldBeTrue();
            command.Execution?.IsCompletedSuccessfully.ShouldBeFalse();
            command.Execution?.IsCanceled.ShouldBeFalse();
            command.Execution?.IsFaulted.ShouldBeFalse();

            completionSemaphore.Release();
            await command.Execution!.TaskCompleted;

            command.IsExecuting.ShouldBeFalse();
            command.Execution.ShouldNotBeNull();

            command.Execution?.IsCompleted.ShouldBeTrue();
            command.Execution?.IsNotCompleted.ShouldBeFalse();
            command.Execution?.IsCompletedSuccessfully.ShouldBeTrue();
            command.Execution?.IsCanceled.ShouldBeFalse();
            command.Execution?.IsFaulted.ShouldBeFalse();
        }

        [Fact]
        public async Task Execute_WithParameter_TransitionsToCorrectStateOnCancel()
        {
            using var runningSemaphore = new SemaphoreSlim(0);
            using var cancelSemaphore = new SemaphoreSlim(0);
            using var cts = new CancellationTokenSource();

            var command = new AsyncCommand(async o =>
            {
                runningSemaphore.Release();
                await cancelSemaphore.WaitAsync().ConfigureAwait(false);
                cts.Token.ThrowIfCancellationRequested();
            });

            command.IsExecuting.ShouldBeFalse();
            command.Execution.ShouldBeNull();

            command.Execute(new());
            runningSemaphore.Wait(TestContext.Current.CancellationToken);

            command.IsExecuting.ShouldBeTrue();
            command.Execution.ShouldNotBeNull();

            command.Execution?.IsCompleted.ShouldBeFalse();
            command.Execution?.IsNotCompleted.ShouldBeTrue();
            command.Execution?.IsCompletedSuccessfully.ShouldBeFalse();
            command.Execution?.IsCanceled.ShouldBeFalse();
            command.Execution?.IsFaulted.ShouldBeFalse();

            cts.Cancel();
            cancelSemaphore.Release();
            await command.Execution!.TaskCompleted;

            command.IsExecuting.ShouldBeFalse();
            command.Execution.ShouldNotBeNull();

            command.Execution?.IsCompleted.ShouldBeTrue();
            command.Execution?.IsNotCompleted.ShouldBeFalse();
            command.Execution?.IsCompletedSuccessfully.ShouldBeFalse();
            command.Execution?.IsCanceled.ShouldBeTrue();
            command.Execution?.IsFaulted.ShouldBeFalse();
        }

        [Fact]
        public async Task Execute_WithoutParameter_TransitionsToCorrectStateOnCancel()
        {
            using var runningSemaphore = new SemaphoreSlim(0);
            using var cancelSemaphore = new SemaphoreSlim(0);
            using var cts = new CancellationTokenSource();

            var command = new AsyncCommand(async () =>
            {
                runningSemaphore.Release();
                await cancelSemaphore.WaitAsync().ConfigureAwait(false);
                cts.Token.ThrowIfCancellationRequested();
            });

            command.IsExecuting.ShouldBeFalse();
            command.Execution.ShouldBeNull();

            command.Execute(new());
            runningSemaphore.Wait(TestContext.Current.CancellationToken);

            command.IsExecuting.ShouldBeTrue();
            command.Execution.ShouldNotBeNull();

            command.Execution?.IsCompleted.ShouldBeFalse();
            command.Execution?.IsNotCompleted.ShouldBeTrue();
            command.Execution?.IsCompletedSuccessfully.ShouldBeFalse();
            command.Execution?.IsCanceled.ShouldBeFalse();
            command.Execution?.IsFaulted.ShouldBeFalse();

            cts.Cancel();
            cancelSemaphore.Release();
            await command.Execution!.TaskCompleted;

            command.IsExecuting.ShouldBeFalse();
            command.Execution.ShouldNotBeNull();

            command.Execution?.IsCompleted.ShouldBeTrue();
            command.Execution?.IsNotCompleted.ShouldBeFalse();
            command.Execution?.IsCompletedSuccessfully.ShouldBeFalse();
            command.Execution?.IsCanceled.ShouldBeTrue();
            command.Execution?.IsFaulted.ShouldBeFalse();
        }

        [Fact]
        public async Task Execute_WithParameter_TransitionsToCorrectStateOnFailure()
        {
            using var runningSemaphore = new SemaphoreSlim(0);
            using var faultSemaphore = new SemaphoreSlim(0);

            var command = new AsyncCommand(async o =>
            {
                runningSemaphore.Release();
                await faultSemaphore.WaitAsync().ConfigureAwait(false);
                throw new InvalidOperationException("Example exception thrown from async command");
            });

            command.IsExecuting.ShouldBeFalse();
            command.Execution.ShouldBeNull();

            command.Execute(new());
            runningSemaphore.Wait(TestContext.Current.CancellationToken);

            command.IsExecuting.ShouldBeTrue();
            command.Execution.ShouldNotBeNull();

            command.Execution?.IsCompleted.ShouldBeFalse();
            command.Execution?.IsNotCompleted.ShouldBeTrue();
            command.Execution?.IsCompletedSuccessfully.ShouldBeFalse();
            command.Execution?.IsCanceled.ShouldBeFalse();
            command.Execution?.IsFaulted.ShouldBeFalse();

            faultSemaphore.Release();
            await command.Execution!.TaskCompleted;

            command.IsExecuting.ShouldBeFalse();
            command.Execution.ShouldNotBeNull();

            command.Execution?.IsCompleted.ShouldBeTrue();
            command.Execution?.IsNotCompleted.ShouldBeFalse();
            command.Execution?.IsCompletedSuccessfully.ShouldBeFalse();
            command.Execution?.IsCanceled.ShouldBeFalse();
            command.Execution?.IsFaulted.ShouldBeTrue();
        }

        [Fact]
        public async Task Execute_WithoutParameter_TransitionsToCorrectStateOnFailure()
        {
            using var runningSemaphore = new SemaphoreSlim(0);
            using var faultSemaphore = new SemaphoreSlim(0);

            var command = new AsyncCommand(async () =>
            {
                runningSemaphore.Release();
                await faultSemaphore.WaitAsync().ConfigureAwait(false);
                throw new InvalidOperationException("Example exception thrown from async command");
            });

            command.IsExecuting.ShouldBeFalse();
            command.Execution.ShouldBeNull();

            command.Execute(new());
            runningSemaphore.Wait(TestContext.Current.CancellationToken);

            command.IsExecuting.ShouldBeTrue();
            command.Execution.ShouldNotBeNull();

            command.Execution?.IsCompleted.ShouldBeFalse();
            command.Execution?.IsNotCompleted.ShouldBeTrue();
            command.Execution?.IsCompletedSuccessfully.ShouldBeFalse();
            command.Execution?.IsCanceled.ShouldBeFalse();
            command.Execution?.IsFaulted.ShouldBeFalse();

            faultSemaphore.Release();
            await command.Execution!.TaskCompleted;

            command.IsExecuting.ShouldBeFalse();
            command.Execution.ShouldNotBeNull();

            command.Execution?.IsCompleted.ShouldBeTrue();
            command.Execution?.IsNotCompleted.ShouldBeFalse();
            command.Execution?.IsCompletedSuccessfully.ShouldBeFalse();
            command.Execution?.IsCanceled.ShouldBeFalse();
            command.Execution?.IsFaulted.ShouldBeTrue();
        }
    }
}
