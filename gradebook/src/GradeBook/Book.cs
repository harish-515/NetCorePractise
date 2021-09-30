using System;
using System.Collections.Generic;

namespace GradeBook
{
    class Book
    {

        private string name;
        private List<double> grades;

        public Book(string name){
            this.name = name;
            this.grades = new List<double>();
        }

        public void AddGrade(double grade){
            this.grades.Add(grade);
        }

        public void ShowStatistics(){
            var highGrade = double.MinValue;
            var lowGrade = double.MaxValue;
            var gradesTotal = 0.0;

            foreach(var number in this.grades){
                lowGrade = Math.Min(number,lowGrade);
                highGrade = Math.Max(number,highGrade);
                gradesTotal += number;
            }

            var avgGrade = gradesTotal / this.grades.Count;

            Console.WriteLine($"The highest grade : {highGrade}");
            Console.WriteLine($"The lowest grade : {lowGrade}");
            Console.WriteLine($"The average grade : {avgGrade:N1}");
        }
    }
}