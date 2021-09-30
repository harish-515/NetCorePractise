using System;
using System.Collections.Generic;

namespace GradeBook
{
    public class Book
    {
        private List<double> grades;
        public string Name;

        public Book(string name){
            Name = name;
            this.grades = new List<double>();
        }

        public void AddGrade(double grade){
            this.grades.Add(grade);
        }

        public Statistics GetStatistics(){
            var highGrade = double.MinValue;
            var lowGrade = double.MaxValue;
            var gradesTotal = 0.0;

            foreach(var grade in this.grades){
                lowGrade = Math.Min(grade,lowGrade);
                highGrade = Math.Max(grade,highGrade);
                gradesTotal += grade;
            }

            var avgGrade = gradesTotal / this.grades.Count;

            return new Statistics(avgGrade,highGrade,lowGrade);
        }
    }
}