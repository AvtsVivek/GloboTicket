using GloboTicket.Web.Services;
using System;
using Xunit;

namespace GloboTicket.Web.Tests
{
    // The purpose of this class is to do a POC for code coverage.
    // This is not relavant to the project in any way. Taken from the following link.
    // https://www.code4it.dev/blog/code-coverage-vs-2019-coverlet#coverlet---the-nuget-package-for-code-coverage

    public class MyArrayTests
    {
        [Fact]
        public void MyArray_Should_ReplaceValue_When_PositionIsValid()
        {
            var arr = new MyArray(5);
            var result = arr.Replace(2, 99);

            Assert.True(result);
            Assert.Equal(99, arr.Array[2]);
            
        }

        [Fact]
        public void MyArray_Should_ThrowException_When_PositionIsLessThanZero()
        {
            var arr = new MyArray(5);
            Assert.Throws<ArgumentException>(() => arr.Replace(-8, 0));
        }
    }
}
