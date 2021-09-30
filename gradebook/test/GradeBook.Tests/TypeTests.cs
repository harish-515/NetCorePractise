using System;
using Xunit;

namespace GradeBook.Tests
{

    public delegate string WriteLogDelegate(string msg);
    public class TypeTests
    {
        int count = 0;
        [Fact]
        public void WriteLogDelegateMulticastTest()
        {

            WriteLogDelegate log = new WriteLogDelegate(ReturnMessageMulticast);

            log += ReturnMessageMulticast;

            var result = log("Hello");

            Assert.Equal(2,count);
        }

        [Fact]
        public void WriteLogDelegateTest()
        {
            WriteLogDelegate log;
            
            //log = new WriteLogDelegate(ReturnMessage);

            log = ReturnMessage;

            var result = log("Hello");

            Assert.Equal("Hello",result);
        }

        string ReturnMessage(string message)
        {
            return message;
        }

        string ReturnMessageMulticast(string message)
        {
            count++;
            return message;
        }

        [Fact]
        public void IntReturnValueTypeTests()
        {
            var x = GetInt();

            Assert.Equal(3,x);
        }

        private int GetInt()
        {
            return 3;
        }


        [Fact]
        public void IntPassByValueTests()
        {
            var x = GetInt();
            SetInt(x);

            Assert.Equal(3,x);
        }

        private void SetInt(int z)
        {
            z = 43;
        }


        [Fact]
        public void IntPassByReferenceTests()
        {
            var x = GetInt();
            SetInt(ref x);

            Assert.Equal(43,x);
        }

        private void SetInt(ref int z)
        {
            z = 43;
        }

        [Fact]
        public void StringBehavesAsValueType()
        {
            var book1 = GetBook("Book 1");
            var book2 = GetBook("Book 2");

            Assert.Equal("Book 1",book1.Name);
            Assert.Equal("Book 2",book2.Name);

            Assert.NotSame(book1,book2);
        }

        [Fact]
        public void TwoVariablesReferenceSameObject()
        {
            var book1 = GetBook("Book 1");
            var book2 = book1;

            Assert.Equal("Book 1",book1.Name);
            Assert.Equal("Book 1",book2.Name);

            Assert.Same(book1,book2);
        }

        [Fact]
        public void SettingNewNameToAnExistingBook()
        {
            var book1 = GetBook("Book 1");
            SetName(book1,"New Name");

            Assert.Equal("New Name",book1.Name);
        }

        private void SetName(Book book, string name)
        {
            book.Name = name;
        }


        [Fact]
        public void PassByValueTest()
        {
            var book1 = GetBook("Book 1");
            GetBookSetName(book1,"New Name");

            Assert.Equal("Book 1",book1.Name);            
        }

        private void GetBookSetName(Book book, string name)
        {
            book = new Book(name);
        }

        [Fact]
        public void PassByReferenceTest()
        {
            var book1 = GetBook("Book 1");
            var book1Copy = book1;
            GetBookSetName(ref book1,"New Name");

            Assert.Equal("New Name",book1.Name);
            Assert.NotSame(book1Copy,book1);            
        }

        private void GetBookSetName(ref Book book, string name)
        {
            book = new Book(name);
        }

        private Book GetBook(string name)
        {
            return new Book(name);
        }
    }
}
