using System;
using System.Threading;
using MultiThreading.Task6.Continuation.Commands;
using MultiThreading.Task6.Continuation.Interfaces;

namespace MultiThreading.Task6.Continuation
{
    public static class CommandManager
    {
        public static void MainTaskExecution(CancellationToken cancellationToken, bool terminate, bool checkCancel)
        {
            Console.WriteLine("Main Task Execution");

            Thread.Sleep(1000);

            if (terminate)
                throw new Exception("Task is failed.");

            if (checkCancel)
                cancellationToken.ThrowIfCancellationRequested();
        }

        public static void ContinuationTaskExecution(string commandName)
        {
            Console.WriteLine($"Continuation Task Execution. Command: {commandName}");
        }

        public static ICommand GetCommand(string command)
        {
            switch (command)
            {
                case "A":
                    return new ACommand();
                case "B":
                    return new BCommand();
                case "C":
                    return new CCommand();
                case "D":
                    return new DCommand();
                default:
                    throw new ArgumentException($"{command} is not corresponded to any command.");
            }
        }
    }
}