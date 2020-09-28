using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MN.Shell.MVVM.Tests
{
    [TestFixture]
    public class TaskNotifierTests
    {
        [Test]
        public void AlreadyCompletedTaskTest()
        {
            var task = Task.CompletedTask;
            Assert.True(task.IsCompleted);

            var taskNotifier = task.ToTaskNotifier();

            CheckCompletedTaskNotifier(task, taskNotifier);
        }

        [Test]
        public void AlreadyCanceledTaskTest()
        {
            var cancellationToken = new CancellationToken(true);
            var task = Task.FromCanceled(cancellationToken);

            var taskNotifier = task.ToTaskNotifier();

            CheckCanceledTaskNotifier(task, taskNotifier);
        }

        [Test]
        public void AlreadyFailedTaskTest()
        {
            var exception = new Exception("Exception message");
            var task = Task.FromException(exception);

            var taskNotifier = task.ToTaskNotifier();

            CheckFaultedTaskNotifier(task, taskNotifier, exception);
        }

        [Test]
        public void NotifyUponCompletionTest()
        {
            using (var runningSemaphore = new SemaphoreSlim(0))
            using (var completionSemaphore = new SemaphoreSlim(0))
            {
                var task = Task.Run(() =>
                {
                    runningSemaphore.Release();
                    completionSemaphore.Wait();
                });

                var taskNotifier = task.ToTaskNotifier();

                runningSemaphore.Wait();

                CheckRunningTaskNotifier(task, taskNotifier, TaskStatus.Running);

                var propertiesToNotify = new Dictionary<string, bool>
                {
                    { nameof(TaskNotifier.Status), false },
                    { nameof(TaskNotifier.IsCompleted), false },
                    { nameof(TaskNotifier.IsNotCompleted), false },
                    { nameof(TaskNotifier.IsCompletedSuccessfully), false },
                };

                taskNotifier.PropertyChanged += (sender, e) =>
                {
                    if (propertiesToNotify.ContainsKey(e.PropertyName))
                        propertiesToNotify[e.PropertyName] = true;
                    else
                        Assert.Fail($"Unexpected PropertyChanged notification: {e.PropertyName}");
                };

                completionSemaphore.Release();
                taskNotifier.TaskCompleted.Wait();

                Assert.True(propertiesToNotify.All(kvp => kvp.Value));

                CheckCompletedTaskNotifier(task, taskNotifier);
            }
        }

        [Test]
        public void NotifyUponCancelTest()
        {
            using (var runningSemaphore = new SemaphoreSlim(0))
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var token = cancellationTokenSource.Token;
                var task = Task.Run(() =>
                {
                    runningSemaphore.Release();
                    token.ThrowIfCancellationRequested();

                    while (true)
                    {
                        token.ThrowIfCancellationRequested();
                        Thread.Sleep(1);
                    }
                }, cancellationTokenSource.Token);

                var taskNotifier = task.ToTaskNotifier();

                runningSemaphore.Wait();

                CheckRunningTaskNotifier(task, taskNotifier, TaskStatus.WaitingForActivation);

                var propertiesToNotify = new Dictionary<string, bool>
                {
                    { nameof(TaskNotifier.Status), false },
                    { nameof(TaskNotifier.IsCompleted), false },
                    { nameof(TaskNotifier.IsNotCompleted), false },
                    { nameof(TaskNotifier.IsCanceled), false },
                };

                taskNotifier.PropertyChanged += (sender, e) =>
                {
                    if (propertiesToNotify.ContainsKey(e.PropertyName))
                        propertiesToNotify[e.PropertyName] = true;
                    else
                        Assert.Fail($"Unexpected PropertyChanged notification: {e.PropertyName}");
                };

                cancellationTokenSource.Cancel();
                taskNotifier.TaskCompleted.Wait();

                Assert.True(propertiesToNotify.All(kvp => kvp.Value));

                CheckCanceledTaskNotifier(task, taskNotifier);
            }
        }

        [Test]
        public void NotifyUponFailingTest()
        {
            using (var runningSemaphore = new SemaphoreSlim(0))
            using (var failingSemaphore = new SemaphoreSlim(0))
            {
                var exception = new Exception("Exception message");
                var task = Task.Run(() =>
                {
                    runningSemaphore.Release();
                    failingSemaphore.Wait();
                    throw exception;
                });

                var taskNotifier = task.ToTaskNotifier();

                runningSemaphore.Wait();

                CheckRunningTaskNotifier(task, taskNotifier, TaskStatus.WaitingForActivation);

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
                    if (propertiesToNotify.ContainsKey(e.PropertyName))
                        propertiesToNotify[e.PropertyName] = true;
                    else
                        Assert.Fail($"Unexpected PropertyChanged notification: {e.PropertyName}");
                };

                failingSemaphore.Release();
                taskNotifier.TaskCompleted.Wait();

                Assert.True(propertiesToNotify.All(kvp => kvp.Value));

                CheckFaultedTaskNotifier(task, taskNotifier, exception);
            }
        }

        private static void CheckRunningTaskNotifier(Task task, TaskNotifier taskNotifier,
            TaskStatus expectedTaskStatus)
        {
            Assert.AreSame(task, taskNotifier.Task);

            Assert.NotNull(taskNotifier.TaskCompleted);
            Assert.False(taskNotifier.TaskCompleted.IsCompleted);

            Assert.AreEqual(expectedTaskStatus, taskNotifier.Status);
            Assert.False(taskNotifier.IsCompleted);
            Assert.True(taskNotifier.IsNotCompleted);
            Assert.False(taskNotifier.IsCompletedSuccessfully);
            Assert.False(taskNotifier.IsCanceled);
            Assert.False(taskNotifier.IsFaulted);
            Assert.Null(taskNotifier.Exception);
            Assert.Null(taskNotifier.InnerException);
            Assert.Null(taskNotifier.ErrorMessage);
        }

        private static void CheckCompletedTaskNotifier(Task task, TaskNotifier taskNotifier)
        {
            Assert.AreSame(task, taskNotifier.Task);

            Assert.NotNull(taskNotifier.TaskCompleted);
            Assert.True(taskNotifier.TaskCompleted.IsCompleted);

            Assert.AreEqual(TaskStatus.RanToCompletion, taskNotifier.Status);
            Assert.True(taskNotifier.IsCompleted);
            Assert.False(taskNotifier.IsNotCompleted);
            Assert.True(taskNotifier.IsCompletedSuccessfully);
            Assert.False(taskNotifier.IsCanceled);
            Assert.False(taskNotifier.IsFaulted);
            Assert.Null(taskNotifier.Exception);
            Assert.Null(taskNotifier.InnerException);
            Assert.Null(taskNotifier.ErrorMessage);
        }

        private static void CheckCanceledTaskNotifier(Task task, TaskNotifier taskNotifier)
        {
            Assert.AreSame(task, taskNotifier.Task);

            Assert.NotNull(taskNotifier.TaskCompleted);
            Assert.True(taskNotifier.TaskCompleted.IsCompleted);

            Assert.AreEqual(TaskStatus.Canceled, taskNotifier.Status);
            Assert.True(taskNotifier.IsCompleted);
            Assert.False(taskNotifier.IsNotCompleted);
            Assert.False(taskNotifier.IsCompletedSuccessfully);
            Assert.True(taskNotifier.IsCanceled);
            Assert.False(taskNotifier.IsFaulted);
            Assert.Null(taskNotifier.Exception);
            Assert.Null(taskNotifier.InnerException);
            Assert.Null(taskNotifier.ErrorMessage);
        }

        private static void CheckFaultedTaskNotifier(Task task, TaskNotifier taskNotifier, Exception exception)
        {
            Assert.AreSame(task, taskNotifier.Task);

            Assert.NotNull(taskNotifier.TaskCompleted);
            Assert.True(taskNotifier.TaskCompleted.IsCompleted);

            Assert.AreEqual(TaskStatus.Faulted, taskNotifier.Status);
            Assert.True(taskNotifier.IsCompleted);
            Assert.False(taskNotifier.IsNotCompleted);
            Assert.False(taskNotifier.IsCompletedSuccessfully);
            Assert.False(taskNotifier.IsCanceled);
            Assert.True(taskNotifier.IsFaulted);
            Assert.NotNull(taskNotifier.Exception);
            Assert.AreSame(exception, taskNotifier.InnerException);
            Assert.AreEqual(exception.Message, taskNotifier.ErrorMessage);
        }
    }
}
