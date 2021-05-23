using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation.Interfaces
{
    public interface ICommand
    {
        Task Run(Action mainAction, CancellationToken cancellationToken);
    }
}