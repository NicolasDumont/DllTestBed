using System;
using System.Collections.Generic;
using System.Text;

//This dll uses the MercatorTunnel.dll strong named assembly
//Strong named assembly: http://msdn.microsoft.com/en-us/library/wd40t7ad.aspx
namespace StrongNamedAssemblyDependentDll
{
    public class Class1
    {
        public Class1()
        {

        }

        public bool StrongNamedAssemblyCall()
        {
            return MercatorApi.Api.Answer("Could we call a function in a strong named assembly ?", "MercatorTunnel.dll Api.Answer");
            //return SimpleStrongNamedDll.Class1.TrueOrFalse();
        }

        /*public static bool StaticMercatorAnswer()
        {
            return MercatorApi.Api.Answer("Could we call a function in a strong named assembly ?", "MercatorTunnel.dll Api.Answer");
        }*/
    }
}
