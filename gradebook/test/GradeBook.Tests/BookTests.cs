using System;
using Xunit;

namespace GradeBook.Tests
{
    public class ValueTypeTests
    {
        [Fact]
        public void StringBehavesAsValueType()
        {
            string name = "scott";
            string upper = MakeUppercase(name);
            Assert.Equal("SCOTT",upper);            
        }

        private string MakeUppercase(string name)
        {
            return name.ToUpper();
        }
    }
    public class BookTests2
    {
        [Fact]
        public void Test1()
        {
            //arrage
            var book = new Book("");
            book.AddGrade(89.1);
            book.AddGrade(90.5);
            book.AddGrade(77.3);

            //act
            var result = book.GetStatistics();

            //assert
            Assert.Equal(85.6,result.Average,1);
            Assert.Equal(90.5,result.High);
            Assert.Equal(77.3,result.Low);
        }
    }
}
