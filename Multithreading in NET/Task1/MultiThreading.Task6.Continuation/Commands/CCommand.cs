using System;
using System.Threading;
using System.Threading.Tasks;
using MultiThreading.Task6.Continuation.Interfaces;

namespace MultiThreading.Task6.Continuation.Commands
{
    public class CCommand : ICommand
    {
        public Task Run(Action mainAction, CancellationToken cancellationToken)
        {
            var task = Task.Run(mainAction, cancellationToken)
                .ContinueWith(x => CommandManager.ContinuationTaskExecution("C"), TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
            return task;
        }
    }
}