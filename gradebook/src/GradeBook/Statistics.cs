namespace GradeBook
{
    public class Statistics
    {
        public Statistics(double average,double high,double low,char letter)
        {
            this.Average = average;
            this.High = high;
            this.Low = low;
            this.Letter = letter;
        }
        public double Average;
        public double High;
        public double Low;
        public char Letter;
    }
}