using System;
using System.Collections.Generic;
using System.Text;

namespace NotStrongNamedAssemblyDependentDll
{
    public class Class1
    {
        public Class1()
        {

        }

        public string GetRandomString(int stringSize)
        {
            //Calling a method from a not strong named dll
            return SimpleNotStrongNamedDll.LibBytes.RandomString(stringSize);
        }
    }
}
