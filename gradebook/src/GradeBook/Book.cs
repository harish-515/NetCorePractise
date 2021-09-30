using System;
using System.Collections.Generic;

namespace GradeBook
{
    public class Book
    {
        private List<double> grades;
        private string name;
        public string Name
        {
            get 
            {
                return name;
            }
            set
            {
                if(!string.IsNullOrEmpty(value))
                {
                    name = value;
                }
                else{
                    throw new ArgumentException($"invalid {nameof(Name)}");
                }
            }
        }

        public Book(string name){
            Name = name;
            this.grades = new List<double>();
        }

        public void AddGrade(char letter)
        {
            switch(letter){
                case 'A':
                    AddGrade(90);
                    break;
                case 'B':
                    AddGrade(80);                
                    break;
                case 'C':
                    AddGrade(70);                
                    break;                                                                              
                default:
                    AddGrade(0);                
                    break;            }
        }

        public void AddGrade(double grade){
            if(grade >= 0 && grade <= 100)
            {
                this.grades.Add(grade);
            }
            else
            {
                throw new ArgumentException($"Invalid {nameof(grade)} Value");
            }
        }

        public Statistics GetStatistics()
        {
            var highGrade = double.MinValue;
            var lowGrade = double.MaxValue;
            var gradesTotal = 0.0;

            foreach(var grade in this.grades){
                lowGrade = Math.Min(grade,lowGrade);
                highGrade = Math.Max(grade,highGrade);
                gradesTotal += grade;
            }

            var avgGrade = gradesTotal / this.grades.Count;
            var letterGrade = 'F';
            switch(avgGrade)
            {
                case var d when d > 90.0 :
                    letterGrade = 'A';
                    break;
                case var d when d > 80.0 :
                    letterGrade = 'B';
                    break;
                case var d when d > 70.0 :
                    letterGrade = 'C';
                    break;
                case var d when d > 60.0 :
                    letterGrade = 'D';
                    break;
                default:
                    letterGrade = 'F';
                    break;                                                                                                  
            }    

            return new Statistics(avgGrade,highGrade,lowGrade,letterGrade);
        }
    }
}