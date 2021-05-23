/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static readonly List<int> _sharedCollection = new List<int>();
        private static readonly ManualResetEventSlim _addEventSlim = new ManualResetEventSlim(false);
        private static readonly ManualResetEventSlim _printEventSlim = new ManualResetEventSlim(true);
        private static readonly object _locker = new object();
        private static bool isPrintingRequired = true;

        private const int WaitingTimeout = 1000;

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var addTask = Task.Factory.StartNew(AddElements);
            var printTask = Task.Factory.StartNew(PrintElements);
            Task.WaitAll(addTask, printTask);

            Console.ReadLine();
        }

        public static void AddElements()
        {
            for (var i = 0; i < 10; i++)
            {
                if (_printEventSlim.Wait(WaitingTimeout))
                {
                    try
                    {
                        _printEventSlim.Reset();
                        lock (_locker)
                        {
                            _sharedCollection.Add(i);
                        }
                    }
                    finally
                    {
                        _addEventSlim.Set();
                    }
                }
            }

            isPrintingRequired = false;
            Console.WriteLine("Adding task is ended.");
        }

        public static void PrintElements()
        {
            while (isPrintingRequired)
            {
                if (_addEventSlim.Wait(WaitingTimeout))
                {
                    try
                    {
                        _addEventSlim.Reset();
                        lock (_locker)
                        {
                            _sharedCollection.ForEach(e => Console.Write(e + " "));
                            Console.WriteLine();
                        }
                    }
                    finally
                    {
                        _printEventSlim.Set();
                    }
                }
            }

            Console.WriteLine("Printing task is ended.");
        }
    }
}