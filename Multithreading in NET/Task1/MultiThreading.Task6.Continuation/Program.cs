/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using MultiThreading.Task6.Continuation.Interfaces;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            while (true)
            {
                try
                {
                    Console.WriteLine("Enter command literal.");
                    var commandLiteral = Console.ReadLine();
                    var command = CommandManager.GetCommand(commandLiteral);

                    Console.WriteLine("Would you like to terminate main task?");
                    var terminateLiteral = Console.ReadLine();
                    var terminate = terminateLiteral == "y";

                    Console.WriteLine("Would you like to cancel main task?");
                    var cancelLiteral = Console.ReadLine();
                    var cancel = cancelLiteral == "y";

                    Execute(command, terminate, cancel);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void Execute(ICommand command, bool terminate, bool checkCancel)
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                Console.WriteLine("Task is started.");

                var token = cancellationTokenSource.Token;
                var task = command.Run(() => CommandManager.MainTaskExecution(token, terminate, checkCancel),
                    token);
                if (checkCancel)
                    cancellationTokenSource.CancelAfter(500);
                task.Wait(token);

                Console.WriteLine("Task is ended.");
            }
        }
    }
}