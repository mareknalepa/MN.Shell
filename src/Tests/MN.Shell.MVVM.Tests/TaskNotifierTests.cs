namespace MN.Shell.MVVM.Tests
{
    public sealed class TaskNotifierTests
    {
        [Fact]
        public void ToTaskNotifier_FromCompletedTask_ReturnsNotifierInCorrectState()
        {
            var task = Task.CompletedTask;
            Assert.True(task.IsCompleted);

            var taskNotifier = task.ToTaskNotifier();

            CheckCompletedTaskNotifier(task, taskNotifier);
        }

        [Fact]
        public void ToTaskNotifier_FromCanceledTask_ReturnsNotifierInCorrectState()
        {
            var cancellationToken = new CancellationToken(true);
            var task = Task.FromCanceled(cancellationToken);

            var taskNotifier = task.ToTaskNotifier();

            CheckCanceledTaskNotifier(task, taskNotifier);
        }

        [Fact]
        public void ToTaskNotifier_FromFailedTask_ReturnsNotifierInCorrectState()
        {
            var exception = new Exception("Exception message");
            var task = Task.FromException(exception);

            var taskNotifier = task.ToTaskNotifier();

            CheckFaultedTaskNotifier(task, taskNotifier, exception);
        }

        [Fact]
        public async Task TaskCompletion_RaisesPropertyChanged()
        {
            using var runningSemaphore = new SemaphoreSlim(0);
            using var completionSemaphore = new SemaphoreSlim(0);
            var task = Task.Run(async () =>
            {
                runningSemaphore.Release();
                await completionSemaphore.WaitAsync();
            }, TestContext.Current.CancellationToken);

            var taskNotifier = task.ToTaskNotifier();

            await runningSemaphore.WaitAsync(TestContext.Current.CancellationToken);

            CheckRunningTaskNotifier(task, taskNotifier);

            var propertiesToNotify = new Dictionary<string, bool>
                {
                    { nameof(TaskNotifier.Status), false },
                    { nameof(TaskNotifier.IsCompleted), false },
                    { nameof(TaskNotifier.IsNotCompleted), false },
                    { nameof(TaskNotifier.IsCompletedSuccessfully), false },
                };

            taskNotifier.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName is not null && propertiesToNotify.ContainsKey(e.PropertyName))
                {
                    propertiesToNotify[e.PropertyName] = true;
                }
                else
                {
                    Assert.Fail($"Unexpected PropertyChanged notification: {e.PropertyName}");
                }
            };

            completionSemaphore.Release();
            await taskNotifier.TaskCompleted;

            propertiesToNotify.ShouldAllBe(kvp => kvp.Value);

            CheckCompletedTaskNotifier(task, taskNotifier);
        }

        [Fact]
        public async Task TaskCancelation_RaisesPropertyChanged()
        {
            using var runningSemaphore = new SemaphoreSlim(0);
            using var cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            var task = Task.Run(async () =>
            {
                runningSemaphore.Release();

                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1);
                }
            }, cancellationTokenSource.Token);

            var taskNotifier = task.ToTaskNotifier();

            await runningSemaphore.WaitAsync(TestContext.Current.CancellationToken);

            CheckRunningTaskNotifier(task, taskNotifier);

            var propertiesToNotify = new Dictionary<string, bool>
                {
                    { nameof(TaskNotifier.Status), false },
                    { nameof(TaskNotifier.IsCompleted), false },
                    { nameof(TaskNotifier.IsNotCompleted), false },
                    { nameof(TaskNotifier.IsCanceled), false },
                };

            taskNotifier.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName is not null && propertiesToNotify.ContainsKey(e.PropertyName))
                {
                    propertiesToNotify[e.PropertyName] = true;
                }
                else
                {
                    Assert.Fail($"Unexpected PropertyChanged notification: {e.PropertyName}");
                }
            };

            cancellationTokenSource.Cancel();
            await taskNotifier.TaskCompleted;

            propertiesToNotify.ShouldAllBe(kvp => kvp.Value);

            CheckCanceledTaskNotifier(task, taskNotifier);
        }

        [Fact]
        public async Task TaskFailure_RaisesPropertyChanged()
        {
            using var runningSemaphore = new SemaphoreSlim(0);
            using var failingSemaphore = new SemaphoreSlim(0);
            var exception = new Exception("Exception message");
            var task = Task.Run(async () =>
            {
                runningSemaphore.Release();
                await failingSemaphore.WaitAsync();
                throw exception;
            }, TestContext.Current.CancellationToken);

            var taskNotifier = task.ToTaskNotifier();

            await runningSemaphore.WaitAsync(TestContext.Current.CancellationToken);

            CheckRunningTaskNotifier(task, taskNotifier);

            var propertiesToNotify = new Dictionary<string, bool>
                {
                    { nameof(TaskNotifier.Status), false },
                    { nameof(TaskNotifier.IsCompleted), false },
                    { nameof(TaskNotifier.IsNotCompleted), false },
                    { nameof(TaskNotifier.IsFaulted), false },
                    { nameof(TaskNotifier.Exception), false },
                    { nameof(TaskNotifier.InnerException), false },
                    { nameof(TaskNotifier.ErrorMessage), false },
                };

            taskNotifier.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName is not null && propertiesToNotify.ContainsKey(e.PropertyName))
                {
                    propertiesToNotify[e.PropertyName] = true;
                }
                else
                {
                    Assert.Fail($"Unexpected PropertyChanged notification: {e.PropertyName}");
                }
            };

            failingSemaphore.Release();
            await taskNotifier.TaskCompleted;

            Assert.True(propertiesToNotify.All(kvp => kvp.Value));

            CheckFaultedTaskNotifier(task, taskNotifier, exception);
        }

        private static void CheckRunningTaskNotifier(Task task, TaskNotifier taskNotifier)
        {
            taskNotifier.Task.ShouldBeSameAs(task);

            taskNotifier.TaskCompleted.ShouldNotBeNull();
            taskNotifier.TaskCompleted.IsCompleted.ShouldBeFalse();

            taskNotifier.IsCompleted.ShouldBeFalse();
            taskNotifier.IsNotCompleted.ShouldBeTrue();
            taskNotifier.IsCompletedSuccessfully.ShouldBeFalse();
            taskNotifier.IsCanceled.ShouldBeFalse();
            taskNotifier.IsFaulted.ShouldBeFalse();
            taskNotifier.Exception.ShouldBeNull();
            taskNotifier.InnerException.ShouldBeNull();
            taskNotifier.ErrorMessage.ShouldBeNull();
        }

        private static void CheckCompletedTaskNotifier(Task task, TaskNotifier taskNotifier)
        {
            taskNotifier.Task.ShouldBeSameAs(task);

            taskNotifier.TaskCompleted.ShouldNotBeNull();
            taskNotifier.TaskCompleted.IsCompleted.ShouldBeTrue();

            taskNotifier.Status.ShouldBe(TaskStatus.RanToCompletion);
            taskNotifier.IsCompleted.ShouldBeTrue();
            taskNotifier.IsNotCompleted.ShouldBeFalse();
            taskNotifier.IsCompletedSuccessfully.ShouldBeTrue();
            taskNotifier.IsCanceled.ShouldBeFalse();
            taskNotifier.IsFaulted.ShouldBeFalse();
            taskNotifier.Exception.ShouldBeNull();
            taskNotifier.InnerException.ShouldBeNull();
            taskNotifier.ErrorMessage.ShouldBeNull();
        }

        private static void CheckCanceledTaskNotifier(Task task, TaskNotifier taskNotifier)
        {
            taskNotifier.Task.ShouldBeSameAs(task);

            taskNotifier.TaskCompleted.ShouldNotBeNull();
            taskNotifier.TaskCompleted.IsCompleted.ShouldBeTrue();

            taskNotifier.Status.ShouldBe(TaskStatus.Canceled);
            taskNotifier.IsCompleted.ShouldBeTrue();
            taskNotifier.IsNotCompleted.ShouldBeFalse();
            taskNotifier.IsCompletedSuccessfully.ShouldBeFalse();
            taskNotifier.IsCanceled.ShouldBeTrue();
            taskNotifier.IsFaulted.ShouldBeFalse();
            taskNotifier.Exception.ShouldBeNull();
            taskNotifier.InnerException.ShouldBeNull();
            taskNotifier.ErrorMessage.ShouldBeNull();
        }

        private static void CheckFaultedTaskNotifier(Task task, TaskNotifier taskNotifier, Exception exception)
        {
            taskNotifier.Task.ShouldBeSameAs(task);

            taskNotifier.TaskCompleted.ShouldNotBeNull();
            taskNotifier.TaskCompleted.IsCompleted.ShouldBeTrue();

            taskNotifier.Status.ShouldBe(TaskStatus.Faulted);
            taskNotifier.IsCompleted.ShouldBeTrue();
            taskNotifier.IsNotCompleted.ShouldBeFalse();
            taskNotifier.IsCompletedSuccessfully.ShouldBeFalse();
            taskNotifier.IsCanceled.ShouldBeFalse();
            taskNotifier.IsFaulted.ShouldBeTrue();
            taskNotifier.Exception.ShouldNotBeNull();
            taskNotifier.InnerException.ShouldBeSameAs(exception);
            taskNotifier.ErrorMessage.ShouldBe(exception.Message);
        }
    }
}
