using System;
using System.Threading.Tasks;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// Collection of extension methods for Task
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Creates new task notifier for observing given task
        /// </summary>
        /// <param name="task">Task to observe</param>
        /// <returns>Task notifier observing given task</returns>
        public static TaskNotifier ToTaskNotifier(this Task task) =>
            new TaskNotifier(task);

        /// <summary>
        /// Creates new task notifier for observing given asynchronous operation
        /// </summary>
        /// <param name="asyncAction">Asynchronous operation to observe</param>
        /// <returns>Task notifier observing given asynchronous operation</returns>
        public static TaskNotifier ToTaskNotifier(this Func<Task> asyncAction) =>
            new TaskNotifier(asyncAction);

        /// <summary>
        /// Creates new task notifier for observing given generic task
        /// </summary>
        /// <typeparam name="T">Type of task result</typeparam>
        /// <param name="task">Task to observe</param>
        /// <param name="defaultResult">Result to expose until task completes</param>
        /// <returns>Task notifier observing given task</returns>
        public static TaskNotifier<T> ToTaskNotifier<T>(this Task<T> task, T defaultResult) =>
            new TaskNotifier<T>(task, defaultResult);

        /// <summary>
        /// Creates new task notifier for observing given asynchronous operation
        /// </summary>
        /// <typeparam name="T">Type of asynchronous operation result</typeparam>
        /// <param name="asyncAction">Asynchronous operation to observe</param>
        /// <param name="defaultResult">Result to expose until task completes</param>
        /// <returns>Task notifier observing given asynchronous operation</returns>
        public static TaskNotifier<T> ToTaskNotifier<T>(this Func<Task<T>> asyncAction, T defaultResult) =>
            new TaskNotifier<T>(asyncAction, defaultResult);
    }
}
