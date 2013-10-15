using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStrongNamedDll
{
    public class Class1
    {
        public Class1()
        { }

        public static bool TrueOrFalse()
        {
            return Convert.ToBoolean(new Random().Next() % 2);
        }
    }    
}
