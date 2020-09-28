using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MN.Shell.MVVM.Tests
{
    [TestFixture]
    public class TaskNotifierGenericTests
    {
        [Test]
        public void AlreadyCompletedTaskTest()
        {
            var task = Task.FromResult(5);
            Assert.True(task.IsCompleted);

            var taskNotifier = task.ToTaskNotifier(123);

            CheckCompletedTaskNotifier(task, taskNotifier, 5);
        }

        [Test]
        public void AlreadyCanceledTaskTest()
        {
            var cancellationToken = new CancellationToken(true);
            var task = Task.FromCanceled<int>(cancellationToken);

            var taskNotifier = task.ToTaskNotifier(123);

            CheckCanceledTaskNotifier(task, taskNotifier, 123);
        }

        [Test]
        public void AlreadyFailedTaskTest()
        {
            var exception = new Exception("Exception message");
            var task = Task.FromException<int>(exception);

            var taskNotifier = task.ToTaskNotifier(123);

            CheckFaultedTaskNotifier(task, taskNotifier, exception, 123);
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
                    return 5;
                });

                var taskNotifier = task.ToTaskNotifier(123);

                runningSemaphore.Wait();

                CheckRunningTaskNotifier(task, taskNotifier, TaskStatus.Running, 123);

                var propertiesToNotify = new Dictionary<string, bool>
                {
                    { nameof(TaskNotifier<int>.Status), false },
                    { nameof(TaskNotifier<int>.IsCompleted), false },
                    { nameof(TaskNotifier<int>.IsNotCompleted), false },
                    { nameof(TaskNotifier<int>.Result), false },
                    { nameof(TaskNotifier<int>.IsCompletedSuccessfully), false },
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

                CheckCompletedTaskNotifier(task, taskNotifier, 5);
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

#pragma warning disable CS0162 // Unreachable code detected
                    return 5;
#pragma warning restore CS0162 // Unreachable code detected
                }, cancellationTokenSource.Token);

                var taskNotifier = task.ToTaskNotifier(123);

                runningSemaphore.Wait();

                CheckRunningTaskNotifier(task, taskNotifier, TaskStatus.Running, 123);

                var propertiesToNotify = new Dictionary<string, bool>
                {
                    { nameof(TaskNotifier<int>.Status), false },
                    { nameof(TaskNotifier<int>.IsCompleted), false },
                    { nameof(TaskNotifier<int>.IsNotCompleted), false },
                    { nameof(TaskNotifier<int>.IsCanceled), false },
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

                CheckCanceledTaskNotifier(task, taskNotifier, 123);
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
#pragma warning disable CS0162 // Unreachable code detected
                    return 5;
#pragma warning restore CS0162 // Unreachable code detected
                });

                var taskNotifier = task.ToTaskNotifier(123);

                runningSemaphore.Wait();

                CheckRunningTaskNotifier(task, taskNotifier, TaskStatus.Running, 123);

                var propertiesToNotify = new Dictionary<string, bool>
                {
                    { nameof(TaskNotifier<int>.Status), false },
                    { nameof(TaskNotifier<int>.IsCompleted), false },
                    { nameof(TaskNotifier<int>.IsNotCompleted), false },
                    { nameof(TaskNotifier<int>.IsFaulted), false },
                    { nameof(TaskNotifier<int>.Exception), false },
                    { nameof(TaskNotifier<int>.InnerException), false },
                    { nameof(TaskNotifier<int>.ErrorMessage), false },
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

                CheckFaultedTaskNotifier(task, taskNotifier, exception, 123);
            }
        }

        private static void CheckRunningTaskNotifier<T>(Task<T> task, TaskNotifier<T> taskNotifier,
            TaskStatus expectedTaskStatus, T expectedResult)
        {
            Assert.AreSame(task, taskNotifier.Task);

            Assert.NotNull(taskNotifier.TaskCompleted);
            Assert.False(taskNotifier.TaskCompleted.IsCompleted);

            Assert.AreEqual(expectedResult, taskNotifier.Result);

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

        private static void CheckCompletedTaskNotifier<T>(Task<T> task, TaskNotifier<T> taskNotifier, T expectedResult)
        {
            Assert.AreSame(task, taskNotifier.Task);

            Assert.NotNull(taskNotifier.TaskCompleted);
            Assert.True(taskNotifier.TaskCompleted.IsCompleted);

            Assert.AreEqual(expectedResult, taskNotifier.Result);

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

        private static void CheckCanceledTaskNotifier<T>(Task<T> task, TaskNotifier<T> taskNotifier, T expectedResult)
        {
            Assert.AreSame(task, taskNotifier.Task);

            Assert.NotNull(taskNotifier.TaskCompleted);
            Assert.True(taskNotifier.TaskCompleted.IsCompleted);

            Assert.AreEqual(expectedResult, taskNotifier.Result);

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

        private static void CheckFaultedTaskNotifier<T>(Task<T> task, TaskNotifier<T> taskNotifier,
            Exception exception, T expectedResult)
        {
            Assert.AreSame(task, taskNotifier.Task);

            Assert.NotNull(taskNotifier.TaskCompleted);
            Assert.True(taskNotifier.TaskCompleted.IsCompleted);

            Assert.AreEqual(expectedResult, taskNotifier.Result);

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
