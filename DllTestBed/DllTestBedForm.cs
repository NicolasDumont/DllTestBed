using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

//Best practices for assembly loading: http://msdn.microsoft.com/en-us/library/dd153782.aspx
//Assembly (loading) log viewer: http://msdn.microsoft.com/en-us/library/e74a18c4%28vs.71%29.aspx ,
//in Windows 7 using Visual Sudio 2010: C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\NETFX 4.0 Tools\x64\FUSLOGVW.exe

namespace DllTestBed
{
    public partial class DllTestBedForm : Form
    {
        public DllTestBedForm()
        {
            InitializeComponent();
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            try
            {
                using (Remoting.InvocationManager invocationManager = new Remoting.InvocationManager(/*dllRootProbingDirectory, dllProbingDirectories.ToArray()*/))
                {
                    string assemblyFile = @"C:\DEV\DllTestBed\SampleDlls\NotStrongNamedAssemblyDependentDll.dll";
                    string typeName = "NotStrongNamedAssemblyDependentDll.Class1";
                    List<string> assembliesLookupPaths = new List<string>();
                    assembliesLookupPaths.Add(@"C:\DEV\DllTestBed\Another folder somewhere");
                    Remoting.IRemote remoteObject1 = invocationManager.InstanciateRemotely(assemblyFile, typeName, null, assembliesLookupPaths.ToArray());
                    object[] constructorParameters = new object[1];
                    constructorParameters[0] = 5;
                    string randomString = (string)invocationManager.Invoke(remoteObject1, "GetRandomString", constructorParameters);
                    MessageBox.Show("NotStrongNamedAssemblyDependentDll generated string: " + randomString);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonRun2_Click(object sender, EventArgs e)
        {
            //Creating and Using Strong-Named Assemblies: http://msdn.microsoft.com/en-us/library/xwb8f617.aspx
            //Dot NET Assemblies and Strong Name Signature: http://resources.infosecinstitute.com/dot-net-assemblies-and-strong-name-signature/
            try
            {
                using (Remoting.InvocationManager invocationManager = new Remoting.InvocationManager())
                {
                    string assemblyFile = @"C:\DEV\DllTestBed\SampleDlls\StrongNamedAssemblyDependentDll.dll";
                    string typeName = "StrongNamedAssemblyDependentDll.Class1";
                    List<string> assembliesLookupPaths = new List<string>();
                    assembliesLookupPaths.Add(@"C:\DEV\DllTestBed\Another folder somewhere");
                    Remoting.IRemote remoteObject = invocationManager.InstanciateRemotely(assemblyFile, typeName, null, assembliesLookupPaths.ToArray());
                    bool answer = (bool)invocationManager.Invoke(remoteObject, "StrongNamedAssemblyCall", null);
                    MessageBox.Show("Réponse: " + answer.ToString());
                    //The second call will, as expected, not have to load the assembly in the disposableAppDomain again
                    //Remoting.IRemote remoteObject2 = invocationManager.InstanciateRemotely(assemblyFile, typeName, null, assembliesLookupPaths.ToArray());
                    //bool answer2 = (bool)invocationManager.Invoke(remoteObject2, "StrongNamedAssemblyCall", null);
                    //MessageBox.Show("Réponse: " + answer2.ToString());
                }
                //Some strongly named assemblies (MercatorTunnel.dll, ...) stay locked while used through the debugged application in Visual Studio,
                //but not when used through the same application launched normally.
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
