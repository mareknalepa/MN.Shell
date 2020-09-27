using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MN.Shell.MVVM.Tests
{
    [TestFixture]
    public class AsyncCommandTests
    {
        [Test]
        public void CanExecuteWithParameterTest()
        {
            bool canExecuteFired = false;
            bool canExecute = false;

            var command = new AsyncCommand(o => Task.CompletedTask, o =>
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

            var command = new AsyncCommand(() => Task.CompletedTask, () =>
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

            var command = new AsyncCommand(o =>
            {
                executeFired = true;
                return Task.CompletedTask;
            }, o => canExecute);

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

            var command = new AsyncCommand(() =>
            {
                executeFired = true;
                return Task.CompletedTask;
            }, () => canExecute);

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

            var command = new AsyncCommand(o =>
            {
                executeFired = true;
                return Task.CompletedTask;
            });

            Assert.True(command.CanExecute(new object()));
            Assert.False(executeFired);

            command.Execute(new object());
            Assert.True(executeFired);
        }

        [Test]
        public void CanExecuteWithoutParameterWithoutDelegateIsTrueByDefaultTest()
        {
            bool executeFired = false;

            var command = new AsyncCommand(() =>
            {
                executeFired = true;
                return Task.CompletedTask;
            });

            Assert.True(command.CanExecute(new object()));
            Assert.False(executeFired);

            command.Execute(new object());
            Assert.True(executeFired);
        }

        [Test]
        public void CannotExecuteWhileCommandIsRunningWithParameterTest()
        {
            using (var runningSemaphore = new SemaphoreSlim(0))
            using (var completionSemaphore = new SemaphoreSlim(0))
            {
                var command = new AsyncCommand(async o =>
                {
                    runningSemaphore.Release();
                    await completionSemaphore.WaitAsync().ConfigureAwait(false);
                });

                Assert.True(command.CanExecute(new object()));

                command.Execute(new object());
                runningSemaphore.Wait();

                Assert.False(command.CanExecute(new object()));

                completionSemaphore.Release();
                command.Execution.TaskCompleted.Wait();

                Assert.True(command.CanExecute(new object()));
            }
        }

        [Test]
        public void CannotExecuteWhileCommandIsRunningWithoutParameterTest()
        {
            using (var runningSemaphore = new SemaphoreSlim(0))
            using (var completionSemaphore = new SemaphoreSlim(0))
            {
                var command = new AsyncCommand(async () =>
                {
                    runningSemaphore.Release();
                    await completionSemaphore.WaitAsync().ConfigureAwait(false);
                });

                Assert.True(command.CanExecute(new object()));

                command.Execute(new object());
                runningSemaphore.Wait();

                Assert.False(command.CanExecute(new object()));

                completionSemaphore.Release();
                command.Execution.TaskCompleted.Wait();

                Assert.True(command.CanExecute(new object()));
            }
        }

        [Test]
        public void SuccessfullCommandWithParametersTest()
        {
            using (var runningSemaphore = new SemaphoreSlim(0))
            using (var completionSemaphore = new SemaphoreSlim(0))
            {
                var command = new AsyncCommand(async o =>
                {
                    runningSemaphore.Release();
                    await completionSemaphore.WaitAsync().ConfigureAwait(false);
                });

                command.Execute(new object());
                runningSemaphore.Wait();

                Assert.False(command.Execution.IsCompleted);
                Assert.True(command.Execution.IsNotCompleted);
                Assert.False(command.Execution.IsCompletedSuccessfully);
                Assert.False(command.Execution.IsCanceled);
                Assert.False(command.Execution.IsFaulted);

                completionSemaphore.Release();
                command.Execution.TaskCompleted.Wait();

                Assert.True(command.Execution.IsCompleted);
                Assert.False(command.Execution.IsNotCompleted);
                Assert.True(command.Execution.IsCompletedSuccessfully);
                Assert.False(command.Execution.IsCanceled);
                Assert.False(command.Execution.IsFaulted);
            }
        }

        [Test]
        public void SuccessfullCommandWithoutParametersTest()
        {
            using (var runningSemaphore = new SemaphoreSlim(0))
            using (var completionSemaphore = new SemaphoreSlim(0))
            {
                var command = new AsyncCommand(async () =>
                {
                    runningSemaphore.Release();
                    await completionSemaphore.WaitAsync().ConfigureAwait(false);
                });

                command.Execute(new object());
                runningSemaphore.Wait();

                Assert.False(command.Execution.IsCompleted);
                Assert.True(command.Execution.IsNotCompleted);
                Assert.False(command.Execution.IsCompletedSuccessfully);
                Assert.False(command.Execution.IsCanceled);
                Assert.False(command.Execution.IsFaulted);

                completionSemaphore.Release();
                command.Execution.TaskCompleted.Wait();

                Assert.True(command.Execution.IsCompleted);
                Assert.False(command.Execution.IsNotCompleted);
                Assert.True(command.Execution.IsCompletedSuccessfully);
                Assert.False(command.Execution.IsCanceled);
                Assert.False(command.Execution.IsFaulted);
            }
        }

        [Test]
        public void CanceledCommandWithParametersTest()
        {
            using (var runningSemaphore = new SemaphoreSlim(0))
            using (var cancelSemaphore = new SemaphoreSlim(0))
            using (var cts = new CancellationTokenSource())
            {
                var command = new AsyncCommand(async o =>
                {
                    runningSemaphore.Release();
                    await cancelSemaphore.WaitAsync().ConfigureAwait(false);
                    cts.Token.ThrowIfCancellationRequested();
                });

                command.Execute(new object());
                runningSemaphore.Wait();

                Assert.False(command.Execution.IsCompleted);
                Assert.True(command.Execution.IsNotCompleted);
                Assert.False(command.Execution.IsCompletedSuccessfully);
                Assert.False(command.Execution.IsCanceled);
                Assert.False(command.Execution.IsFaulted);

                cts.Cancel();
                cancelSemaphore.Release();
                command.Execution.TaskCompleted.Wait();

                Assert.True(command.Execution.IsCompleted);
                Assert.False(command.Execution.IsNotCompleted);
                Assert.False(command.Execution.IsCompletedSuccessfully);
                Assert.True(command.Execution.IsCanceled);
                Assert.False(command.Execution.IsFaulted);
            }
        }

        [Test]
        public void CanceledCommandWithoutParametersTest()
        {
            using (var runningSemaphore = new SemaphoreSlim(0))
            using (var cancelSemaphore = new SemaphoreSlim(0))
            using (var cts = new CancellationTokenSource())
            {
                var command = new AsyncCommand(async () =>
                {
                    runningSemaphore.Release();
                    await cancelSemaphore.WaitAsync().ConfigureAwait(false);
                    cts.Token.ThrowIfCancellationRequested();
                });

                command.Execute(new object());
                runningSemaphore.Wait();

                Assert.False(command.Execution.IsCompleted);
                Assert.True(command.Execution.IsNotCompleted);
                Assert.False(command.Execution.IsCompletedSuccessfully);
                Assert.False(command.Execution.IsCanceled);
                Assert.False(command.Execution.IsFaulted);

                cts.Cancel();
                cancelSemaphore.Release();
                command.Execution.TaskCompleted.Wait();

                Assert.True(command.Execution.IsCompleted);
                Assert.False(command.Execution.IsNotCompleted);
                Assert.False(command.Execution.IsCompletedSuccessfully);
                Assert.True(command.Execution.IsCanceled);
                Assert.False(command.Execution.IsFaulted);
            }
        }

        [Test]
        public void FaultedCommandWithParametersTest()
        {
            using (var runningSemaphore = new SemaphoreSlim(0))
            using (var faultSemaphore = new SemaphoreSlim(0))
            {
                var command = new AsyncCommand(async o =>
                {
                    runningSemaphore.Release();
                    await faultSemaphore.WaitAsync().ConfigureAwait(false);
                    throw new InvalidOperationException("Example exception thrown from async command");
                });

                command.Execute(new object());
                runningSemaphore.Wait();

                Assert.False(command.Execution.IsCompleted);
                Assert.True(command.Execution.IsNotCompleted);
                Assert.False(command.Execution.IsCompletedSuccessfully);
                Assert.False(command.Execution.IsCanceled);
                Assert.False(command.Execution.IsFaulted);

                faultSemaphore.Release();
                command.Execution.TaskCompleted.Wait();

                Assert.True(command.Execution.IsCompleted);
                Assert.False(command.Execution.IsNotCompleted);
                Assert.False(command.Execution.IsCompletedSuccessfully);
                Assert.False(command.Execution.IsCanceled);
                Assert.True(command.Execution.IsFaulted);
                Assert.AreEqual(typeof(InvalidOperationException), command.Execution.InnerException.GetType());
                Assert.AreEqual("Example exception thrown from async command", command.Execution.ErrorMessage);
            }
        }

        [Test]
        public void FaultedCommandWithoutParametersTest()
        {
            using (var runningSemaphore = new SemaphoreSlim(0))
            using (var faultSemaphore = new SemaphoreSlim(0))
            {
                var command = new AsyncCommand(async () =>
                {
                    runningSemaphore.Release();
                    await faultSemaphore.WaitAsync().ConfigureAwait(false);
                    throw new InvalidOperationException("Example exception thrown from async command");
                });

                command.Execute(new object());
                runningSemaphore.Wait();

                Assert.False(command.Execution.IsCompleted);
                Assert.True(command.Execution.IsNotCompleted);
                Assert.False(command.Execution.IsCompletedSuccessfully);
                Assert.False(command.Execution.IsCanceled);
                Assert.False(command.Execution.IsFaulted);

                faultSemaphore.Release();
                command.Execution.TaskCompleted.Wait();

                Assert.True(command.Execution.IsCompleted);
                Assert.False(command.Execution.IsNotCompleted);
                Assert.False(command.Execution.IsCompletedSuccessfully);
                Assert.False(command.Execution.IsCanceled);
                Assert.True(command.Execution.IsFaulted);
                Assert.AreEqual(typeof(InvalidOperationException), command.Execution.InnerException.GetType());
                Assert.AreEqual("Example exception thrown from async command", command.Execution.ErrorMessage);
            }
        }
    }
}
