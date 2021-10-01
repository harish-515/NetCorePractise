using System;
using System.Collections.Generic;

namespace GradeBook
{
    class Program
    {
        static void OnGradeAdded(object sender,EventArgs args)
        {
            Console.WriteLine("Grade has been Added");
        }

        static void Main(string[] args)
        {
            var book = new DiskBook("Demo Grade Book");
            book.GradeAdded += OnGradeAdded;


            Console.WriteLine("Enter the grades or 'q'to quit");

            EnterGrades(book);

            var stats = book.GetStatistics();
            Console.WriteLine($"The highest grade : {stats.High}");
            Console.WriteLine($"The lowest grade : {stats.Low}");
            Console.WriteLine($"The average grade : {stats.Average:N1}");
            Console.WriteLine($"The average letter grade : {stats.Letter}");


        }

        private static void EnterGrades(IBook book)
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (input == "q")
                    break;

                try
                {
                    var grade = double.Parse(input);
                    book.AddGrade(grade);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    // .. code to run in finally block
                }
            }
        }
    }
}
