/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    public class Program
    {
        private const int ArrayLength = 10;
        private static readonly Random Random;

        static Program()
        {
            Random = new Random();
        }

        public static void Main()
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            Task.Factory.StartNew(GetArrayOfIntegers)
                .ContinueWith(task => GetMultiplied(task.Result))
                .ContinueWith(task => GetSortedByAscending(task.Result))
                .ContinueWith(task => GetAverage(task.Result));

            Console.ReadLine();
        }

        private static List<int> GetArrayOfIntegers()
        {
            var numbers = new List<int>();
            for (var i = 0; i < ArrayLength; i++)
            {
                numbers.Add(GetRandomInteger());
            }

            Console.WriteLine($"Array: {string.Join(", ", numbers)}");

            return numbers;
        }

        private static List<int> GetMultiplied(List<int> array)
        {
            var randomInteger = GetRandomInteger();
            var result = array.Select(i => i * randomInteger).ToList();
            Console.WriteLine($"Multiplier: {randomInteger}; Array: {string.Join(", ", result)}");

            return result;
        }

        private static List<int> GetSortedByAscending(List<int> array)
        {
            var result = array.OrderBy(i => i).ToList();
            Console.WriteLine($"Sorted array: {string.Join(", ", result)}");

            return result;
        }

        private static double GetAverage(List<int> array)
        {
            var result = array.Average();
            Console.WriteLine($"Average: {result}");

            return result;
        }

        private static int GetRandomInteger() => Random.Next(int.MinValue, int.MaxValue);
    }
}