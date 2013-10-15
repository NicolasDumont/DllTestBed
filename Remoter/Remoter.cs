using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace Remoter
{
    ///Wrapping class managing remote invocation.
    public class Remoter : MarshalByRefObject, IDisposable
    {
        public Remoter()
        {
            //Create an AppDomain in which to run our DLL code
            AppDomainSetup appDomainSetup = new AppDomainSetup();
            appDomainSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;  //Environment.CurrentDirectory
            disposableAppDomain = AppDomain.CreateDomain("DisposableAppDomain" + Guid.NewGuid().ToString(), null, appDomainSetup);
            //disposableAppDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            //Create the remoteLoaderFactory class in the secondary app-domain
            //remoteLoaderFactory = (RemoteLoaderFactory)disposableAppDomain.CreateInstanceAndUnwrap("RemoteLoader", "RemoteLoader.RemoteLoaderFactory");
            remoteContainer = (RemoteContainer)disposableAppDomain.CreateInstanceAndUnwrap("RemoteContainer", "RemoteLoader.RemoteContainer");
        }

        public IRemote InstanciateRemotely(string assemblyFile, string typeName, object[] constructorArguments)
        {
            try
            {
                //With help of remoteLoaderFactory, create a real 'LiveClass' instance in the new appDomain without loading type information in the current AppDomain
                //return (IRemote)remoteLoaderFactory.Create(assemblyFile, typeName, constructorArguments);
                remoteContainer.CreateInstance(assemblyFile, typeName, constructorArguments);
                return (IRemote)remoteContainer;
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't load class " + typeName + " in " + assemblyFile + " because:" + Environment.NewLine + ex.ToString());
            }
        }

        public object Invoke(IRemote remoteObject, string methodName, object[] methodParameters)
        {
            try
            {
                //Indirectly call the remote interface
                //return remoteObject.Invoke(methodName, methodParameters);
                return remoteContainer.Invoke(methodName, methodParameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't invoke " + methodName + " because:" + Environment.NewLine + ex.ToString());
            }
        }

        private AppDomain disposableAppDomain;
        private RemoteContainer remoteContainer;

        /*Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                Assembly assembly = System.Reflection.Assembly.Load(args.Name);
                if (assembly != null)
                {
                    return assembly;
                }
            }
            catch 
            { 
                // ignore load error 
            }
            // *** Try to load by filename - split out the filename of the full assembly name
            // *** and append the base path of the original assembly (ie. look in the same dir)
            // *** NOTE: this doesn't account for special search paths but then that never
            //           worked before either.
            string[] Parts = args.Name.Split(',');
            string File = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + Parts[0].Trim() + ".dll";

            return Assembly.LoadFrom(File);
        }*/

        #region IDisposable Members

        public void Dispose()
        {
            if (disposableAppDomain != null)
            {
                AppDomain.Unload(disposableAppDomain);
                disposableAppDomain = null;
            }
        }

        #endregion
    }
}
