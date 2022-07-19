using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.Web.Services
{
    // The purpose of this class is to do a POC for code coverage.
    // This is not relavant to the project in any way. Taken from the following link.
    // https://www.code4it.dev/blog/code-coverage-vs-2019-coverlet#coverlet---the-nuget-package-for-code-coverage

    public class MyArray
    {
        private readonly int[] _myArr;

        public int[] Array => _myArr;

        public MyArray(int size)
        {
            _myArr = Enumerable.Range(0, size).ToArray();
        }

        public bool Replace(int position, int newValue)
        {
            if (position < 0)
                throw new ArgumentException("Position must not be less than zero");

            if (position >= _myArr.Length)
                throw new ArgumentException("Position must be valid");

            _myArr[position] = newValue;
            return true;
        }
    }
}
