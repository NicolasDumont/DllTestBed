using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.IO;

namespace Remoting
{
    ///Proxy loader class that returns an interface reference.
    public class RemoteProxy : CrossAppDomainObject, IRemote
    {
        public RemoteProxy()
        {
            assembliesLookupPaths = new List<string>();
            //Appdomain.AppendPrivatePath is obsolete: http://blogs.msdn.com/b/dotnet/archive/2009/05/14/why-is-appdomain-appendprivatepath-obsolete.aspx?Redirected=true
            //So we use the "AssemblyResolve event" method (instead of the "AppDomainSetup.PrivateBinPath" method because
            //the latter would involve setting up "Appdomain.ApplicationBase" to a common root for the loadable assemblies;
            //indeed, the "AssemblyResolve event" lets us search anywhere we want and even add probing paths while running).
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }

        public void CreateInstance(string assemblyFile, string typeName, object[] constructorArguments)
        {
            embeddedObject = Activator.CreateInstanceFrom(assemblyFile, typeName, false, bindingFlags, null, constructorArguments,
               null, null, null).Unwrap();
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string assemblyName = args.Name.Split(',')[0].Trim();
            foreach (string path in assembliesLookupPaths)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                FileInfo[] filesInfo = directoryInfo.GetFiles(assemblyName + ".dll", SearchOption.TopDirectoryOnly);
                FileInfo assemblyFile = Array.Find(filesInfo, delegate(FileInfo f) { return f.Name.Equals(assemblyName + ".dll"); });
                    //directoryInfo.GetFiles().FirstOrDefault(i => i.Name == args.Name + ".dll");
                if (assemblyFile != null)
                {
                    return Assembly.LoadFrom(assemblyFile.FullName);
                }
            }
            return null;
        }

        #region IRemote Members

        public object Invoke(string methodName, object[] methodParameters)
        {
            object returnValue = null;
            try
            {
                //I am in a disposable AppDomain, so I can load (and lock) the assembly using Reflection
                returnValue = embeddedObject.GetType().InvokeMember(methodName, BindingFlags.InvokeMethod, null, embeddedObject, methodParameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return returnValue;
        }

        #endregion

        //Live indefinitely while the (disposable) AppDomain in which I run is alive
        //We should instead register a sponsor, or better?
        //http://nbevans.wordpress.com/2011/04/17/memory-leaks-with-an-infinite-lifetime-instance-of-marshalbyrefobject/
        /*public override object InitializeLifetimeService()
        {
            return null;
        }*/

        public void AddAssembliesLookupPath(string path)
        {
            if (!assembliesLookupPaths.Contains(path))
            {
                assembliesLookupPaths.Add(path);
            }
        }

        public void AddAssembliesLookupPaths(string[] paths)
        {
            foreach (string path in paths)
            {
                AddAssembliesLookupPath(path);
            }
        }

        private object embeddedObject;
        private const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance;
        private List<string> assembliesLookupPaths;
    }
}
