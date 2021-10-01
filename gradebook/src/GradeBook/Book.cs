using System;
using System.Collections.Generic;
using System.IO;

namespace GradeBook
{
    public delegate void GradeAddedDelegate(object sender,EventArgs args);
    
    public class NamedObject {
        private string name;

        public NamedObject(string name)
        {
            Name = name;
        }

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
    }
    
    public interface IBook
    {
        void AddGrade(double grade);
        Statistics GetStatistics();
        string Name {get;}
        event GradeAddedDelegate GradeAdded;
    }
    
    public abstract class Book : NamedObject,IBook
    {
        public Book(string name) : base(name)
        {
        }

        public abstract event GradeAddedDelegate GradeAdded;

        public abstract void AddGrade(double grade);

        public abstract Statistics GetStatistics();

    }

    public class InMemoryBook : Book
    {
        private List<double> grades;
        public InMemoryBook(string name) : base(name)
        {
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

        public override void AddGrade(double grade){
            if(grade >= 0 && grade <= 100)
            {
                this.grades.Add(grade);
                if(GradeAdded != null)
                    GradeAdded(this,new EventArgs());
            }
            else
            {
                throw new ArgumentException($"Invalid {nameof(grade)} Value");
            }
        }

        public override event GradeAddedDelegate GradeAdded;

        public override Statistics GetStatistics()
        {
            var results = new Statistics();
            for(var index =0;index < grades.Count;index++)
            {
                results.Add(grades[index]);
            }
            return results;
        }
    }

    public class DiskBook : Book
    {
        public DiskBook(string name) : base(name)
        {

        }

        public override event GradeAddedDelegate GradeAdded;

        public override void AddGrade(double grade)
        {
            using(var writter = File.AppendText($"{Name}.txt"))
            {
                writter.WriteLine(grade);
                if(GradeAdded!=null)
                {
                    GradeAdded(this,new EventArgs());
                }
            }
        }

        public override Statistics GetStatistics()
        {
            var results = new Statistics();
            using(var reader  = File.OpenText($"{Name}.txt"))
            {
                while(!reader.EndOfStream)
                {
                    results.Add(Double.Parse(reader.ReadLine()));
                }
            }
            return results;
        }
    }
}