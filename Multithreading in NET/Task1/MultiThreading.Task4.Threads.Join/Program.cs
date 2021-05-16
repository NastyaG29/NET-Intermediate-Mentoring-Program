/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private static Semaphore _semaphore;

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");
            Console.WriteLine();

            var threadAmount = 10;

            //var thread = new Thread(ExecuteA);
            //thread.Start(threadAmount);

            _semaphore = new Semaphore(1, 1);
            ThreadPool.QueueUserWorkItem(ExecuteB, threadAmount);

            Console.ReadLine();
        }

        public static void ExecuteA(object state)
        {
            var counter = (int) state;
            counter--;
            Console.WriteLine($"Counter: {counter}");

            if (counter == 0)
            {
                return;
            }

            var thread = new Thread(ExecuteA);
            thread.Start(counter);
            thread.Join();
        }

        private static void ExecuteB(object state)
        {
            _semaphore.WaitOne();

            var counter = (int)state;
            counter--;
            Console.WriteLine($"Counter: {counter}");

            if(counter == 0)
                return;

            ThreadPool.QueueUserWorkItem(ExecuteB, counter);

            _semaphore.Release();
        }
    }
}