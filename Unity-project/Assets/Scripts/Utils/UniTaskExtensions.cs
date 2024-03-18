using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Debug = UnityEngine.Debug;

namespace Bohemia
{
    internal static class UniTaskExtensions
    {
        private static readonly Action<Task> _handleFinishedTask = HandleFinishedTask;

        private static void HandleExceptions(this Task task)
        {
            task.ContinueWith(_handleFinishedTask);
        }

        private static void HandleFinishedTask(Task task)
        {
            if (task.IsFaulted)
                Debug.LogException(task.Exception);
        }

        /// <summary>
        /// async void causes silent exceptions, use this method to handle them.
        /// https://medium.com/c-sharp-progarmming/the-problem-with-async-void-9f40b613cba
        /// </summary>
        internal static void HandleExceptions(this UniTask task)
        {
            HandleExceptions(task.AsTask());
        }
    }
}