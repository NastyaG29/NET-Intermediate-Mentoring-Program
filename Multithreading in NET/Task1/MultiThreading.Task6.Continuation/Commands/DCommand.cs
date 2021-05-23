using System;
using System.Threading;
using System.Threading.Tasks;
using MultiThreading.Task6.Continuation.Interfaces;

namespace MultiThreading.Task6.Continuation.Commands
{
    public class DCommand : ICommand
    {
        public Task Run(Action mainAction, CancellationToken cancellationToken)
        {
            var task = Task.Factory.StartNew(mainAction, cancellationToken)
                .ContinueWith(x => CommandManager.ContinuationTaskExecution("D"), TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);
            return task;
        }
    }
}