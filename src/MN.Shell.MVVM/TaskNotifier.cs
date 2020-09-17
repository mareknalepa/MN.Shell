using System;
using System.Threading.Tasks;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// Wrapper for asynchronous task, which enables easy data binding to observe its state
    /// </summary>
    public sealed class TaskNotifier : PropertyChangedBase
    {
        /// <summary>
        /// Wrapped task
        /// </summary>
        public Task Task { get; }

        /// <summary>
        /// Task that is completed successfully whenever observed task completes (successfully or not)
        /// </summary>
        public Task TaskCompleted { get; }

        /// <summary>
        /// Task current status
        /// </summary>
        public TaskStatus Status => Task.Status;

        /// <summary>
        /// Returns if task is completed
        /// </summary>
        public bool IsCompleted => Task.IsCompleted;

        /// <summary>
        /// Returns if task is not yet completed (i.e. is ongoing)
        /// </summary>
        public bool IsNotCompleted => !Task.IsCompleted;

        /// <summary>
        /// Returns if task is completed successfully
        /// </summary>
        public bool IsCompletedSuccessfully => Task.Status == TaskStatus.RanToCompletion;

        /// <summary>
        /// Returns if task is canceled
        /// </summary>
        public bool IsCanceled => Task.IsCanceled;

        /// <summary>
        /// Returns if task is faulted
        /// </summary>
        public bool IsFaulted => Task.IsFaulted;

        /// <summary>
        /// Returns wrapped exception thrown by task (null if no exceptions have been thrown)
        /// </summary>
        public AggregateException Exception => Task.Exception;

        /// <summary>
        /// Returns the original exception thrown by task (null if no exception has been thrown)
        /// </summary>
        public Exception InnerException => Exception?.InnerException;

        /// <summary>
        /// Gets the error message for the original exception thrown by task
        /// </summary>
        public string ErrorMessage => InnerException?.Message;

        /// <summary>
        /// Creates new TaskNotifier to observe given asynchronous task
        /// </summary>
        /// <param name="task">Task to observe</param>
        public TaskNotifier(Task task)
        {
            Task = task ?? throw new ArgumentNullException(nameof(task));
            TaskCompleted = WrapTaskAsync(task);
        }

        /// <summary>
        /// Creates new TaskNotifier to observe given asynchronous action
        /// </summary>
        /// <param name="asyncAction">Asynchronous action to observe</param>
        public TaskNotifier(Func<Task> asyncAction) :
            this(asyncAction?.Invoke() ?? throw new ArgumentNullException(nameof(asyncAction)))
        { }

        private async Task WrapTaskAsync(Task task)
        {
            try
            {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
                await task;
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
            }
            catch
            {
                throw;
            }
            finally
            {
                NotifyPropertiesChanged();
            }
        }

        private void NotifyPropertiesChanged()
        {
            NotifyPropertyChanged(nameof(Status));
            NotifyPropertyChanged(nameof(IsCompleted));
            NotifyPropertyChanged(nameof(IsNotCompleted));

            if (Task.IsCanceled)
            {
                NotifyPropertyChanged(nameof(IsCanceled));
            }
            else if (Task.IsFaulted)
            {
                NotifyPropertyChanged(nameof(IsFaulted));
                NotifyPropertyChanged(nameof(Exception));
                NotifyPropertyChanged(nameof(InnerException));
                NotifyPropertyChanged(nameof(ErrorMessage));
            }
            else
            {
                NotifyPropertyChanged(nameof(IsCompletedSuccessfully));
            }
        }
    }

    /// <summary>
    /// Wrapper for asynchronous task (generic), which enables easy data binding to observe its state
    /// </summary>
    /// <typeparam name="T">Type of task result</typeparam>
    public sealed class TaskNotifier<T> : PropertyChangedBase
    {
        private readonly T _defaultResult;

        /// <summary>
        /// Wrapped task
        /// </summary>
        public Task<T> Task { get; }

        /// <summary>
        /// Task that is completed successfully whenever observed task completes (successfully or not)
        /// </summary>
        public Task TaskCompleted { get; }

        /// <summary>
        /// Result of the successfully completed task or default result until task successfully completes
        /// </summary>
        public T Result => Task.Status == TaskStatus.RanToCompletion ? Task.Result : _defaultResult;

        /// <summary>
        /// Task current status
        /// </summary>
        public TaskStatus Status => Task.Status;

        /// <summary>
        /// Returns if task is completed
        /// </summary>
        public bool IsCompleted => Task.IsCompleted;

        /// <summary>
        /// Returns if task is not yet completed (i.e. is ongoing)
        /// </summary>
        public bool IsNotCompleted => !Task.IsCompleted;

        /// <summary>
        /// Returns if task is completed successfully
        /// </summary>
        public bool IsCompletedSuccessfully => Task.Status == TaskStatus.RanToCompletion;

        /// <summary>
        /// Returns if task is canceled
        /// </summary>
        public bool IsCanceled => Task.IsCanceled;

        /// <summary>
        /// Returns if task is faulted
        /// </summary>
        public bool IsFaulted => Task.IsFaulted;

        /// <summary>
        /// Returns wrapped exception thrown by task (null if no exceptions have been thrown)
        /// </summary>
        public AggregateException Exception => Task.Exception;

        /// <summary>
        /// Returns the original exception thrown by task (null if no exception has been thrown)
        /// </summary>
        public Exception InnerException => Exception?.InnerException;

        /// <summary>
        /// Gets the error message for the original exception thrown by task
        /// </summary>
        public string ErrorMessage => InnerException?.Message;

        /// <summary>
        /// Creates new TaskNotifier to observe given asynchronous task
        /// </summary>
        /// <param name="task">Task to observe</param>
        /// <param name="defaultResult">Result to expose until task completes</param>
        public TaskNotifier(Task<T> task, T defaultResult = default)
        {
            _defaultResult = defaultResult;
            Task = task ?? throw new ArgumentNullException(nameof(task));
            TaskCompleted = WrapTaskAsync(task);
        }

        /// <summary>
        /// Creates new TaskNotifier to observe given asynchronous action
        /// </summary>
        /// <param name="asyncAction">Asynchronous action to observe</param>
        public TaskNotifier(Func<Task<T>> asyncAction) :
            this(asyncAction?.Invoke() ?? throw new ArgumentNullException(nameof(asyncAction)))
        { }

        private async Task WrapTaskAsync(Task<T> task)
        {
            try
            {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
                await task;
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
            }
            catch
            {
                throw;
            }
            finally
            {
                NotifyPropertiesChanged();
            }
        }

        private void NotifyPropertiesChanged()
        {
            NotifyPropertyChanged(nameof(Status));
            NotifyPropertyChanged(nameof(IsCompleted));
            NotifyPropertyChanged(nameof(IsNotCompleted));

            if (Task.IsCanceled)
            {
                NotifyPropertyChanged(nameof(IsCanceled));
            }
            else if (Task.IsFaulted)
            {
                NotifyPropertyChanged(nameof(IsFaulted));
                NotifyPropertyChanged(nameof(Exception));
                NotifyPropertyChanged(nameof(InnerException));
                NotifyPropertyChanged(nameof(ErrorMessage));
            }
            else
            {
                NotifyPropertyChanged(nameof(Result));
                NotifyPropertyChanged(nameof(IsCompletedSuccessfully));
            }
        }
    }
}
