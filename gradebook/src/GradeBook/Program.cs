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

            book.ShowStatistics();
        }
    }
}
