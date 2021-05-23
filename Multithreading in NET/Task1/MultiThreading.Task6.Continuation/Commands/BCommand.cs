using System;
using System.Threading;
using System.Threading.Tasks;
using MultiThreading.Task6.Continuation.Interfaces;

namespace MultiThreading.Task6.Continuation.Commands
{
    public class BCommand : ICommand
    {
        public Task Run(Action mainAction, CancellationToken cancellationToken)
        {
            var task = Task.Run(mainAction, cancellationToken)
                .ContinueWith(x => CommandManager.ContinuationTaskExecution("B"), TaskContinuationOptions.NotOnRanToCompletion);
            return task;
        }
    }
}