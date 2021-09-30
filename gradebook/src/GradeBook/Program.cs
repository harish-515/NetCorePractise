using System;
using System.Collections.Generic;

namespace GradeBook
{
    class Program
    {
        static void Main(string[] args)
        {    
            var book = new Book("Demo Grade Book");
            book.AddGrade(89.5);
            book.AddGrade(19.5);
            book.AddGrade(34.5);
            book.AddGrade(90.5);

            var stats = book.GetStatistics();
            Console.WriteLine($"The highest grade : {stats.High}");
            Console.WriteLine($"The lowest grade : {stats.Low}");
            Console.WriteLine($"The average grade : {stats.Average:N1}");


        }
    }
}
