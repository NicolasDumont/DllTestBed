using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Windows.Forms;

//What are AppDomains: http://blogs.msdn.com/b/cbrumme/archive/2003/06/01/51466.aspx
//Discover Techniques for Safely Hosting Untrusted Add-Ins with the .NET Framework 2.0:
//http://msdn.microsoft.com/fr-fr/magazine/cc163701%28en-us%29.aspx

namespace Remoting
{
    ///Wrapping class managing remote invocation.
    public class InvocationManager : CrossAppDomainObject, IDisposable
    {
        public InvocationManager(/*string applicationBase, string[] privateBinPaths*/)
        {
            //Create an AppDomain in which to run code from our DLL(s)
            AppDomainSetup appDomainSetup = new AppDomainSetup();
            disposableAppDomain = AppDomain.CreateDomain("DisposableAppDomain" + Guid.NewGuid().ToString(), null, appDomainSetup);
        }

        public IRemote InstanciateRemotely(string assemblyFile, string typeName, object[] constructorArguments, string[] assembliesLookupPaths)
        {
            try
            {
                //Create the RemoteProxy object (wrapper that will contain the instance of our target dll class) in the secondary app-domain
                RemoteProxy remoteProxy = (RemoteProxy)disposableAppDomain.CreateInstanceAndUnwrap("RemoteProxy", "Remoting.RemoteProxy");
                foreach(string path in assembliesLookupPaths)
                {
                    if (FileTools.CanRead(path))
                    {
                        remoteProxy.AddAssembliesLookupPath(path);
                    }
                    /*else
                    {
                        throw new Exception("Cannot access assembly probing path:" + Environment.NewLine + path);
                    }*/
                }
                //Create a real 'LiveClass' instance in the new appDomain without loading type information in the current AppDomain
                remoteProxy.CreateInstance(assemblyFile, typeName, constructorArguments);
                return (IRemote)remoteProxy;
            }
            catch (Exception ex)
            {
                /*throw new Exception*/MessageBox.Show("(InvocationManager.InstanciateRemotely) Couldn't load '" + typeName + "' class in " + 
                    assemblyFile + " because:" + Environment.NewLine + ex.ToString());
                return null;
            }
        }

        public object Invoke(IRemote remoteObject, string methodName, object[] methodParameters)
        {
            try
            {
                //Indirectly call the remote interface
                return remoteObject.Invoke(methodName, methodParameters);
            }
            catch (Exception ex)
            {
                throw new Exception("(InvocationManager.Invoke) Couldn't invoke '" + methodName + "' method:" + Environment.NewLine + ex.ToString());
            }
        }

        //Live indefinitely while the (disposable) AppDomain in which I run is alive
        /*public override object InitializeLifetimeService()
        {
            return null;
        }*/

        #region IDisposable Members

        public new void Dispose()
        {
            if (disposableAppDomain != null)
            {
                AppDomain.Unload(disposableAppDomain);
                disposableAppDomain = null;
            }
            //Call CrossAppDomainObject.Dispose()
            base.Dispose();
        }

        #endregion

        private AppDomain disposableAppDomain;
        private const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance;
    }

    internal class FileTools
    {
        //Check access to a directory
        //http://stackoverflow.com/questions/11709862/check-if-directory-is-accessible-in-c
        public static bool CanRead(string path)
        {
            var readAllow = false;
            var readDeny = false;
            var accessControlList = Directory.GetAccessControl(path);
            if (accessControlList == null)
                return false;
            var accessRules = accessControlList.GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));
            if (accessRules == null)
                return false;

            foreach (System.Security.AccessControl.FileSystemAccessRule rule in accessRules)
            {
                if ((System.Security.AccessControl.FileSystemRights.Read & rule.FileSystemRights) != System.Security.AccessControl.FileSystemRights.Read) continue;

                if (rule.AccessControlType == System.Security.AccessControl.AccessControlType.Allow)
                    readAllow = true;
                else if (rule.AccessControlType == System.Security.AccessControl.AccessControlType.Deny)
                    readDeny = true;
            }

            return readAllow && !readDeny;
        }
    }
}
