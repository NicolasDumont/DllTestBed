using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MercatorApi;
using MercatorUi;
using System.ComponentModel;
using System.Windows.Forms;
// <ReferenceInclude>"IRemote.dll"</ReferenceInclude>
// <ReferenceInclude>"RemoteProxy.dll"</ReferenceInclude>
// <ReferenceInclude>"InvocationManager.dll"</ReferenceInclude>
//using Remoting;

namespace MyNameSpace
{
	public class Class1 : MercatorUi.Interfaces.IExec
	{
		public void Main()
		{
			try
			{
				//Create invocationManager, a tool to instanciate classes remotely in a disposable AppDomain
				using (Remoting.InvocationManager invocationManager = new Remoting.InvocationManager())
				{
					//The assembly (dll or exe) containing our class
					string assemblyFile = @"C:\DEV\DllTestBed\SampleDlls\NotStrongNamedAssemblyDependentDll.dll";
					//Namespace.ClassName inside the assembly
					string typeName = "NotStrongNamedAssemblyDependentDll.Class1";
					List<string> assembliesLookupPaths = new List<string>();
					//Look for assemblies referenced by assemblyFile in the following directory (there can be more)
					assembliesLookupPaths.Add(@"C:\DEV\DllTestBed\Another folder somewhere");
					//Instanciate our NotStrongNamedAssemblyDependentDll.Class1 in a disposable Appdomain (born in invocationManager):
					Remoting.IRemote remoteObject1 = invocationManager.InstanciateRemotely(assemblyFile, typeName, null, assembliesLookupPaths.ToArray());
					object[] constructorParameters = new object[1];
					constructorParameters[0] = 5;
					string randomString = (string) invocationManager.Invoke(remoteObject1, "GetRandomString", constructorParameters);
					//Now the assembly referenced by assemblyFile is locked.
					MessageBox.Show("NotStrongNamedAssemblyDependentDll generated string: " + randomString);
				}
				//Now the assembly referenced by assemblyFile is unlocked, and can be overwritten without exiting Mercator!
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
	}
}